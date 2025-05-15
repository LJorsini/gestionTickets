using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using gestionTickets.Models;
using Microsoft.AspNetCore.Authorization;

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

        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            return await _context.Tickets.ToListAsync();
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

        // GET: api/Categorias/5 --- el 5 hace referencia al id, puede ser cualquier otro número
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
        public async Task<IActionResult> PutTicket(int id, Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return BadRequest();
            }


            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
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

            return NoContent();
        }

        // POST: api/Categorias

        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {

            var ticketExiste = await _context.Tickets.AnyAsync(t => t.TicketId == ticket.TicketId);

            if (ticketExiste == false)
            {
                
                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTicket", new { id = ticket.TicketId }, ticket);

            }
            else
            {
                return BadRequest("El ticket ya existe");
            }


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