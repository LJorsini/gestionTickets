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

namespace gestionTickets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Tickets
        [HttpGet]

        public async Task<ActionResult<IEnumerable<VistaTicket>>> GetTickets()
        {
            var tickets = await _context.Tickets.Include(t => t.Categoria).ToListAsync();

            var vistaTickets = tickets.Select(t => new VistaTicket
            {

                Titulo = t.Titulo,
                Descripcion = t.Descripcion,
                Estado = t.Estado.ToString(),
                Prioridad = t.Prioridad.ToString(),
                FechaCreacion = t.FechaCreacion.ToString("dd/MM/yyyy"),
                CategoriaNombre = t.Categoria.Descripcion,
                TicketId = t.TicketId,
                CategoriaId = t.CategoriaId


            });
            return Json(vistaTickets);
        }
        [HttpGet("ObtenerEstadosyPrioridad")] //obtengo los estados de los tickets
        public IActionResult ObtenerEstadosyPrioridad()
        {
            var estados = Enum.GetValues(typeof(Estado))
                .Cast<Estado>()
                .Select(e => new
                {
                    Id = (int)e,
                    Nombre = e.ToString()
                });

            var prioridades = Enum.GetValues(typeof(Prioridad))
            .Cast<Prioridad>()
            .Select(e => new
            {
                Id = (int)e,
                Nombre = e.ToString()
            });

            var resultado = new
            {
                estados,
                prioridades
            };

            return Json(resultado);
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

        /* [HttpGet("ObtenerHistorialTicket/{ticketId}")]

        public IActionResult GetHistorial(int ticketId)
        {
            var historial = _context.Historial.Include(t => t.Ticket).Where(h => h.TicketId == ticketId).ToList();

            return Json(historial);
        } */


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

    var ticketExiste = await _context.Tickets.AnyAsync(t => t.TicketId == ticket.TicketId);

    if (ticketExiste == false)
    {
        ticket.Estado = Estado.Abierto;
        ticket.FechaCreacion = DateOnly.FromDateTime(DateTime.Now);
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTicket", new { id = ticket.TicketId }, ticket);

    }
    else
    {
        return BadRequest("El ticket ya existe");
    }


}

/* [HttpPost]
public async Task<ActionResult<Ticket>> PostHistorial(Ticket historial)
{

    var historialExiste = await _context.Historial.AnyAsync(t => t.HistorialId == historial.TicketId);

    if (ticketExiste == false)
    {
        ticket.Estado = Estado.Abierto;
        ticket.FechaCreacion = DateOnly.FromDateTime(DateTime.Now);
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTicket", new { id = ticket.TicketId }, ticket);

    }
    else
    {
        return BadRequest("El ticket ya existe");
    }


}
*/

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