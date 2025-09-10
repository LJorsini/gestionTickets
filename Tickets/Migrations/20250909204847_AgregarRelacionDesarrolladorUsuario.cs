using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gestionTickets.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRelacionDesarrolladorUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioClienteID",
                table: "Desarrolladores",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsuarioClienteID",
                table: "Desarrolladores");
        }
    }
}
