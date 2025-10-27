
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using gestionTickets.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using gestionTickets.Models.Vistas;
using gestionTickets.ModelsVistas;
using Microsoft.AspNetCore.Identity;
using System.Runtime.Intrinsics.X86;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Http.HttpResults;
using System.IO.Compression;

namespace gestionTickets.Controllers
{
    [Authorize]
    [Route("api/auth/[controller]")]
    [ApiController]

    public class TicketsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public TicketsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        //GET para llenar desplegables
        //OBTENGO CLIENTES PARA EL SELECT DE FILTRAR TICKETS POR CLIENTE
        [HttpGet("SelectClientes")]

        public IActionResult ObtenerClientes()
        {
            var clientes = _context.Clientes.Select(c => new
            {
                ClienteId = c.ClienteId,
                Nombre = c.Nombre,
            }
            ).ToList();

            return Ok(clientes);
        }
        //OBTENGO LAS CATEGORIAS PARA LLENAR EL DROPDOWN DEL SELECT
        [HttpGet("categorias")]
        public IActionResult ObtenerCategorias()
        {
            var categorias = _context.Categorias.ToList();

            return Json(categorias);
        }


        //Metodos GET....Aca agrupo todos los metodos GET

        /* [HttpGet("obtenerTickets")]
        public async Task<ActionResult<IEnumerable<VistaTicket>>> GetTickets()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rol = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            IQueryable<Ticket> query = _context.Tickets
                .Include(t => t.Categoria);

            
            if (!string.Equals(rol, "ADMINISTRADOR", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(rol, "CLIENTE", StringComparison.OrdinalIgnoreCase))
                {
                    
                    query = query.Where(t => t.UsuarioClienteID == userId);
                }
                else if (string.Equals(rol, "DESARROLLADOR", StringComparison.OrdinalIgnoreCase))
                {
                    var desarrollador = await _context.Desarrolladores
                        .Include(d => d.Puesto)
                        .FirstOrDefaultAsync(d => d.UsuarioClienteID == userId);

                    if (desarrollador != null)
                    {
                        var categoriaIds = await _context.PuestoCategorias
                            .Where(pc => pc.PuestoId == desarrollador.PuestoId)
                            .Select(pc => pc.CategoriaId)
                            .ToListAsync();

                        query = query.Where(t => categoriaIds.Contains(t.CategoriaId));
                    }
                    else
                    {
                        
                        query = query.Where(t => false);
                    }
                }
            }

            var tickets = await query.ToListAsync();

            var vistaTickets = new List<VistaTicket>();

            foreach (var ticket in tickets)
            {
                var usuario = await _userManager.FindByIdAsync(ticket.UsuarioClienteID);

                vistaTickets.Add(new VistaTicket
                {
                    TicketId = ticket.TicketId,
                    Titulo = ticket.Titulo,
                    FechaCreacionString = ticket.FechaCreacionString,
                    CategoriaString = ticket.CategoriaString,
                    EstadoString = ticket.EstadoString,
                    Prioridad = (int)ticket.Prioridad,
                    PrioridadString = ticket.PrioridadString,
                    NombreUsuario = usuario?.NombreCompleto,
                    EmailUsuario = usuario?.Email
                });
            }

            return Ok(vistaTickets);
        }*/

