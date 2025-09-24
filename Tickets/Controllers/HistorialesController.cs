
using gestionTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gestionTickets.Controllers
{
    [Authorize]
    [Route("api/auth/[controller]")]
    [ApiController]

    public class HistorialesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HistorialesController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/Historial      
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<VistaHistorial>>> GetHistorial(int id)
        {

            var historial = await _context.Historial
                 .Include(h => h.UsuarioCliente) // 👈 Trae la info del usuario
                 .Where(h => h.TicketId == id)
                 .OrderByDescending(h => h.FechaModificacion)
                 .Select(h => new VistaHistorial
                 {
                     HistorialId = h.HistorialId,
                     TicketId = h.TicketId,
                     CamposModificados = h.CamposModificados,
                     ValorAnterior = h.ValorAnterior,
                     ValorNuevo = h.ValorNuevo,
                     FechaModificacionString = h.FechaModificacionString,
                     UsuarioClienteID = h.UsuarioClienteID,
                     NombreUsuario = h.UsuarioCliente != null ? h.UsuarioCliente.NombreCompleto : null,
                     EmailUsuario = h.UsuarioCliente != null ? h.UsuarioCliente.Email : null
                 })
                 .ToListAsync();

            return historial;
            /* return await _context.Historial.Where(h => h.TicketId == id).OrderByDescending(c => c.FechaModificacion).ToListAsync(); */
        }
    }

}