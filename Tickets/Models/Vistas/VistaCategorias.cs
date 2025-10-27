using gestionTickets.Models;
using Humanizer;

namespace gestionTickets.Models.Vistas
{
    public class VistaCategorias
    {
        public int CategoriaId { get; set; }
        public string? Descripcion { get; set; }
        public int CantidadDeTickets { get; set; }
        public int CantidadTicketsAbiertos { get; set; }
        public int CantidadTicketProceso { get; set; }
        public int CantidadTicketsCerrados { get; set; }
        public int PorcentajeCriticos { get; set; }
        public string FechaUltimoCreadoString { get; set; }
        public string FechaUltimoFinalizado { get; set; }
        public List<VistaTicket> Tickets { get; set; }
    }

    public class VistaEstados
    {
        public int EstadoId { get; set; }
        public string EstadoString { get; set; }
        public List<VistaTicket> Tickets { get; set; }
    }
}