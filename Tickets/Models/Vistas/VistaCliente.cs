using gestionTickets.Models;

namespace gestionTickets.ModelsVistas
{
    public class VistaClientes
    {
        public int ClienteId { get; set; }
        public string? Nombre { get; set; }
        public string Email { get; set; }
        public List<VistaTicket> Tickets { get; set; }

    }
}