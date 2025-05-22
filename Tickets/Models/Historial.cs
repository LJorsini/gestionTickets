using System.ComponentModel.DataAnnotations.Schema;
using gestionTickets.Models;


public class Historial
{
    public int HistorialId { get; set; }
    public int TicketId { get; set; }
    public string CamposModificados { get; set; }
    public string ValorAnterior { get; set; }
    public string ValorNuevo { get; set; }

    public DateTime FechaModificacion { get; set; }

    [NotMapped]
    public string FechaModificacionString { get { return FechaModificacion.ToString("dd/MM/yyyy HH:mm"); } }

    /* public virtual Ticket Ticket { get; set; } */

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