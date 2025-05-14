using System.ComponentModel.DataAnnotations;

namespace gestionTickets.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public string Titulo {get;set;}
        public string Descripcion {get; set;}
        public  Estado Estado {get; set;} //crear enum
        public Prioridad Prioridad {get; set;} //crear enum
        public DateOnly FechaCreacion {get; set;}
        public DateOnly FechaCierre {get; set;}
        //public int UsuarioClienteID {get; set;}
        public int CategoriaId {get; set;}
        //public virtual ICollection<ComentarioTicket> Comentarios { get; set; }
        public virtual Categoria Categoria { get; set; }
        
    }

    public enum Estado
    {
        Abierto ,
        EnProceso,
        Cerrado,
        Cancelado,
    }

    public enum Prioridad
    {
        Baja ,
        Media,
        Alta,
    }
}