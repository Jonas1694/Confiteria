using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArquitecturaModel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProveedor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Proveedors",
                newName: "ProveedorId");

            migrationBuilder.AlterColumn<string>(
                name: "Rif",
                table: "Proveedors",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "TipoDocumento",
                table: "Proveedors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoDocumento",
                table: "Proveedors");

            migrationBuilder.RenameColumn(
                name: "ProveedorId",
                table: "Proveedors",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "Rif",
                table: "Proveedors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(9)",
                oldMaxLength: 9);
        }
    }
}
