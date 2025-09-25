using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gestionTickets.Models
{
    public class Desarrollador
    {
        [Key]
        public int DesarrolladorId { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string DNI { get; set; }
        public string Telefono { get; set; }
        public int PuestoId { get; set; }
        public string Observacion { get; set; }
        public string? UsuarioClienteID { get; set; } 
        public virtual Puesto? Puesto { get; set; }

        //public virtual ICollection<Ticket>? Tickets { get; set; } 
        //public virtual ApplicationUser? UsuarioCliente { get; set; } 

    }

 

    

}