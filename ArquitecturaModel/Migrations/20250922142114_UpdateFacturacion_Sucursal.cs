using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArquitecturaModel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFacturacion_Sucursal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Sucursales_SucursalesSucursalId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Facturacion_Sucursales_SucursalesSucursalId",
                table: "Facturacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventario_Sucursales_SucursalesSucursalId",
                table: "Inventario");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SucursalesSucursalId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SucursalId",
                table: "Inventario");

            migrationBuilder.DropColumn(
                name: "SucursalId",
                table: "Facturacion");

            migrationBuilder.DropColumn(
                name: "SucursalesSucursalId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "SucursalesSucursalId",
                table: "Inventario",
                newName: "SucursalesId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventario_SucursalesSucursalId",
                table: "Inventario",
                newName: "IX_Inventario_SucursalesId");

            migrationBuilder.RenameColumn(
                name: "SucursalesSucursalId",
                table: "Facturacion",
                newName: "SucursalesId");

            migrationBuilder.RenameIndex(
                name: "IX_Facturacion_SucursalesSucursalId",
                table: "Facturacion",
                newName: "IX_Facturacion_SucursalesId");

            migrationBuilder.RenameColumn(
                name: "SucursalId",
                table: "AspNetUsers",
                newName: "SucursalesId");

            migrationBuilder.AddColumn<int>(
                name: "SucursalesId",
                table: "Devolucions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Devolucions_SucursalesId",
                table: "Devolucions",
                column: "SucursalesId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SucursalesId",
                table: "AspNetUsers",
                column: "SucursalesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Sucursales_SucursalesId",
                table: "AspNetUsers",
                column: "SucursalesId",
                principalTable: "Sucursales",
                principalColumn: "SucursalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devolucions_Sucursales_SucursalesId",
                table: "Devolucions",
                column: "SucursalesId",
                principalTable: "Sucursales",
                principalColumn: "SucursalId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Facturacion_Sucursales_SucursalesId",
                table: "Facturacion",
                column: "SucursalesId",
                principalTable: "Sucursales",
                principalColumn: "SucursalId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventario_Sucursales_SucursalesId",
                table: "Inventario",
                column: "SucursalesId",
                principalTable: "Sucursales",
                principalColumn: "SucursalId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Sucursales_SucursalesId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Devolucions_Sucursales_SucursalesId",
                table: "Devolucions");

            migrationBuilder.DropForeignKey(
                name: "FK_Facturacion_Sucursales_SucursalesId",
                table: "Facturacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventario_Sucursales_SucursalesId",
                table: "Inventario");

            migrationBuilder.DropIndex(
                name: "IX_Devolucions_SucursalesId",
                table: "Devolucions");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SucursalesId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SucursalesId",
                table: "Devolucions");

            migrationBuilder.RenameColumn(
                name: "SucursalesId",
                table: "Inventario",
                newName: "SucursalesSucursalId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventario_SucursalesId",
                table: "Inventario",
                newName: "IX_Inventario_SucursalesSucursalId");

            migrationBuilder.RenameColumn(
                name: "SucursalesId",
                table: "Facturacion",
                newName: "SucursalesSucursalId");

            migrationBuilder.RenameIndex(
                name: "IX_Facturacion_SucursalesId",
                table: "Facturacion",
                newName: "IX_Facturacion_SucursalesSucursalId");

            migrationBuilder.RenameColumn(
                name: "SucursalesId",
                table: "AspNetUsers",
                newName: "SucursalId");

            migrationBuilder.AddColumn<int>(
                name: "SucursalId",
                table: "Inventario",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SucursalId",
                table: "Facturacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SucursalesSucursalId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SucursalesSucursalId",
                table: "AspNetUsers",
                column: "SucursalesSucursalId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Sucursales_SucursalesSucursalId",
                table: "AspNetUsers",
                column: "SucursalesSucursalId",
                principalTable: "Sucursales",
                principalColumn: "SucursalId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Facturacion_Sucursales_SucursalesSucursalId",
                table: "Facturacion",
                column: "SucursalesSucursalId",
                principalTable: "Sucursales",
                principalColumn: "SucursalId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventario_Sucursales_SucursalesSucursalId",
                table: "Inventario",
                column: "SucursalesSucursalId",
                principalTable: "Sucursales",
                principalColumn: "SucursalId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
