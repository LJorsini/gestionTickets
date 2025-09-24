using System.ComponentModel.DataAnnotations;
using gestionTickets.Models;

namespace gestionTickets
{
    public class PuestoCategoria
    {
        [Key]
        public int PuestoCategoriaId { get; set; }
        public int PuestoId { get; set; }
        public int CategoriaId { get; set; }
        public virtual Puesto? Puesto { get; set; }
        public virtual Categoria? Categoria { get; set; }

    }

   
}