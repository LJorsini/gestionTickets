using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace gestionTickets.Models
{
    public class Puesto
    {
        [Key]
        public int PuestoId { get; set; }
        public string NombrePuesto { get; set; }
        public bool Activo { get; set; }
        

        public virtual ICollection<Desarrollador>? Desarrolladores { get; set; }
    }

}