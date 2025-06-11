using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gestionTickets.Migrations
{
    /// <inheritdoc />
    public partial class CampoUsuarioId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioClienteID",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsuarioClienteID",
                table: "Tickets");
        }
    }
}
