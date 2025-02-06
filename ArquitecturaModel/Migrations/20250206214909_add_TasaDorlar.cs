using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArquitecturaModel.Migrations
{
    /// <inheritdoc />
    public partial class add_TasaDorlar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TasaDolars",
                table: "TasaDolars");

            migrationBuilder.RenameTable(
                name: "TasaDolars",
                newName: "TasaDolar");

            migrationBuilder.AddColumn<int>(
                name: "TasaDolarId",
                table: "Facturacion",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TasaDolar",
                table: "TasaDolar",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Facturacion_TasaDolarId",
                table: "Facturacion",
                column: "TasaDolarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturacion_TasaDolar_TasaDolarId",
                table: "Facturacion",
                column: "TasaDolarId",
                principalTable: "TasaDolar",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturacion_TasaDolar_TasaDolarId",
                table: "Facturacion");

            migrationBuilder.DropIndex(
                name: "IX_Facturacion_TasaDolarId",
                table: "Facturacion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TasaDolar",
                table: "TasaDolar");

            migrationBuilder.DropColumn(
                name: "TasaDolarId",
                table: "Facturacion");

            migrationBuilder.RenameTable(
                name: "TasaDolar",
                newName: "TasaDolars");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TasaDolars",
                table: "TasaDolars",
                column: "Id");
        }
    }
}
