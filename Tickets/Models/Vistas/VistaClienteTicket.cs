using gestionTickets.Models.Vistas;
using Humanizer;

namespace gestionTickets.Models
{

    public class VistaClienteTicket
    {
        public int ClienteId { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string UsuarioClienteID { get; set; }
        public List<VistaCategorias> Categorias { get; set; }
        

    }

    public class VistaDesarrolladorTicket
    {
        public int DesarrolladorId { get; set; }
        public string NombreCompleto { get; set; }
        public string? UsuarioClienteID { get; set; }
        public List<VistaPuesto> Puestos { get; set; }
        
    }
}
