using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ResiduosApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_PONTO_COLETA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Endereco = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    CapacidadeKg = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PONTO_COLETA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_TIPO_RESIDUO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Cor = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    InstrucoesDescarte = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_TIPO_RESIDUO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_COLETA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PontoColetaId = table.Column<int>(type: "int", nullable: false),
                    TipoResiduoId = table.Column<int>(type: "int", nullable: false),
                    PesoKg = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DataColeta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Responsavel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_COLETA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_COLETA_TB_PONTO_COLETA_PontoColetaId",
                        column: x => x.PontoColetaId,
                        principalTable: "TB_PONTO_COLETA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_COLETA_TB_TIPO_RESIDUO_TipoResiduoId",
                        column: x => x.TipoResiduoId,
                        principalTable: "TB_TIPO_RESIDUO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "TB_TIPO_RESIDUO",
                columns: new[] { "Id", "Cor", "InstrucoesDescarte", "Nome" },
                values: new object[,]
                {
                    { 1, "AZUL", "Mantenha seco e limpo; não amasse demais.", "PAPEL" },
                    { 2, "VERMELHO", "Lave embalagens antes de descartar.", "PLASTICO" },
                    { 3, "VERDE", "Cuidado com cacos; não descarte espelhos/lâmpadas aqui.", "VIDRO" },
                    { 4, "AMARELO", "Latas e tampas limpas.", "METAL" },
                    { 5, "MARROM", "Restos de alimentos; ideal para compostagem.", "ORGANICO" },
                    { 6, "LARANJA", "Leve a pontos de logística reversa específicos.", "ELETRONICO" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_COLETA_PontoColetaId",
                table: "TB_COLETA",
                column: "PontoColetaId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_COLETA_TipoResiduoId",
                table: "TB_COLETA",
                column: "TipoResiduoId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_TIPO_RESIDUO_Nome",
                table: "TB_TIPO_RESIDUO",
                column: "Nome",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_COLETA");

            migrationBuilder.DropTable(
                name: "TB_PONTO_COLETA");

            migrationBuilder.DropTable(
                name: "TB_TIPO_RESIDUO");
        }
    }
}
