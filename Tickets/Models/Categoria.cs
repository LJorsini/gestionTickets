using System.ComponentModel.DataAnnotations;

namespace gestionTickets.Models
{
    public class Categoria
    {
        [Key]
        public int CategoriaId { get; set; }
        public string Descripcion {get; set;}
        public bool Eliminado {get; set;}

        public virtual ICollection<Ticket> Tickets { get; set; }
        
    }
}