        [HttpPost("obtenerTicketsFiltrar")]
        public async Task<ActionResult<IEnumerable<VistaTicket>>> GetTickets([FromBody] FiltroTickets? filtro)
        {
            var vistaTickets = new List<VistaTicket>();

            
            var ticketsQuery = _context.Tickets
                .Include(t => t.Categoria)
                .AsQueryable();

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rol = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            //Filtrar según rol
            if (rol != "ADMINISTRADOR")
            {
                if (rol == "CLIENTE")
                {
                    ticketsQuery = ticketsQuery.Where(t => t.UsuarioClienteID == userId);
                }
                else if (rol == "DESARROLLADOR")
                {
                    var desarrollador = await _context.Desarrolladores
                        .Include(d => d.Puesto)
                        .FirstOrDefaultAsync(d => d.UsuarioClienteID == userId);

                    if (desarrollador != null)
                    {
                        var categoriasAsociadas = await _context.PuestoCategorias
                            .Where(pc => pc.PuestoId == desarrollador.PuestoId)
                            .Select(pc => pc.CategoriaId)
                            .ToListAsync();

                        ticketsQuery = ticketsQuery.Where(t => categoriasAsociadas.Contains(t.CategoriaId));
                    }
                    else
                    {
                        ticketsQuery = ticketsQuery.Where(t => false); // no tiene categorías asignadas
                    }
                }
            }

            //Filtro de fechas
            if (!string.IsNullOrEmpty(filtro.FechaDesde) && !string.IsNullOrEmpty(filtro.FechaHasta))
            {
                if (DateTime.TryParse(filtro.FechaDesde, out var fechaDesde) && DateTime.TryParse(filtro.FechaHasta, out var fechaHasta))
                {
                    // Incluir todo el día hasta las 23:59:59
                    fechaHasta = fechaHasta.AddDays(1).AddSeconds(-1);
                    ticketsQuery = ticketsQuery.Where(t => t.FechaCreacion >= fechaDesde && t.FechaCreacion <= fechaHasta);
                }
            }

            //Filtros simples
            if (filtro.CategoriaId > 0)
                ticketsQuery = ticketsQuery.Where(t => t.CategoriaId == filtro.CategoriaId);

            if (filtro.Prioridad > 0)
                ticketsQuery = ticketsQuery.Where(t => t.Prioridad == (Prioridad)filtro.Prioridad);

            if (filtro.Estado > 0)
                ticketsQuery = ticketsQuery.Where(t => t.Estado == (Estado)filtro.Estado);

            // El TolistAsync hace que se ejecute la consulta una sola vez en la base de datos
            var tickets = await ticketsQuery.ToListAsync();

            //Creo las vistas
            foreach (var ticket in tickets)
            {
                var usuario = await _userManager.FindByIdAsync(ticket.UsuarioClienteID);

                vistaTickets.Add(new VistaTicket
                {
                    TicketId = ticket.TicketId,
                    Titulo = ticket.Titulo,
                    FechaCreacionString = ticket.FechaCreacionString,
                    CategoriaString = ticket.Categoria?.Descripcion,
                    EstadoString = ticket.EstadoString,
                    Prioridad = (int)ticket.Prioridad,
                    PrioridadString = ticket.PrioridadString,
                    NombreUsuario = usuario?.NombreCompleto,
                    EmailUsuario = usuario?.Email
                });
            }

            return Ok(vistaTickets);
        }







        /* [HttpGet("obtenerTickets")]
        public async Task<ActionResult<IEnumerable<VistaTicket>>> GetTickets()
        {
            List<VistaTicket> vistaTickets = new List<VistaTicket>();

            
            var tickets = await _context.Tickets
                .Include(t => t.Categoria)
                .ToListAsync();

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rol = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            
            if (!string.Equals(rol, "ADMINISTRADOR", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(rol, "CLIENTE", StringComparison.OrdinalIgnoreCase))
                {
                    
                    tickets = tickets.Where(t => t.UsuarioClienteID == userId).ToList();
                }
                else if (string.Equals(rol, "DESARROLLADOR", StringComparison.OrdinalIgnoreCase))
                {
                    
                    var desarrollador = await _context.Desarrolladores
                                                    .Include(d => d.Puesto)
                                                    .FirstOrDefaultAsync(d => d.UsuarioClienteID == userId);

                    if (desarrollador != null && desarrollador.PuestoLaboralID.HasValue)
                    {
                        var categoriaIds = await _context.PuestoCategorias
                                                        .Where(pc => pc.Puesto == desarrollador.PuestoLaboralID.Value)
                                                        .Select(pc => pc.CategoriaId)
                                                        .ToListAsync();

                        tickets = tickets.Where(t => categoriaIds.Contains(t.CategoriaId));
                    }
                    else
                    {
                        
                        tickets = tickets.Where(t => false).ToList();
                    }
                }
            }   

            foreach (var ticket in tickets)
            {
                var usuario = await _userManager.FindByIdAsync(ticket.UsuarioClienteID);

                var ticketMostrar = new VistaTicket
                {
                    TicketId = ticket.TicketId,
                    Titulo = ticket.Titulo,
                    FechaCreacionString = ticket.FechaCreacionString,
                    CategoriaString = ticket.CategoriaString,
                    EstadoString = ticket.EstadoString,
                    Prioridad = (int)ticket.Prioridad,
                    PrioridadString = ticket.PrioridadString,
                    NombreUsuario = usuario?.NombreCompleto, 
                };

                vistaTickets.Add(ticketMostrar);
            }

            return Ok(vistaTickets);
        } */


