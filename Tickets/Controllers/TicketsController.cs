using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using gestionTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Security.Claims;
using System.IO.Compression;

namespace gestionTickets.Controllers
{
    [Authorize]
    [Route("api/auth/[controller]")]
    [ApiController]

    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }



        // GET: api/Tickets
        [HttpGet("ObtenerCategorias")]
        public IActionResult ObtenerCategorias()
        {
            var categorias = _context.Categorias.Select(c => new
            {
                Id = c.CategoriaId,
                Nombre = c.Descripcion

            }).ToList();

            return Json(categorias);
        }

        //Obtener  cliente
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


        [HttpGet(" SelectTicketsPorCliente/{clienteId}")]

        public async Task<ActionResult<IEnumerable<VistaTicket>>> GetTicketsPorCliente(int clienteId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rol = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

          /*   var tickets = _context.Tickets
                //.Include(t => t.Categoria)
                //.Include(t => t.UsuarioCliente)
                .Where(t => t.UsuarioClienteID == clienteId)
                .AsQueryable(); */

                 var tickets = _context.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.UsuarioCliente)
                .AsQueryable();

            
            return Ok();

        }



        // GET: api/tickets/5 --- el 5 hace referencia al id, puede ser cualquier otro número
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }

        // PUT: api/Categorias/5

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, Ticket ticketEditado)
        {
            if (id != ticketEditado.TicketId)
                return BadRequest();

            // Traer el ticket original con su categoría
            var ticketOriginal = await _context.Tickets
                .Include(t => t.Categoria)
                .FirstOrDefaultAsync(t => t.TicketId == id);

            if (ticketOriginal == null)
                return NotFound();

            var historialCambios = new List<Historial>();
            var fechaModificacion = DateTime.Now;

            // Comparar y guardar cambios en historial
            if (ticketOriginal.Titulo != ticketEditado.Titulo)
            {
                historialCambios.Add(new Historial
                {
                    TicketId = ticketEditado.TicketId,
                    CamposModificados = "Título",
                    ValorAnterior = ticketOriginal.Titulo,
                    ValorNuevo = ticketEditado.Titulo,
                    FechaModificacion = fechaModificacion,
                });

                ticketOriginal.Titulo = ticketEditado.Titulo;
            }

            if (ticketOriginal.Descripcion != ticketEditado.Descripcion)
            {
                historialCambios.Add(new Historial
                {
                    TicketId = ticketEditado.TicketId,
                    CamposModificados = "Descripción",
                    ValorAnterior = ticketOriginal.Descripcion,
                    ValorNuevo = ticketEditado.Descripcion,
                    FechaModificacion = fechaModificacion,
                });

                ticketOriginal.Descripcion = ticketEditado.Descripcion;
            }

            if (ticketOriginal.Prioridad != ticketEditado.Prioridad)
            {
                historialCambios.Add(new Historial
                {
                    TicketId = ticketEditado.TicketId,
                    CamposModificados = "Prioridad",
                    ValorAnterior = ticketOriginal.Prioridad.ToString(),
                    ValorNuevo = ticketEditado.Prioridad.ToString(),
                    FechaModificacion = fechaModificacion,
                });

                ticketOriginal.Prioridad = ticketEditado.Prioridad;
            }

            if (ticketOriginal.CategoriaId != ticketEditado.CategoriaId)
            {
                // Manejo seguro si alguna categoría es null
                string valorAnterior = ticketOriginal.Categoria?.Descripcion ?? "Sin categoría";
                string valorNuevo = ticketEditado.Categoria?.Descripcion ?? "Sin categoría";

                historialCambios.Add(new Historial
                {
                    TicketId = ticketEditado.TicketId,
                    CamposModificados = "Categoría",
                    ValorAnterior = valorAnterior,
                    ValorNuevo = valorNuevo,
                    FechaModificacion = fechaModificacion,
                });

                ticketOriginal.CategoriaId = ticketEditado.CategoriaId;
            }

            _context.Update(ticketOriginal);

            if (historialCambios.Any())
                _context.Historial.AddRange(historialCambios);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
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
            ticket.FechaCierre = Convert.ToDateTime("01/01/0001");
            ticket.UsuarioClienteID = userId;
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicket", new { id = ticket.TicketId }, ticket);


        }

        [HttpPost("filtrar")]
        public async Task<ActionResult<IEnumerable<VistaTicket>>> FiltroTickets([FromBody] FiltroTickets filtro)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rol = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            var tickets = _context.Tickets
                .Include(t => t.Categoria)
                .Include(t => t.UsuarioCliente)
                .AsQueryable();

            
            if (rol == "ADMINISTRADOR")
            {
                
            }
            else if (rol == "CLIENTE")
            {
                tickets = tickets.Where(t => t.UsuarioClienteID == userId);
            }
            else if (rol == "DESARROLLADOR")
            {
                tickets = tickets.Where(t =>
                          t.Categoria.PuestosCategorias
                          .Any(pc => pc.Puesto.Desarrolladores
                          .Any(d => d.UsuarioClienteID == userId)));
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
        }





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