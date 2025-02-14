using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArquitecturaModel.Migrations
{
    /// <inheritdoc />
    public partial class ProductoId_Dev_Det : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleDevoluciones_Productos_ProductosId",
                table: "DetalleDevoluciones");

            migrationBuilder.DropIndex(
                name: "IX_DetalleDevoluciones_ProductosId",
                table: "DetalleDevoluciones");

            migrationBuilder.DropColumn(
                name: "ProductosId",
                table: "DetalleDevoluciones");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleDevoluciones_ProductoId",
                table: "DetalleDevoluciones",
                column: "ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleDevoluciones_Productos_ProductoId",
                table: "DetalleDevoluciones",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleDevoluciones_Productos_ProductoId",
                table: "DetalleDevoluciones");

            migrationBuilder.DropIndex(
                name: "IX_DetalleDevoluciones_ProductoId",
                table: "DetalleDevoluciones");

            migrationBuilder.AddColumn<int>(
                name: "ProductosId",
                table: "DetalleDevoluciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DetalleDevoluciones_ProductosId",
                table: "DetalleDevoluciones",
                column: "ProductosId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleDevoluciones_Productos_ProductosId",
                table: "DetalleDevoluciones",
                column: "ProductosId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