        [HttpGet("{id}")]

        public async Task<ActionResult<VistaTicket>> GetTicketById(int id)

        {
            var ticket = await _context.Tickets
                         .Include(t => t.Categoria)
                         .FirstOrDefaultAsync(t => t.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            var ticketMostrar = new VistaTicket
            {
                TicketId = ticket.TicketId,
                Titulo = ticket.Titulo,
                Descripcion = ticket.Descripcion,
                Prioridad = (int)ticket.Prioridad,     // valor para el select
                PrioridadString = ticket.PrioridadString,
                CategoriaId = ticket.CategoriaId,
                CategoriaString = ticket.Categoria?.Descripcion,
                EstadoString = ticket.Estado.ToString(),
                FechaCreacionString = ticket.FechaCierreString,
                UsuarioClienteID = ticket.UsuarioClienteID

            };
            return Ok(ticketMostrar);
        }




        //METODO PARA OBTENER EL INFORME DE 2 NIVELES QUE ME TRAE LOS TICKETS POR CATEGORIA

        [HttpGet("ticketsCategorias")]

        public async Task<ActionResult<IEnumerable<VistaCategorias>>> GetTicketsCategorias()
        {
            List<VistaCategorias> vistaCategoria = new List<VistaCategorias>();

            var tickets = _context.Tickets
                          .Include(t => t.Categoria)
                          .AsQueryable();

            foreach (var ticket in tickets)
            {
                var mostrarCategoria = vistaCategoria.FirstOrDefault(c => c.CategoriaId == ticket.CategoriaId);

                if (mostrarCategoria == null)
                {
                    mostrarCategoria = new VistaCategorias
                    {
                        CategoriaId = ticket.CategoriaId,
                        Descripcion = ticket.CategoriaString,
                        Tickets = new List<VistaTicket>()


                    };
                }

                vistaCategoria.Add(mostrarCategoria);

                var mostrarTicket = new VistaTicket
                {
                    TicketId = ticket.TicketId,
                    Titulo = ticket.Titulo,
                    Prioridad = (int)ticket.Prioridad,
                    EstadoString = ticket.Estado.ToString(),
                    FechaCreacionString = ticket.FechaCreacion.ToString("dd/MM/yyyy"),
                    PrioridadString = ticket.Prioridad.ToString(),
                    CategoriaString = ticket.Categoria != null ? ticket.Categoria.Descripcion : null,
                    UsuarioClienteID = ticket.UsuarioClienteID,
                    /* NombreUsuario = ticket.UsuarioCliente != null ? ticket.UsuarioCliente.NombreCompleto : null,
                    EmailUsuario = ticket.UsuarioCliente != null ? ticket.UsuarioCliente.Email : null */
                };

                mostrarCategoria.Tickets.Add(mostrarTicket);

            }
            return vistaCategoria.ToList();
        }

        [HttpGet("ticketsClientes")]

        public async Task<ActionResult<IEnumerable<VistaCliente>>> GetTicketsClientes()
        {
            List<VistaCliente> vistaClientes = new List<VistaCliente>();

            var clientes = await _context.Clientes.ToListAsync();

            /* var ticketsCliente = await _context.Tickets
                              .Include(t => t.Categoria)
                              .Where(t => t.UsuarioClienteID == clientes.UsuarioClienteID)
                              .ToListAsync(); */

            foreach (var cliente in clientes)
            {
                var mostrarCliente = vistaClientes.FirstOrDefault(c => c.ClienteId == cliente.ClienteId);

                if (mostrarCliente == null)
                {
                    mostrarCliente = new VistaCliente
                    {
                        Nombre = cliente.Nombre,
                        Email = cliente.Email,
                        Tickets = new List<VistaTicket>()

                    };

                    vistaClientes.Add(mostrarCliente);
                }

                var ticketCliente = await _context.Tickets
                                    .Include(t => t.Categoria)
                                    .Where(t => t.UsuarioClienteID == cliente.UsuarioClienteID)
                                    .ToListAsync();

                foreach (var ticket in ticketCliente)
                {
                    var mostrarTicket = new VistaTicket
                    {
                        Titulo = ticket.Titulo,


                    };
                    mostrarCliente.Tickets.Add(mostrarTicket);

                }

            }



            return Ok(vistaClientes);

        }

        [HttpGet("informeCerrados")]

        public async Task<ActionResult<IEnumerable<VistaTicket>>> GetTicketPorDesarrollador()
        {

            List<VistaDesarrollador> vistaDesarrollador = new List<VistaDesarrollador>();

            var desarrolladores = await _context.Desarrolladores.ToListAsync();

            foreach (var desarrollador in desarrolladores)
            {
                var mostrarDesarrollador = vistaDesarrollador.FirstOrDefault(t => t.DesarrolladorId == desarrollador.DesarrolladorId);

                if (mostrarDesarrollador == null)
                {
                    mostrarDesarrollador = new VistaDesarrollador
                    {
                        NombreCompleto = desarrollador.NombreCompleto,
                        Email = desarrollador.Email,
                        TicketsCerrados = new List<VistaTicket>(),
                    };

                    vistaDesarrollador.Add(mostrarDesarrollador);


                }

                var ticketsCerrados = await _context.Tickets
                                      .Include(t => t.Categoria)
                                      .Where(t => t.Cerro == desarrollador.UsuarioClienteID).ToListAsync();

                foreach (var ticketCerrado in ticketsCerrados)
                {
                    var mostrarTicket = new VistaTicket
                    {
                        Titulo = ticketCerrado.Titulo,

                    };
                    mostrarDesarrollador.TicketsCerrados.Add(mostrarTicket);


                }

            }

            return Ok(vistaDesarrollador);
        }



        /* GTickets ultimo 4 meses */

        [HttpGet("graficoBarraMes")]

        public async Task<ActionResult<IEnumerable<VistaTicketMes>>> GetTicketCerradosMes()
        {
            List<VistaTicketMes> vistaTicketMes = new List<VistaTicketMes>();

            var FechaActual = DateTime.Now;

            var ticketsCerrados = await _context.Tickets
                                  .Where(t => t.Estado == Estado.Cerrado)
                                  .ToListAsync();

            for (int i = 3; i >= 0; i--)
            {
                var mes = FechaActual.AddMonths(-i);

                var cantidad = ticketsCerrados
                               .Where(t => t.FechaCierre.Month == mes.Month && t.FechaCierre.Year == mes.Year)
                               .Count();

                vistaTicketMes.Add(new VistaTicketMes
                {
                    Mes = mes.Month,
                    Anio = mes.Year,
                    CantidadCerrados = cantidad,

                });


            }
            return vistaTicketMes;
        }

        [HttpGet("graficoBarrasCreadosCerrados")]

        public async Task<ActionResult<IEnumerable<VistaTicketMes>>> GetTicketCreadosCerrados()
        {
            List<VistaTicketMes> vistaTicketMes = new List<VistaTicketMes>();

            var FechaActual = DateTime.Now;

            var ticketsCerrados = await _context.Tickets
                                  .Where(t => t.Estado == Estado.Cerrado)
                                  .ToListAsync();


            var ticketsCreadaos = await _context.Tickets
                                 .ToListAsync();


            for (int i = 5; i >= 0; i--)
            {
                var mes = FechaActual.AddMonths(-i);

                var cantidadCerrados = ticketsCerrados
                               .Where(t => t.FechaCierre.Month == mes.Month && t.FechaCierre.Year == mes.Year)
                               .Count();

                var cantidadCreados = ticketsCreadaos
                                      .Where(t => t.FechaCreacion.Month == mes.Month && t.FechaCreacion.Year == mes.Year)
                                      .Count();


                vistaTicketMes.Add(new VistaTicketMes
                {
                    Mes = mes.Month,
                    Anio = mes.Year,
                    CantidadCerrados = cantidadCerrados,
                    CantidadCreados = cantidadCreados,

                });
            }
            return vistaTicketMes;
        }



        [HttpGet("getTicketsPorCliente/{clienteID}")]
        public async Task<ActionResult<IEnumerable<VistaTicket>>> GetTicketsPorCliente(int clienteID)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var rol = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

                List<VistaTicket> ticketsCliente = new List<VistaTicket>();

                var cliente = _context.Clientes.FirstOrDefault(c => c.ClienteId == clienteID);

                if (cliente == null)
                {
                    return NotFound(new { message = $"No existe un cliente con el ID {clienteID}" });
                }

                var usuarioId = cliente.UsuarioClienteID; //hago esto para traer el usuarioId del cliente seleccionado, si no no puedo comparar un string con un int

                if (string.IsNullOrEmpty(cliente.UsuarioClienteID))
                {
                    return BadRequest(new { message = "El cliente no tiene un UsuarioClienteID asociado" });
                }

                var tickets = await _context.Tickets
                              .Include(t => t.Categoria)
                              .Where(t => t.UsuarioClienteID == usuarioId)
                              .ToListAsync();

                foreach (var ticket in tickets)
                {


                    var vistaTicket = new VistaTicket
                    {
                        TicketId = ticket.TicketId,
                        Titulo = ticket.Titulo,
                        Prioridad = (int)ticket.Prioridad,
                        EstadoString = ticket.EstadoString,
                        FechaCreacionString = ticket.FechaCreacion.ToString("dd/MM/yyyy"),
                        PrioridadString = ticket.Prioridad.ToString(),
                        CategoriaString = ticket.Categoria != null ? ticket.Categoria.Descripcion : null,
                        UsuarioClienteID = ticket.UsuarioClienteID,
                        /* NombreUsuario = ticket.UsuarioCliente != null ? ticket.UsuarioCliente.NombreCompleto : null,
                        EmailUsuario = ticket.UsuarioCliente != null ? ticket.UsuarioCliente.Email : null */
                    };
                    ticketsCliente.Add(vistaTicket);

                }

                return Ok(ticketsCliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener tickets", detalle = ex.Message });
            }
        }

