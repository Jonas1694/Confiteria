using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArquitecturaModel.Migrations
{
    /// <inheritdoc />
    public partial class updateProductos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "idProveedor",
                table: "Productos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "idProveedor",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
