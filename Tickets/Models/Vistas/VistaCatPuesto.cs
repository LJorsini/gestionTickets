using Humanizer;

namespace gestionTickets.Models
{
    public class VistaCatPuesto
    {
        public int CategoriaId { get; set; }
        public string Descripcion { get; set; }
        public List<VistaPuesto> Puestos { get; set; } 
    }
}