        [HttpGet("informeHome")]

        public async Task<ActionResult<IEnumerable<VistaTicket>>> GetDatosHome()
        {
            List<VistaTicket> vistaHome = new List<VistaTicket>();

            var tickets = await _context.Tickets
                          .Include(t => t.Categoria)
                          .ToListAsync();

            foreach (var ticket in tickets)
            {
                var mostrarTicket = new VistaTicket
                {
                    TicketId = ticket.TicketId,
                    Titulo = ticket.Titulo,
                    Descripcion = ticket.Descripcion,
                    EstadoString = ticket.EstadoString,
                    PrioridadString = ticket.PrioridadString,
                    FechaCreacionString = ticket.FechaCreacionString,
                    FechaComienzoString = ticket.FechaComienzoString,
                    FechaCierreString = ticket.FechaCierreString,
                };

                vistaHome.Add(mostrarTicket);
            }

            return vistaHome;


        }
        /* [HttpGet("GetTicketsPorCliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<VistaTicket>>> GetTicketsPorCliente(int clienteId)
        {
            try
            {
                var usuario = _context.Clientes.FirstOrDefault(c => c.ClienteId == clienteId);

                var usuarioId = usuario.UsuarioClienteID;

                var tickets = await _context.Tickets
                .OrderByDescending(t => t.FechaCreacion)
                    .Include(t => t.Categoria)
                    .Where(t => t.UsuarioClienteID == usuarioId)
                    .Select(t => new VistaTicket
                    {
                        TicketId = t.TicketId,
                        Titulo = t.Titulo,
                        Prioridad = t.Prioridad,
                        EstadoString = t.Estado.ToString(),
                        FechaCreacionString = t.FechaCreacion.ToString("dd/MM/yyyy"),
                        PrioridadString = t.Prioridad.ToString(),
                        CategoriaString = t.Categoria != null ? t.Categoria.Descripcion : null,
                        UsuarioClienteID = t.UsuarioClienteID,
                        NombreUsuario = t.UsuarioCliente != null ? t.UsuarioCliente.NombreCompleto : null,
                        EmailUsuario = t.UsuarioCliente != null ? t.UsuarioCliente.Email : null
                    })
                    .ToListAsync();

                return Ok(tickets);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new { message = "Error al obtener tickets", detalle = ex.Message });
            }
        }
 */



