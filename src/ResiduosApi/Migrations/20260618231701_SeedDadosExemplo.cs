using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ResiduosApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedDadosExemplo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TB_PONTO_COLETA",
                columns: new[] { "Id", "Ativo", "CapacidadeKg", "CriadoEm", "Endereco", "Latitude", "Longitude", "Nome" },
                values: new object[,]
                {
                    { 1, true, 1000m, new DateTime(2026, 1, 10, 12, 0, 0, 0, DateTimeKind.Utc), "Praça da Sé, 1", -23.5505, -46.633299999999998, "Ecoponto Centro" },
                    { 2, true, 750m, new DateTime(2026, 1, 10, 12, 0, 0, 0, DateTimeKind.Utc), "Av. Ibirapuera, 500", -23.597999999999999, -46.655999999999999, "Ecoponto Zona Sul" }
                });

            migrationBuilder.InsertData(
                table: "TB_COLETA",
                columns: new[] { "Id", "DataColeta", "PesoKg", "PontoColetaId", "Responsavel", "TipoResiduoId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc), 120.5m, 1, "Equipe A", 1 },
                    { 2, new DateTime(2026, 2, 3, 9, 0, 0, 0, DateTimeKind.Utc), 80m, 1, "Equipe A", 2 },
                    { 3, new DateTime(2026, 2, 5, 9, 0, 0, 0, DateTimeKind.Utc), 45m, 2, "Equipe B", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TB_COLETA",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TB_COLETA",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TB_COLETA",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TB_PONTO_COLETA",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TB_PONTO_COLETA",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
