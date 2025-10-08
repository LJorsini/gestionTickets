using gestionTickets.Models;

namespace gestionTickets.Models.Vistas
{
    public class VistaCategorias
    {
        public int CategoriaId { get; set; }
        public string? Descripcion { get; set; }

        public int CantidadTicketsAbiertos { get; set; }
        public int CantidadTicketProceso { get; set; }
        public int CantidadTicketsCerrados { get; set; }
        public List<VistaTicket> Tickets { get; set; }
    }
}