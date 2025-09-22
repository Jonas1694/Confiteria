using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArquitecturaModel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDetalleDevolucion_Sucursal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SucursalId",
                table: "DetalleDevoluciones",
                newName: "SucursalesId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleDevoluciones_SucursalesId",
                table: "DetalleDevoluciones",
                column: "SucursalesId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleDevoluciones_Sucursales_SucursalesId",
                table: "DetalleDevoluciones",
                column: "SucursalesId",
                principalTable: "Sucursales",
                principalColumn: "SucursalId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleDevoluciones_Sucursales_SucursalesId",
                table: "DetalleDevoluciones");

            migrationBuilder.DropIndex(
                name: "IX_DetalleDevoluciones_SucursalesId",
                table: "DetalleDevoluciones");

            migrationBuilder.RenameColumn(
                name: "SucursalesId",
                table: "DetalleDevoluciones",
                newName: "SucursalId");
        }
    }
}
