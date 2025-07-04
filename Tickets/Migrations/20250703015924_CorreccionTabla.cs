using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gestionTickets.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "PuestoId",
                table: "Desarrolladores",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Desarrolladores_PuestoId",
                table: "Desarrolladores",
                column: "PuestoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Desarrolladores_Puestos_PuestoId",
                table: "Desarrolladores",
                column: "PuestoId",
                principalTable: "Puestos",
                principalColumn: "PuestoId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Desarrolladores_Puestos_PuestoId",
                table: "Desarrolladores");

            migrationBuilder.DropIndex(
                name: "IX_Desarrolladores_PuestoId",
                table: "Desarrolladores");

            migrationBuilder.AlterColumn<string>(
                name: "PuestoId",
                table: "Desarrolladores",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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
    }
}
