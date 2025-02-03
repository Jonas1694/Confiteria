using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArquitecturaModel.Migrations
{
    /// <inheritdoc />
    public partial class Marcas_Productos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MarcasId",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_MarcasId",
                table: "Productos",
                column: "MarcasId");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Marcas_MarcasId",
                table: "Productos",
                column: "MarcasId",
                principalTable: "Marcas",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Marcas_MarcasId",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_MarcasId",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "MarcasId",
                table: "Productos");
        }
    }
}
