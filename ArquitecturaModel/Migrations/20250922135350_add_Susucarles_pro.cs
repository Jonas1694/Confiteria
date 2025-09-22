using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArquitecturaModel.Migrations
{
    /// <inheritdoc />
    public partial class add_Susucarles_pro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Productos");

            migrationBuilder.AddColumn<int>(
                name: "SucursalId",
                table: "Facturacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SucursalesSucursalId",
                table: "Facturacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SucursalId",
                table: "DetalleFacturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SucursalesSucursalId",
                table: "DetalleFacturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SucursalId",
                table: "DetalleDevoluciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SucursalId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SucursalesSucursalId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Inventario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    ProductosId = table.Column<int>(type: "int", nullable: false),
                    SucursalId = table.Column<int>(type: "int", nullable: false),
                    SucursalesSucursalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventario_Productos_ProductosId",
                        column: x => x.ProductosId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Inventario_Sucursales_SucursalesSucursalId",
                        column: x => x.SucursalesSucursalId,
                        principalTable: "Sucursales",
                        principalColumn: "SucursalId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facturacion_SucursalesSucursalId",
                table: "Facturacion",
                column: "SucursalesSucursalId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleFacturas_SucursalesSucursalId",
                table: "DetalleFacturas",
                column: "SucursalesSucursalId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SucursalesSucursalId",
                table: "AspNetUsers",
                column: "SucursalesSucursalId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_ProductosId",
                table: "Inventario",
                column: "ProductosId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_SucursalesSucursalId",
                table: "Inventario",
                column: "SucursalesSucursalId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Sucursales_SucursalesSucursalId",
                table: "AspNetUsers",
                column: "SucursalesSucursalId",
                principalTable: "Sucursales",
                principalColumn: "SucursalId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleFacturas_Sucursales_SucursalesSucursalId",
                table: "DetalleFacturas",
                column: "SucursalesSucursalId",
                principalTable: "Sucursales",
                principalColumn: "SucursalId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Facturacion_Sucursales_SucursalesSucursalId",
                table: "Facturacion",
                column: "SucursalesSucursalId",
                principalTable: "Sucursales",
                principalColumn: "SucursalId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Sucursales_SucursalesSucursalId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_DetalleFacturas_Sucursales_SucursalesSucursalId",
                table: "DetalleFacturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Facturacion_Sucursales_SucursalesSucursalId",
                table: "Facturacion");

            migrationBuilder.DropTable(
                name: "Inventario");

            migrationBuilder.DropIndex(
                name: "IX_Facturacion_SucursalesSucursalId",
                table: "Facturacion");

            migrationBuilder.DropIndex(
                name: "IX_DetalleFacturas_SucursalesSucursalId",
                table: "DetalleFacturas");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SucursalesSucursalId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SucursalId",
                table: "Facturacion");

            migrationBuilder.DropColumn(
                name: "SucursalesSucursalId",
                table: "Facturacion");

            migrationBuilder.DropColumn(
                name: "SucursalId",
                table: "DetalleFacturas");

            migrationBuilder.DropColumn(
                name: "SucursalesSucursalId",
                table: "DetalleFacturas");

            migrationBuilder.DropColumn(
                name: "SucursalId",
                table: "DetalleDevoluciones");

            migrationBuilder.DropColumn(
                name: "SucursalId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SucursalesSucursalId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
