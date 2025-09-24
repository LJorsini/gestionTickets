using Humanizer;

namespace gestionTickets.Models
{
    public class VistaPuestoCategoria
    {
        public int PuestoCategoriaId { get; set; }
        public int PuestoId { get; set; }
        public int CategoriaId { get; set; }
        public string NombrePuesto { get; set; }
        public string NombreCategoria { get; set; }
        public List<VistaPuesto> Puestos { get; set; }

        
    }
}