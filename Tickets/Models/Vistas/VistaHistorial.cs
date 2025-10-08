namespace gestionTickets.Models
{

    public class VistaHistorial
    {
        public int HistorialId { get; set; }
        public int TicketId { get; set; }
        public string CamposModificados { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorNuevo { get; set; }
        public string FechaModificacionString { get; set; }
        public string? UsuarioClienteID { get; set; } // se agrego recien
        public string? NombreUsuario { get; set; } //Nombre del usuario que creó el ticket se agrego recien
        public string? EmailUsuario { get; set; } //Email del usuario que creó el ticket se agrego recien
    }
}