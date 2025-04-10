using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estacionamento.Migrations
{
    /// <inheritdoc />
    public partial class edit_type_value : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ValorPagar",
                table: "RegistrosEstacionamentos",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ValorPagar",
                table: "RegistrosEstacionamentos",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
