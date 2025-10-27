using gestionTickets.Models;
using Humanizer;

public class VistaTicket
    {
        public int TicketId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int Prioridad { get; set; } //crear enum pero como string
        public string PrioridadString { get; set; }

        public string EstadoString { get; set; } //crear enum pero como string
        public string FechaCreacionString { get; set; }
        public string FechaComienzoString { get; set; }
        public string FechaCierreString { get; set; }
        public string FechaUltimoCreadoString { get; set; }
        public string FechaUltimoFinalizado { get; set; }
        public int CategoriaId { get; set; }
        public string? CategoriaString { get; set; }
        public int TicketsTotales { get; set; }
        public int CantidadTicketProceso { get; set; }
        public int CantidadCerrados { get; set; }
        public int PorcentajeCritico { get; set; }
         
        
        
          
        public string? UsuarioClienteID { get; set; }
        public string? NombreUsuario { get; set; } //Nombre del usuario que creó el ticket
        public string? EmailUsuario { get; set; } //Email del usuario que creó el ticket
        
    }


/* Vista  */
