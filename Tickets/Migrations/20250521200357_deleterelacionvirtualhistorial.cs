using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gestionTickets.Migrations
{
    /// <inheritdoc />
    public partial class deleterelacionvirtualhistorial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Historial_Tickets_TicketId",
                table: "Historial");

            migrationBuilder.DropIndex(
                name: "IX_Historial_TicketId",
                table: "Historial");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Historial_TicketId",
                table: "Historial",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Historial_Tickets_TicketId",
                table: "Historial",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
