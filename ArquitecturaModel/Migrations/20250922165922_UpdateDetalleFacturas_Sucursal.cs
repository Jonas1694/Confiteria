using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArquitecturaModel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDetalleFacturas_Sucursal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleFacturas_Sucursales_SucursalesSucursalId",
                table: "DetalleFacturas");

            migrationBuilder.DropColumn(
                name: "SucursalId",
                table: "DetalleFacturas");

            migrationBuilder.RenameColumn(
                name: "SucursalesSucursalId",
                table: "DetalleFacturas",
                newName: "SucursalesId");

            migrationBuilder.RenameIndex(
                name: "IX_DetalleFacturas_SucursalesSucursalId",
                table: "DetalleFacturas",
                newName: "IX_DetalleFacturas_SucursalesId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleFacturas_Sucursales_SucursalesId",
                table: "DetalleFacturas",
                column: "SucursalesId",
                principalTable: "Sucursales",
                principalColumn: "SucursalId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleFacturas_Sucursales_SucursalesId",
                table: "DetalleFacturas");

            migrationBuilder.RenameColumn(
                name: "SucursalesId",
                table: "DetalleFacturas",
                newName: "SucursalesSucursalId");

            migrationBuilder.RenameIndex(
                name: "IX_DetalleFacturas_SucursalesId",
                table: "DetalleFacturas",
                newName: "IX_DetalleFacturas_SucursalesSucursalId");

            migrationBuilder.AddColumn<int>(
                name: "SucursalId",
                table: "DetalleFacturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleFacturas_Sucursales_SucursalesSucursalId",
                table: "DetalleFacturas",
                column: "SucursalesSucursalId",
                principalTable: "Sucursales",
                principalColumn: "SucursalId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
