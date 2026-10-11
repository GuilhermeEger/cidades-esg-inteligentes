using System.Text;
using System.Text.Json;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ResiduosApi.Data;
using ResiduosApi.Mapping;
using ResiduosApi.Middleware;
using ResiduosApi.Repositories;
using ResiduosApi.Security;
using ResiduosApi.Services;

var builder = WebApplication.CreateBuilder(args);

// ----- Configurações tipadas (Jwt / Auth) -----
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("Auth"));
var jwt = builder.Configuration.GetSection("Jwt").Get<JwtSettings>() ?? new JwtSettings();

// ----- EF Core 8 + SQL Server -----
// EnableRetryOnFailure: em container o banco pode demorar a aceitar conexões
// (ou cair por instantes); o EF tenta de novo em vez de derrubar a API.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.EnableRetryOnFailure(maxRetryCount: 5)));

// ----- Health check (Fase 6 / DevOps) -----
// Usado pelo HEALTHCHECK do Docker, pelo docker compose --wait e pelos smoke
// tests do pipeline: "Healthy" só quando a API responde E o banco conecta.
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>("banco-de-dados");

// ----- AutoMapper (Cap 06/07) -----
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

// ----- Injeção de dependência: repositórios e serviços (AddScoped) -----
builder.Services.AddScoped<ITipoResiduoRepository, TipoResiduoRepository>();
builder.Services.AddScoped<IPontoColetaRepository, PontoColetaRepository>();
builder.Services.AddScoped<IColetaRepository, ColetaRepository>();
builder.Services.AddScoped<ITipoResiduoService, TipoResiduoService>();
builder.Services.AddScoped<IPontoColetaService, PontoColetaService>();
builder.Services.AddScoped<IColetaService, ColetaService>();
builder.Services.AddScoped<IIndicadorService, IndicadorService>();
builder.Services.AddSingleton<ITokenService, TokenService>();

// ----- Autenticação JWT -----
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret))
        };
    });
builder.Services.AddAuthorization();

// ----- Versionamento de API -----
builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddControllers();

// ----- Swagger / OpenAPI com suporte a JWT -----
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API ESG — Gestão de Resíduos", Version = "v1" });

    var scheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe: Bearer {token}",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };
    c.AddSecurityDefinition("Bearer", scheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement { { scheme, Array.Empty<string>() } });
});

var app = builder.Build();

// Aplica migrações automaticamente no startup (exceto em testes / provider não-relacional).
if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (db.Database.IsRelational())
        db.Database.Migrate();
}

// Swagger: sempre em Development; nos demais ambientes depende de Swagger:Enabled
// (ligado em staging para testes, desligado em produção por segurança).
if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("Swagger:Enabled"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware global de exceções no início do pipeline.
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// GET /health — status da API e do banco em JSON.
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            duracaoMs = Math.Round(report.TotalDuration.TotalMilliseconds, 1),
            checks = report.Entries.ToDictionary(e => e.Key, e => e.Value.Status.ToString())
        }));
    }
});

// GET / — identifica o ambiente e a versão (commit) que está no ar.
// O pipeline grava APP_VERSION na imagem; assim dá para provar qual build
// foi promovido de staging para produção.
app.MapGet("/", (IWebHostEnvironment env, IConfiguration config) => Results.Ok(new
{
    aplicacao = "Cidades ESGInteligentes — API de Gestão de Resíduos",
    ambiente = env.EnvironmentName,
    versao = config["APP_VERSION"] ?? "local",
    swagger = app.Environment.IsDevelopment() || config.GetValue<bool>("Swagger:Enabled") ? "/swagger" : "desabilitado",
    health = "/health",
    horarioUtc = DateTime.UtcNow
})).ExcludeFromDescription();

app.Run();

// Necessário para o WebApplicationFactory<Program> nos testes de integração (xUnit).
public partial class Program { }
