using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArquitecturaModel.Migrations
{
    /// <inheritdoc />
    public partial class StatusDocumento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusDocumentoId",
                table: "Facturacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "StatusDocumentos",
                columns: table => new
                {
                    StatusDocumentoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusDocumento = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusDocumentos", x => x.StatusDocumentoId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facturacion_StatusDocumentoId",
                table: "Facturacion",
                column: "StatusDocumentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturacion_StatusDocumentos_StatusDocumentoId",
                table: "Facturacion",
                column: "StatusDocumentoId",
                principalTable: "StatusDocumentos",
                principalColumn: "StatusDocumentoId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturacion_StatusDocumentos_StatusDocumentoId",
                table: "Facturacion");

            migrationBuilder.DropTable(
                name: "StatusDocumentos");

            migrationBuilder.DropIndex(
                name: "IX_Facturacion_StatusDocumentoId",
                table: "Facturacion");

            migrationBuilder.DropColumn(
                name: "StatusDocumentoId",
                table: "Facturacion");
        }
    }
}
