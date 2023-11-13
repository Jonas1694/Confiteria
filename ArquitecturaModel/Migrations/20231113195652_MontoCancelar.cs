using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArquitecturaModel.Migrations
{
    /// <inheritdoc />
    public partial class MontoCancelar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormaPagoId",
                table: "Facturacion",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MontoCancelar",
                table: "Facturacion",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facturacion_FormaPagoId",
                table: "Facturacion",
                column: "FormaPagoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturacion_FormaPago_FormaPagoId",
                table: "Facturacion",
                column: "FormaPagoId",
                principalTable: "FormaPago",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturacion_FormaPago_FormaPagoId",
                table: "Facturacion");

            migrationBuilder.DropIndex(
                name: "IX_Facturacion_FormaPagoId",
                table: "Facturacion");

            migrationBuilder.DropColumn(
                name: "FormaPagoId",
                table: "Facturacion");

            migrationBuilder.DropColumn(
                name: "MontoCancelar",
                table: "Facturacion");
        }
    }
}
