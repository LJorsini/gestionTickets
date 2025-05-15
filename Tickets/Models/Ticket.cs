using System.ComponentModel.DataAnnotations;

namespace gestionTickets.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public Estado Estado { get; set; } //crear enum
        public Prioridad Prioridad { get; set; } //crear enum
        public DateOnly FechaCreacion { get; set; }
        public DateOnly? FechaCierre { get; set; }
        //public int UsuarioClienteID {get; set;}
        public int CategoriaId { get; set; }
        //public virtual ICollection<ComentarioTicket> Comentarios { get; set; }
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
        public string Descripcion { get; set; }
        public Estado Estado { get; set; } //crear enum
        public Prioridad Prioridad { get; set; } //crear enum
        public string FechaCreacion { get; set; }
        public string FechaCierre { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; }
    }
}