        // GET: api/tickets/5 --- el 5 hace referencia al id, puede ser cualquier otro número
        /* [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        } */

        // PUT: api/Categorias/5


        [HttpPut("{id}")]

        public async Task<IActionResult> PutTicketEditado(int id, Ticket ticketEditado)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var ticketOriginal = await _context.Tickets
                                 .Include(t => t.Categoria)
                                 .FirstOrDefaultAsync(t => t.TicketId == id);

                DateTime fechaCambio = DateTime.Now;

                if (ticketOriginal.Titulo != ticketEditado.Titulo)
                {
                    var modificacionTitulo = new Historial
                    {
                        TicketId = ticketEditado.TicketId,
                        CamposModificados = "Titulo",
                        ValorAnterior = ticketOriginal.Titulo,
                        ValorNuevo = ticketEditado.Titulo,
                        FechaModificacion = fechaCambio,
                        UsuarioClienteID = userId,


                    };
                    _context.Historial.Add(modificacionTitulo);
                    ticketOriginal.Titulo = ticketEditado.Titulo;

                }

                if (ticketOriginal.Descripcion != ticketEditado.Descripcion)
                {
                    var modificacionDescripcion = new Historial
                    {
                        TicketId = ticketEditado.TicketId,
                        CamposModificados = "Descripcion",
                        ValorAnterior = ticketOriginal.Descripcion,
                        ValorNuevo = ticketEditado.Descripcion,
                        FechaModificacion = fechaCambio,
                        UsuarioClienteID = userId,

                    };
                    _context.Historial.Add(modificacionDescripcion);
                    ticketOriginal.Descripcion = ticketEditado.Descripcion;
                }

