using gestionTickets.Models.Vistas;
using Humanizer;

namespace gestionTickets.Models
{

    public class VistaClienteTicket
    {
        public int ClienteId { get; set; }
        public string Nombre { get; set; }
        public string UsuarioClienteID { get; set; }
        public List<VistaCategorias> Categorias { get; set; }
        
    }
}
