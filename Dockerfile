# syntax=docker/dockerfile:1
# =====================================================================
# Cidades ESGInteligentes — API de Gestão de Resíduos (.NET 8)
# Build multi-stage e multi-arquitetura (linux/amd64 e linux/arm64).
# =====================================================================

# ---------- Estágio 1: build ----------------------------------------
# O SDK roda na arquitetura da máquina que faz o build ($BUILDPLATFORM)
# e compila para a arquitetura de destino ($TARGETARCH). Assim a mesma
# imagem roda no servidor Linux (amd64) e no Mac Apple Silicon (arm64)
# sem emulação lenta durante a compilação.
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /src

# Restaura só o .csproj primeiro: enquanto as dependências não mudarem,
# essa camada vem do cache e o build fica bem mais rápido.
COPY src/ResiduosApi/ResiduosApi.csproj src/ResiduosApi/
RUN dotnet restore src/ResiduosApi/ResiduosApi.csproj -a $TARGETARCH

# Copia o restante do código e publica em Release.
COPY src/ResiduosApi/ src/ResiduosApi/
RUN dotnet publish src/ResiduosApi/ResiduosApi.csproj -c Release -a $TARGETARCH \
    --no-restore -o /app/publish /p:UseAppHost=false

# ---------- Estágio 2: runtime --------------------------------------
# Só o ASP.NET runtime (sem SDK nem código-fonte): imagem menor e com
# menos superfície de ataque.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# curl é usado apenas pelo HEALTHCHECK abaixo.
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app/publish .

# Versão (commit) gravada pelo pipeline; aparece em GET /
ARG APP_VERSION=local
ENV APP_VERSION=$APP_VERSION \
    ASPNETCORE_HTTP_PORTS=8080

EXPOSE 8080

# Roda como usuário sem privilégios (já existe nas imagens .NET 8).
USER $APP_UID

# O Docker marca o container como "healthy" só quando a API e o banco respondem.
HEALTHCHECK --interval=15s --timeout=5s --start-period=40s --retries=5 \
    CMD curl -fsS http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "ResiduosApi.dll"]