                if (ticketOriginal.Prioridad != ticketEditado.Prioridad)
                {
                    var modificacionPrioridad = new Historial
                    {
                        TicketId = ticketEditado.TicketId,
                        CamposModificados = "Prioridad",
                        ValorAnterior = ticketOriginal.PrioridadString,
                        ValorNuevo = ticketEditado.PrioridadString,
                        FechaModificacion = fechaCambio,
                        UsuarioClienteID = userId,

                    };
                    _context.Historial.Add(modificacionPrioridad);
                    ticketOriginal.Prioridad = ticketEditado.Prioridad;
                }

                if (ticketOriginal.CategoriaId != ticketEditado.CategoriaId)
                {
                    var categoriaAnterior = _context.Categorias.Where(c => c.CategoriaId == ticketEditado.CategoriaId).Single();
                    var categoriaNueva = _context.Categorias.Where(c => c.CategoriaId == ticketEditado.CategoriaId).Single();

                    var modificacionCategoria = new Historial
                    {
                        TicketId = ticketEditado.TicketId,
                        CamposModificados = "Categoria",
                        ValorAnterior = categoriaAnterior.Descripcion,
                        ValorNuevo = categoriaNueva.Descripcion,
                        FechaModificacion = fechaCambio,
                        UsuarioClienteID = userId,

                    };
                    _context.Historial.Add(modificacionCategoria);
                    ticketOriginal.CategoriaId = ticketEditado.CategoriaId;

                }

