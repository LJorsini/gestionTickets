using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gestionTickets.Migrations
{
    /// <inheritdoc />
    public partial class RelacionesTablaPuestoCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PuestoCategorias_CategoriaId",
                table: "PuestoCategorias",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_PuestoCategorias_PuestoId",
                table: "PuestoCategorias",
                column: "PuestoId");

            migrationBuilder.AddForeignKey(
                name: "FK_PuestoCategorias_Categorias_CategoriaId",
                table: "PuestoCategorias",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "CategoriaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PuestoCategorias_Puestos_PuestoId",
                table: "PuestoCategorias",
                column: "PuestoId",
                principalTable: "Puestos",
                principalColumn: "PuestoId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PuestoCategorias_Categorias_CategoriaId",
                table: "PuestoCategorias");

            migrationBuilder.DropForeignKey(
                name: "FK_PuestoCategorias_Puestos_PuestoId",
                table: "PuestoCategorias");

            migrationBuilder.DropIndex(
                name: "IX_PuestoCategorias_CategoriaId",
                table: "PuestoCategorias");

            migrationBuilder.DropIndex(
                name: "IX_PuestoCategorias_PuestoId",
                table: "PuestoCategorias");
        }
    }
}
