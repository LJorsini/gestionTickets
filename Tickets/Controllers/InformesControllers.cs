using Ezpeleta2025.Models.Usuario;
using gestionTickets.Models;
using gestionTickets.Models.Vistas;
using gestionTickets.ModelsVistas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Compression;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace gestionTickets.Controllers
{
    [Authorize]
    [Route("api/auth/[controller]")]
    [ApiController]

    public class InformesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public InformesController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("ticketsFechaPrioridad")]

        public async Task<ActionResult<IEnumerable<VistaFechaPrioridad>>> GetTicketFechaPrioridad()
        {
            List<VistaFechaPrioridad> vistaFechaPrioridad = new List<VistaFechaPrioridad>();

            var tickets = await _context.Tickets
                          .Include(t => t.Categoria)
                          .ToListAsync();
            foreach (var ticket in tickets)
            {
                var mostrarVistaFecha = vistaFechaPrioridad.FirstOrDefault(t => t.FechaCreacionString == ticket.FechaCreacionString);

                if (mostrarVistaFecha == null)
                {
                    mostrarVistaFecha = new VistaFechaPrioridad
                    {
                        FechaCreacionString = ticket.FechaCreacionString,
                        PrioridadString = new List<Prioridades>(),
                    };
                    vistaFechaPrioridad.Add(mostrarVistaFecha);
                }

                var mostrarPrioridad = mostrarVistaFecha.PrioridadString.FirstOrDefault(t => t.PrioridadString == ticket.PrioridadString);

                if (mostrarPrioridad == null)
                {
                    mostrarPrioridad = new Prioridades
                    {
                        PrioridadString = ticket.PrioridadString,
                        Tickets = new List<VistaTicket>(),
                    };
                    mostrarVistaFecha.PrioridadString.Add(mostrarPrioridad);
                }

                var mostrarTicket = new VistaTicket
                {
                    Titulo = ticket.Titulo,
                    Descripcion = ticket.Descripcion
                };
                mostrarPrioridad.Tickets.Add(mostrarTicket);
            }
            return vistaFechaPrioridad;
        }

        [HttpGet("ticketsFechaEstado")]
        public async Task<ActionResult<IEnumerable<VistaFechaEstado>>> GetTicketFechaEstado()
        {

            List<VistaFechaEstado> vistaFechaEstado = new List<VistaFechaEstado>();

            var tickets = await _context.Tickets
                          .Include(t => t.Categoria)
                          .ToListAsync();


            foreach (var ticket in tickets)
            {
                var mostrarVistaFecha = vistaFechaEstado.FirstOrDefault(t => t.FechaCreacionString == ticket.FechaCreacionString);

                if (mostrarVistaFecha == null)
                {
                    mostrarVistaFecha = new VistaFechaEstado
                    {
                        FechaCreacionString = ticket.FechaCreacionString,
                        Estados = new List<Estados>(),
                    };
                    vistaFechaEstado.Add(mostrarVistaFecha);
                }

                var mostrarEstados = mostrarVistaFecha.Estados.FirstOrDefault(t => t.EstadoString == ticket.EstadoString);

                if (mostrarEstados == null)
                {
                    mostrarEstados = new Estados
                    {
                        EstadoString = ticket.EstadoString,
                        Tickets = new List<VistaTicket>(),
                    };
                    mostrarVistaFecha.Estados.Add(mostrarEstados);
                }

                var vistaTickets = new VistaTicket
                {
                    Titulo = ticket.Titulo,
                    Descripcion = ticket.Descripcion
                };
                mostrarEstados.Tickets.Add(vistaTickets);


            }
            return vistaFechaEstado;
        }

        [HttpGet("ticketsCantidad")]
        public async Task<ActionResult<IEnumerable<VistaClienteTicket>>> GetTicketCantidad()
        {

            List<VistaClienteTicket> vistaCantidad = new List<VistaClienteTicket>();

            var tickets = await _context.Tickets.Include(t => t.Categoria).ToListAsync();

            foreach (var ticket in tickets)
            {
                //Busco El usurioClienteId del ticket
                var clienteUser = _context.Users.Where(u => u.Id == ticket.UsuarioClienteID).FirstOrDefault();
                

                if (clienteUser == null)
                    continue;

                var cliente = _context.Clientes.Where(c => c.Email == clienteUser.Email).FirstOrDefault();

                if (cliente == null)
                    continue;

                var mostrarCliente = vistaCantidad.Where(c => c.Email == cliente.Email).FirstOrDefault();
                if (mostrarCliente == null)
                {
                    mostrarCliente = new VistaClienteTicket
                    {
                        ClienteId = cliente.ClienteId,
                        Nombre = cliente.Nombre,
                        Email = cliente.Email,
                        Categorias = new List<VistaCategorias>(),
                    };
                    vistaCantidad.Add(mostrarCliente);

                }

                var mostrarCategoria = mostrarCliente.Categorias.Where(c => c.CategoriaId == ticket.CategoriaId).FirstOrDefault();

                
                
                if (mostrarCategoria == null)
                {
                    mostrarCategoria = new VistaCategorias
                    {
                        CategoriaId = ticket.CategoriaId,
                        Descripcion = ticket.CategoriaString,
                        FechaUltimoCreadoString = ticket.FechaCreacionString,
                        FechaUltimoFinalizado = ticket.FechaCierreString,
                        CantidadTicketsAbiertos = tickets.Where(t => t.UsuarioClienteID == ticket.UsuarioClienteID && t.Estado == Estado.Abierto).Count(),
                        CantidadTicketProceso = tickets.Where(t => t.UsuarioClienteID == ticket.UsuarioClienteID && t.Estado == Estado.EnProceso).Count(),
                        CantidadTicketsCerrados = tickets.Where(t => t.UsuarioClienteID == ticket.UsuarioClienteID && t.Estado == Estado.Cerrado).Count(),
                        CantidadDeTickets = tickets.Where(t => t.UsuarioClienteID == ticket.UsuarioClienteID).Count(),
                        PorcentajeCriticos = tickets.Where(t =>  t.UsuarioClienteID == ticket.UsuarioClienteID && t.Prioridad == Prioridad.Alta).Count() * 100 / 2


                    };
                    mostrarCliente.Categorias.Add(mostrarCategoria);
                }




            }
            return vistaCantidad;
        }

        [HttpGet("ticketsDesarrollador")]
        public async Task<ActionResult<IEnumerable<VistaDesarrollador>>> GetTicketDesarrollador()
        {
            List<VistaDesarrollador> vistaDesarrollador = new List<VistaDesarrollador>();

            var tickets = _context.Tickets
                          .Include(t => t.Categoria)
                          .ToList();

            foreach (var ticket in tickets)
            {
                var puestosRelacionados = _context.PuestoCategorias
                                         .Where(pc => pc.CategoriaId == ticket.Categoria.CategoriaId)
                                         .Select(pc => pc.PuestoId)
                                         .ToList();

                var desarrolladoresPuesto = _context.Desarrolladores
                                            .Where(d => puestosRelacionados.Contains(d.PuestoId))
                                            .ToList();

                foreach (var desarrolladorPuesto in desarrolladoresPuesto)
                {
                    var mostrarDesarrollador = vistaDesarrollador.FirstOrDefault(d => d.DesarrolladorId == desarrolladorPuesto.DesarrolladorId);

                    if (mostrarDesarrollador == null)
                    {
                        mostrarDesarrollador = new VistaDesarrollador
                        {
                            DesarrolladorId = desarrolladorPuesto.DesarrolladorId,
                            NombreCompleto = desarrolladorPuesto.NombreCompleto,
                            Categorias = new List<VistaCategorias>()
                        };

                        vistaDesarrollador.Add(mostrarDesarrollador);
                    }
                }


            }

            return vistaDesarrollador;

        }

        [HttpGet("informeEstadistico")]

        public async Task<ActionResult<IEnumerable<VistaCliente>>> GetTicketsClientes()
        {
            List<VistaCliente> vistaClientes = new List<VistaCliente>();

           

            var tickets = _context.Tickets
                          .Include(t => t.Categoria)
                          .ToList();

            

            foreach (var ticket in tickets)
            {

                var clineteUser = _context.Users
                                  .Where(c => c.Id == ticket.UsuarioClienteID)
                                  .FirstOrDefault();
                                  
                if (clineteUser == null)
                    continue;

                var cliente = _context.Clientes
                              .Where(c => c.UsuarioClienteID == clineteUser.Id)
                              .FirstOrDefault();

                if (cliente == null)
                    continue;

                var clienteMostrar = vistaClientes.Where(c => c.ClienteId == cliente.ClienteId).FirstOrDefault();

                var mostrarTotales = tickets.Where(t => t.UsuarioClienteID == cliente.UsuarioClienteID && t.Estado != Estado.Cancelado).Count();
                var mostrarPrioridadAlta = tickets
                                           .Where(t => t.UsuarioClienteID == cliente.UsuarioClienteID
                                           && t.Prioridad == Prioridad.Alta
                                           && (t.Estado != Estado.Cancelado && t.Estado != Estado.Cerrado)).Count();

                if (clienteMostrar == null)
                {
                    
                    clienteMostrar = new VistaCliente
                    {
                        ClienteId = cliente.ClienteId,
                        Nombre = cliente.Nombre,
                        TicketsTotales = mostrarTotales,
                        TicketsAbiertos = tickets.Where(t => t.UsuarioClienteID == cliente.UsuarioClienteID && (t.Estado == Estado.Abierto || t.Estado == Estado.EnProceso)).Count(),
                        TicketsCerrados = tickets.Where(t => t.UsuarioClienteID == cliente.UsuarioClienteID && t.Estado == Estado.Cerrado).Count(),
                        /* TicketsPrioridadAlta = mostrarPrioridadAlta, */
                        PorcentajeCriticos = mostrarTotales == 0 ? 0 : (mostrarPrioridadAlta * 100 / mostrarTotales),
                        FechaUltimoCreado = tickets.Where(t => t.UsuarioClienteID == cliente.UsuarioClienteID).Max(t => t.FechaCreacionString),
                        FechaUltimoCerrado = tickets.Where(t => t.UsuarioClienteID == cliente.UsuarioClienteID).Max(t => t.FechaCierreString),
                    };
                    vistaClientes.Add(clienteMostrar);

                }
                        
            }
            return vistaClientes;
        }
    }

}