                await _context.SaveChangesAsync();
            }
            catch
            {
                if (!TicketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        [HttpPost("estadoTicket/{ticketId}")]

        public async Task<IActionResult> EstadoTicket(int ticketId)
        {
            var usuarioLogueadoId = HttpContext.User.Identity.Name;
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rol = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket == null)
            {
                return BadRequest("El ticket no existe");
            }

            ticket.Estado = Estado.EnProceso;
            ticket.FechaComienzo = DateTime.Now;


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("finalizarTicket/{ticketId}")]

        public async Task<IActionResult> FinalizarTicket(int ticketId)
        {
            var usuarioLogueadoId = HttpContext.User.Identity.Name;
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rol = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket == null)
            {
                return BadRequest("El ticket no existe");
            }

            ticket.Estado = Estado.Cerrado;
            ticket.FechaCierre = DateTime.Now;
            ticket.Cerro = userId;


            await _context.SaveChangesAsync();
            return Ok();
        }



        // POST: api/Categorias

        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {
            var usuarioLogueadoId = HttpContext.User.Identity.Name;
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rol = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            ticket.Estado = Estado.Abierto;
            ticket.FechaCreacion = DateTime.Now;
            ticket.FechaComienzo = Convert.ToDateTime("01/01/0001");
            ticket.FechaCierre = Convert.ToDateTime("01/01/0001");

            ticket.UsuarioClienteID = userId;



            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTickets", new { id = ticket.TicketId }, ticket);


        }

        /* -------------------------------------------------------------------------------------------------------------------------- */
        /* [HttpPost("filtrar")]
                public async Task<ActionResult<IEnumerable<VistaTicket>>> FiltroTickets([FromBody] FiltroTickets filtro)
                {
                    var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var rol = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

                    var tickets = _context.Tickets
                        .Include(t => t.Categoria)
                        .Include(t => t.UsuarioCliente)
                        .Where(t => t.UsuarioClienteID == userId)
                        .AsQueryable();


                     if (rol == "ADMINISTRADOR")
                     {

                     }


                    if (DateTime.TryParse(filtro.FechaDesde, out var fechaDesde) &&
                        DateTime.TryParse(filtro.FechaHasta, out var fechaHasta))
                    {
                        fechaHasta = fechaHasta.AddHours(23).AddMinutes(59).AddSeconds(59);
                        tickets = tickets.Where(t => t.FechaCreacion >= fechaDesde && t.FechaCreacion <= fechaHasta);
                    }

                    // 🔎 Filtro por categoría
                    if (filtro.CategoriaId > 0)
                    {
                        tickets = tickets.Where(t => t.CategoriaId == filtro.CategoriaId);
                    }

                    // 🔎 Filtro por prioridad
                    if (filtro.Prioridad > 0)
                    {
                        tickets = tickets.Where(t => t.Prioridad == (Prioridad)filtro.Prioridad);
                    }

                    // 🔎 Filtro por estado
                    if (filtro.Estado > 0)
                    {
                        tickets = tickets.Where(t => t.Estado == (Estado)filtro.Estado);
                    }


                    var vista = await tickets
                        .OrderByDescending(t => t.FechaCreacion)
                        .Select(ticket => new VistaTicket
                        {
                            TicketId = ticket.TicketId,
                            Titulo = ticket.Titulo,
                            FechaCreacionString = ticket.FechaCreacionString,
                            Prioridad = ticket.Prioridad,
                            EstadoString = ticket.EstadoString,
                            CategoriaString = ticket.CategoriaString,
                            PrioridadString = ticket.PrioridadString,
                            UsuarioClienteID = ticket.UsuarioClienteID,
                            NombreUsuario = ticket.UsuarioCliente != null ? ticket.UsuarioCliente.NombreCompleto : null,
                            EmailUsuario = ticket.UsuarioCliente != null ? ticket.UsuarioCliente.Email : null

                        })
                        .ToListAsync();

                    return vista;
                } */

        /* -------------------------------------------------------------------------------------------------------------------------- */



        // DELETE: api/Categorias/5
        [HttpPost("cancelar/{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ticket.Estado = Estado.Cancelado;

            
            await _context.SaveChangesAsync();

            return NoContent();
        }



        private bool TicketExists(int id)
        {



            return _context.Tickets.Any(e => e.TicketId == id);
        }

       

    

    }


    

}