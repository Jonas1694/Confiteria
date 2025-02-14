using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArquitecturaModel.Migrations
{
    /// <inheritdoc />
    public partial class UserId_Devolucion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleDevoluciones_Devolucions_DevolucionId",
                table: "DetalleDevoluciones");

            migrationBuilder.DropIndex(
                name: "IX_DetalleDevoluciones_DevolucionId",
                table: "DetalleDevoluciones");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Devolucions");

            migrationBuilder.DropColumn(
                name: "DevolucionId",
                table: "DetalleDevoluciones");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleDevoluciones_DocumentoId",
                table: "DetalleDevoluciones",
                column: "DocumentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleDevoluciones_Devolucions_DocumentoId",
                table: "DetalleDevoluciones",
                column: "DocumentoId",
                principalTable: "Devolucions",
                principalColumn: "DevolucionId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleDevoluciones_Devolucions_DocumentoId",
                table: "DetalleDevoluciones");

            migrationBuilder.DropIndex(
                name: "IX_DetalleDevoluciones_DocumentoId",
                table: "DetalleDevoluciones");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Devolucions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DevolucionId",
                table: "DetalleDevoluciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DetalleDevoluciones_DevolucionId",
                table: "DetalleDevoluciones",
                column: "DevolucionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleDevoluciones_Devolucions_DevolucionId",
                table: "DetalleDevoluciones",
                column: "DevolucionId",
                principalTable: "Devolucions",
                principalColumn: "DevolucionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
