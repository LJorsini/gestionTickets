namespace gestionTickets.Models
{
    public class VistaFechaPrioridad
    {
        public string FechaCreacionString { get; set; }
        public List<Prioridades> PrioridadString { get; set; }
    }

    public class Prioridades
    {
        public string? PrioridadString { get; set; }
        public List<VistaTicket> Tickets { get; set; }
    }
}



