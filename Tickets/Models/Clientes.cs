using System.ComponentModel.DataAnnotations;

namespace gestionTickets.Models
{
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }
        public string? Nombre {get; set;}
        public string? Email {get; set;}
        public string? Telefono {get; set;}
        public string? Cuit { get; set; }
        public string? Observaciones { get; set; }
        public bool? Eliminado { get; set; } 
        
        
    }
}