using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gestionTickets.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public Estado Estado { get; set; } 

        [NotMapped]
        public string EstadoString { get { return Estado.ToString(); } }
        public Prioridad Prioridad { get; set; } 

        [NotMapped]
        public string PrioridadString { get { return Prioridad.ToString(); } }
        public DateTime FechaCreacion { get; set; }

        [NotMapped]
        public string FechaCreacionString { get { return FechaCreacion.ToString("dd/MM/yyyy"); } }

        [NotMapped]
        public string? CategoriaString { get { return Categoria?.Descripcion; } }
        public DateTime? FechaCierre { get; set; }
         public int CategoriaId { get; set; }
        
        public string? UsuarioClienteID { get; set; }
        public virtual Categoria? Categoria { get; set; }

    }

    public enum Estado
    {
        Abierto = 1,
        EnProceso,
        Cerrado,
        Cancelado,
    }

    public enum Prioridad
    {
        Baja = 1,
        Media,
        Alta,
    }

    public class VistaTicket
    {
        public int TicketId { get; set; }
        public string Titulo { get; set; }
        public Prioridad Prioridad { get; set; } //crear enum pero como string
        public string EstadoString { get; set; } //crear enum pero como string
        public string FechaCreacionString { get; set; }
        public string PrioridadString { get; set; }
        public string? CategoriaString { get; set; }
        
        public string? UsuarioClienteID { get; set; }
        public string? NombreUsuario { get; set; } //Nombre del usuario que creó el ticket
        public string? EmailUsuario { get; set; } //Email del usuario que creó el ticket
        
    }

    public class FiltroTickets
    {
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
        public int CategoriaId { get; set; }
        public int Prioridad { get; set; }
        public int Estado { get; set; } // 1: Abierto, 2: EnProceso, 3: Cerrado, 4: Cancelado
        
    }
}