using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gestionTickets.Migrations
{
    /// <inheritdoc />
    public partial class RelacionesVirtuales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PuestoId1",
                table: "Desarrolladores",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Desarrolladores_PuestoId1",
                table: "Desarrolladores",
                column: "PuestoId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Desarrolladores_Puestos_PuestoId1",
                table: "Desarrolladores",
                column: "PuestoId1",
                principalTable: "Puestos",
                principalColumn: "PuestoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Desarrolladores_Puestos_PuestoId1",
                table: "Desarrolladores");

            migrationBuilder.DropIndex(
                name: "IX_Desarrolladores_PuestoId1",
                table: "Desarrolladores");

            migrationBuilder.DropColumn(
                name: "PuestoId1",
                table: "Desarrolladores");
        }
    }
}
