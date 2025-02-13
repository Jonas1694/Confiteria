using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArquitecturaModel.Migrations
{
    /// <inheritdoc />
    public partial class Devolucion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devolucions",
                columns: table => new
                {
                    DevolucionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    NDocumento = table.Column<double>(type: "float", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalIva = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Iva = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusDocumentoId = table.Column<int>(type: "int", nullable: false),
                    DescripcionDevolucion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacturacionId = table.Column<int>(type: "int", nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devolucions", x => x.DevolucionId);
                    table.ForeignKey(
                        name: "FK_Devolucions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Devolucions_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Devolucions_Facturacion_FacturacionId",
                        column: x => x.FacturacionId,
                        principalTable: "Facturacion",
                        principalColumn: "FacturacionId");
                    table.ForeignKey(
                        name: "FK_Devolucions_StatusDocumentos_StatusDocumentoId",
                        column: x => x.StatusDocumentoId,
                        principalTable: "StatusDocumentos",
                        principalColumn: "StatusDocumentoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devolucions_ClienteId",
                table: "Devolucions",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Devolucions_FacturacionId",
                table: "Devolucions",
                column: "FacturacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Devolucions_StatusDocumentoId",
                table: "Devolucions",
                column: "StatusDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Devolucions_UserId",
                table: "Devolucions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devolucions");
        }
    }
}
