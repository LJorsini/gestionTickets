using System.ComponentModel.DataAnnotations.Schema;
using gestionTickets.Models;


public class Historial
{
    public int HistorialId { get; set; }
    public int TicketId { get; set; }
    public string CamposModificados { get; set; }
    public string ValorAnterior { get; set; }
    public string ValorNuevo { get; set; }
    public string? UsuarioClienteID { get; set; } //se agrego recinen
    public virtual ApplicationUser? UsuarioCliente { get; set; } //se agrego recinen

    public DateTime FechaModificacion { get; set; }

    [NotMapped]
    public string FechaModificacionString { get { return FechaModificacion.ToString("dd/MM/yyyy HH:mm"); } }

    /* public virtual Ticket Ticket { get; set; } */

}

