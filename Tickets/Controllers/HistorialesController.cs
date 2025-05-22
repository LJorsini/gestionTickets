using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gestionTickets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<Historial>>> GetHistorial(int id)
        {
            return await _context.Historial.Where(h => h.TicketId == id).OrderByDescending(c => c.FechaModificacion).ToListAsync();
        }  
    }

}