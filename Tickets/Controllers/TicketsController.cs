
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using gestionTickets.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using gestionTickets.Models.Vistas;
using gestionTickets.ModelsVistas;
using Microsoft.AspNetCore.Identity;
using System.Runtime.Intrinsics.X86;

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
            var categorias = _context.Categorias.Select(c => new
            {
                Id = c.CategoriaId,
                Nombre = c.Descripcion

            }).ToList();

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

        [HttpGet("obtenerTickets")]
        public async Task<ActionResult<IEnumerable<VistaTicket>>> GetTickets()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rol = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            var tickets = await _context.Tickets
                .Include(t => t.Categoria)
                .ToListAsync();

            // Filtrar según rol
            if (rol != "ADMINISTRADOR")
            {
                if (rol == "CLIENTE")
                {
                    tickets = tickets.Where(t => t.UsuarioClienteID == userId).ToList();
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

                        tickets = tickets.Where(t => categoriasAsociadas.Contains(t.CategoriaId)).ToList();
                    }
                   
                }
            }

            /* var tickets = await query.ToListAsync(); */
            var vistaTickets = new List<VistaTicket>();

            foreach (var ticket in tickets)
            {
                var usuario = await _userManager.FindByIdAsync(ticket.UsuarioClienteID);

                vistaTickets.Add(new VistaTicket
                {
                    TicketId = ticket.TicketId,
                    Titulo = ticket.Titulo,
                    FechaCreacionString = ticket.FechaCreacion.ToString("dd/MM/yyyy"),
                    CategoriaString = ticket.Categoria?.Descripcion,
                    EstadoString = ticket.Estado.ToString(),
                    Prioridad = (int)ticket.Prioridad,
                    PrioridadString = ticket.Prioridad.ToString(),
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
                PrioridadString = ticket.Prioridad.ToString(),
                CategoriaId = ticket.CategoriaId,
                CategoriaString = ticket.Categoria?.Descripcion,
                EstadoString = ticket.Estado.ToString(),
                FechaCreacionString = ticket.FechaCreacion.ToString("dd/MM/yyyy"),
                UsuarioClienteID = ticket.UsuarioClienteID

            };
            return Ok(ticketMostrar);
        }




        //METODO PARA OBTENER EL INFORME DE 2 NIVELES QUE ME TRAE LOS TICKETS POR CATEGORIA
        /* [HttpGet("ticketsCategorias")]

        public async Task<ActionResult<IEnumerable<VistaCategorias>>> GetTicketsCategorias()
        {
            List<VistaCategorias> vistaCategorias = new List<VistaCategorias>();

            var tickets = _context.Tickets
                          .Include(t => t.Categoria)
                          .AsQueryable();

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rol = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (rol == "CLIENTE")
            {
                tickets = tickets.Where(t => t.UsuarioClienteID == userId);
            }

            foreach (var ticket in tickets)
            {
                var categoriaMostrar = vistaCategorias.FirstOrDefault(c => c.CategoriaId == ticket.CategoriaId);

                if (categoriaMostrar == null)
                {
                    categoriaMostrar = new VistaCategorias
                    {
                        CategoriaId = ticket.CategoriaId,
                        Descripcion = ticket.CategoriaString,
                        Tickets = new List<VistaTicket>()
                    };

                    
                    vistaCategorias.Add(categoriaMostrar);
                }

                var ticketMostrar = new VistaTicket
                {
                    TicketId = ticket.TicketId,
                    Titulo = ticket.Titulo,
                    Prioridad = ticket.Prioridad,
                    EstadoString = ticket.Estado.ToString(),
                    FechaCreacionString = ticket.FechaCreacion.ToString("dd/MM/yyyy"),
                    PrioridadString = ticket.Prioridad.ToString(),
                    CategoriaString = ticket.Categoria != null ? ticket.Categoria.Descripcion : null,
                    UsuarioClienteID = ticket.UsuarioClienteID,
                    NombreUsuario = ticket.UsuarioCliente != null ? ticket.UsuarioCliente.NombreCompleto : null,
                    EmailUsuario = ticket.UsuarioCliente != null ? ticket.UsuarioCliente.Email : null
                };

                categoriaMostrar.Tickets.Add(ticketMostrar);
        }
        return vistaCategorias.ToList();
        }
 */


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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.TicketId == id);
        }

    }
}