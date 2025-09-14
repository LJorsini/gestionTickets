namespace gestionTickets.Models
{
    public class VistaPuesto
    {
        public int PuestoId { get; set; }
        public string NombrePuesto { get; set; }
        public List<VistaDesarrollador> Desarrollador {get; set;}
    }
}