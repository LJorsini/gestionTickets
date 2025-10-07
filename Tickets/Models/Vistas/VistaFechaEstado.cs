namespace gestionTickets.Models
{
    public class VistaFechaEstado
    {
        public string FechaCreacionString { get; set; }
        public List<Estados> Estados { get; set; }
    }

    public class Estados
    {
        public string EstadoString { get; set; }
        public List<VistaTicket> Tickets { get; set; }
    }
}