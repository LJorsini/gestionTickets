using gestionTickets.Models;


public class Historial
{
    public int HistorialId { get; set; }
    public int TicketId { get; set; }
    public string CamposModificados { get; set; }
    public string ValorAnterior { get; set; }
    public string ValorNuevo { get; set; }

    public DateTime FechaModificacion { get; set; }

    public virtual Ticket Tickets { get; set; }

}

public class VistaHistorial
{
    public int HistorialId { get; set; }
    public int TicketId { get; set; }
    public string CamposModificados { get; set; }
    public string ValorAnterior { get; set; }
    public string ValorNuevo { get; set; }
    public DateTime FechaModificacion { get; set; }
}