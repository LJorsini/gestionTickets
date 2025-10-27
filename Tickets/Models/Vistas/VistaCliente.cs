using gestionTickets.Models;
using gestionTickets.Models.Vistas;

namespace gestionTickets.ModelsVistas
{
    public class VistaCliente
    {
        public int ClienteId { get; set; }
        public string? Nombre { get; set; }
        public string Email { get; set; }
        public string? Telefono { get; set; }
        public string? Cuit { get; set; }
        public string? Observaciones { get; set; }
        public bool? Eliminado { get; set; }
        public int TicketsTotales { get; set; }
        public int TicketsAbiertos { get; set; }
        public int TicketsCerrados { get; set; }
        public int TicketsPrioridadAlta { get; set; }
        public float PorcentajeCriticos { get; set; }
        public string FechaUltimoCreado { get; set; }
        public string FechaUltimoCerrado { get; set; }
        public string? UsuarioClienteID { get; set; }
        public List<VistaTicket> Tickets { get; set; }
        public List<VistaCategorias> Categorias { get; set; }
    }
}