/* [HttpPost("cantidadticketsporClientes")] //Informe de 2 niveles por clientes
        public async Task<ActionResult<IEnumerable<ClienteTicket>>> InformeCantidadTicketPorClientes([FromBody] FiltroTicket filtro)
        {
            List<ClienteTicket> clientesMostrar = new List<ClienteTicket>();

            var tickets = _context.Ticket.Include(t => t.Categoria).AsQueryable();

            //VER DE ACUERDO AL ROL QUE TIENE SI DEBE FILTRAR POR USUARIO O NO
            //var usuarioLogueadoID = HttpContext.User.Identity.Name;
            // var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // var rol = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            // if (rol == "CLIENTE")
            // {
            //     tickets = tickets.Where(t => t.UsuarioClienteID == userId);
            // }

            DateTime fechaDesde = new DateTime();
            bool fechaDesdeValida = DateTime.TryParse(filtro.FechaDesde, out fechaDesde);

            DateTime fechaHasta = new DateTime();
            bool fechaHastaValida = DateTime.TryParse(filtro.FechaHasta, out fechaHasta);

            if (fechaDesdeValida && fechaHastaValida)
            {
                fechaHasta = fechaHasta.AddHours(23);
                fechaHasta = fechaHasta.AddMinutes(59);
                fechaHasta = fechaHasta.AddSeconds(59);
                tickets = tickets.Where(t => t.FechaCreacion >= fechaDesde && t.FechaCreacion <= fechaHasta);
            }

            foreach (var ticket in tickets)
            {
                //buscar el usuarioclienteID del Ticket
                var clienteUser = await _context.Users.Where(u => u.Id == ticket.UsuarioClienteID).FirstOrDefaultAsync();
                //comparamos el email guardado en user con el email de Cliente
                var cliente = await _context.Cliente.Where(c => c.Email == clienteUser.Email).FirstOrDefaultAsync();
                //ver si el cliente ya esta cargado en el listado
                var clienteMostrar = clientesMostrar.Where(c => c.Email == cliente.Email).FirstOrDefault();
                if (clienteMostrar == null)
                {
                    clienteMostrar = new ClienteTicket
                    {
                        ClienteID = cliente.ClienteID,
                        Nombre = cliente.Nombre,
                        Categorias = new List<CategoriaTickets>()
                    };
                    clientesMostrar.Add(clienteMostrar);
                }

                var categoriaMostrar = clienteMostrar.Categorias.Where(x => x.CategoriaID == ticket.CategoriaID).FirstOrDefault();
                if (categoriaMostrar == null)
                {
                    categoriaMostrar = new CategoriaTickets
                    {
                        CategoriaID = ticket.CategoriaID,
                        Nombre = ticket.CategoriaString,
                        CantidadAbiertos = tickets.Where(t => t.CategoriaID == ticket.CategoriaID && t.UsuarioClienteID == clienteUser.Id && t.Estados == Estado.Abierto).Count(),
                        CantidadCerrados = tickets.Where(t => t.CategoriaID == ticket.CategoriaID && t.UsuarioClienteID == clienteUser.Id && t.Estados == Estado.Cerrado).Count(),
                        CantidadenProceso = tickets.Where(t => t.CategoriaID == ticket.CategoriaID && t.UsuarioClienteID == clienteUser.Id && t.Estados == Estado.EnProceso).Count()
                    };
                    clienteMostrar.Categorias.Add(categoriaMostrar);
                }
            }

            return clientesMostrar;
        } */
        

       /*  CantidadTicketsAbiertos = tickets.Where(t => t.CategoriaId == ticket.CategoriaId && t.UsuarioClienteID == ticket.UsuarioClienteID && t.Estado == Estado.Abierto).Count(),
                        CantidadTicketProceso = tickets.Where(t => t.CategoriaId == ticket.CategoriaId && t.UsuarioClienteID == ticket.UsuarioClienteID && t.Estado == Estado.EnProceso).Count(),
                        CantidadTicketsCerrados = tickets.Where(t => t.CategoriaId == ticket.CategoriaId && t.UsuarioClienteID == ticket.UsuarioClienteID && t.Estado == Estado.Cerrado).Count(), */