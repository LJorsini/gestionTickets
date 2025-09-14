using gestionTickets.Models;

public class VistaTicket
    {
        public int TicketId { get; set; }
        public string Titulo { get; set; }
        public Prioridad Prioridad { get; set; } //crear enum pero como string
        public string EstadoString { get; set; } //crear enum pero como string
        public string FechaCreacionString { get; set; }
        public string PrioridadString { get; set; }
        public string? CategoriaString { get; set; }
        
        public string? UsuarioClienteID { get; set; }
        public string? NombreUsuario { get; set; } //Nombre del usuario que creó el ticket
        public string? EmailUsuario { get; set; } //Email del usuario que creó el ticket
        
    }