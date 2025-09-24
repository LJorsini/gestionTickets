using gestionTickets.Models;

namespace gestionTickets.ModelsVistas
{
    public class VistaClientes
    {
        public int ClienteId { get; set; }
        public string? Nombre { get; set; }
        public string Email { get; set; }
        public string? Telefono { get; set; }
        public string? Cuit { get; set; }
        public string? Observaciones { get; set; }
        public bool? Eliminado { get; set; }
        public string? UsuarioClienteID { get; set; }
        public List<VistaTicket> Tickets { get; set; }
    }
}