using Ezpeleta2025.Models.Usuario;
using gestionTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
                          .ToArrayAsync();
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
    }


}