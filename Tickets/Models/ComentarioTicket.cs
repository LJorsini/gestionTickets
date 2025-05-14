/* using System.ComponentModel.DataAnnotations;

namespace gestionTickets.Models
{
    public class ComentarioTicket
    {
        [Key]
        public int ComentarioId { get; set; }
        public int TicketId { get; set; }
        //public int UsuarioId { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaCreacion { get; set; } 
        public virtual Ticket Ticket { get; set; }
    }
} */