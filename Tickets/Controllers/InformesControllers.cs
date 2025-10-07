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
    }


}