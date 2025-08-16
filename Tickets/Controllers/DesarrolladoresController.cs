using System.Security.Claims;
using gestionTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gestionTickets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class DesarrolladoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _rolManager;
        private readonly IConfiguration _configuration;

        public DesarrolladoresController(
                ApplicationDbContext context,
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                RoleManager<IdentityRole> rolManager,
                IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _rolManager = rolManager;
            _configuration = configuration;
        }

        // GET: api/Tickets
        [HttpGet]

        public async Task<ActionResult<IEnumerable<VistaDesarrollador>>> GetDesarrollador()
        {
            List<VistaDesarrollador> vistaDesarrolladores = new List<VistaDesarrollador>();

            var desarrolladores = await _context.Desarrolladores.Include(t => t.Puesto).ToListAsync();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            /* var desarrrollador = await _context.Desarrolladores
                .Include(t => t.Puesto)
                .Where(t => t.UsuarioClienteID == userId)
                .ToListAsync(); */



            foreach (var desarrollador in desarrolladores.OrderByDescending(t => t.NombreCompleto))
            {
                var mostrarDesarrollador = new VistaDesarrollador
                {
                    DesarrolladorId = desarrollador.DesarrolladorId,
                    NombreCompleto = desarrollador.NombreCompleto,
                    Email = desarrollador.Email,
                    Telefono = desarrollador.Telefono,
                    DNI = desarrollador.DNI,
                    Observacion = desarrollador.Observacion,
                    PuestoId = desarrollador.PuestoId,
                    NombrePuesto = desarrollador.Puesto?.NombrePuesto
                };

                vistaDesarrolladores.Add(mostrarDesarrollador);
            }
            return Ok(vistaDesarrolladores);
        }

        /* [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, Desarrollador desarrollador)
        {
            if (id != desarrollador.DesarrolladorId)
            {
                return BadRequest();
            }

            _context.Entry(desarrollador).State = EntityState.Modified;

            try
            {

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DesarrolladorExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        } */

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDesarrollador(int id, Desarrollador desarrollador)
        {
            if (id != desarrollador.DesarrolladorId)
            {
                return BadRequest();
            }

            _context.Entry(desarrollador).State = EntityState.Modified;

            try
            {

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DesarrolladorExist(id))
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


        [HttpGet("{id}")]
        public async Task<ActionResult<Desarrollador>> GetDesarrollador(int id)
        {
            var desarrollador = await _context.Desarrolladores.FindAsync(id);

            if (desarrollador == null)
            {
                return NotFound();
            }

            return Ok(desarrollador);
        }




        [HttpPost]
        public async Task<ActionResult<Desarrollador>> PostCliente(Desarrollador desarrollador)
        {
            /* var nombreRolCrearExiste = _context.Roles.Where(r => r.Name == "DESARROLLADOR").SingleOrDefault();
            if (nombreRolCrearExiste == null)
            {
                var roleResult = await _rolManager.CreateAsync(new IdentityRole("DESARROLLADOR"));
            } */
            _context.Desarrolladores.Add(desarrollador);
            await _context.SaveChangesAsync();



            return Ok();
        }

        private bool DesarrolladorExist(int id)
        {
            return _context.Desarrolladores.Any(e => e.DesarrolladorId == id);
        }


    }
}