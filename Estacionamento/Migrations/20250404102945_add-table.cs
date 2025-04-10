using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estacionamento.Migrations
{
    /// <inheritdoc />
    public partial class addtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrosEstacionamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlacaCarro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HorarioEntrada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HorarioSaida = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValorPagar = table.Column<double>(type: "float", nullable: false),
                    MetodoPagamento = table.Column<int>(type: "int", nullable: false),
                    Pago = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosEstacionamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TabelaValores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrecoPorMeiaHora = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TempoToleranciaMinutos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabelaValores", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrosEstacionamentos");

            migrationBuilder.DropTable(
                name: "TabelaValores");
        }
    }
}
