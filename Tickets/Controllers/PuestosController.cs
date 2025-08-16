using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gestionTickets.Models
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class PuestosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PuestosController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Puesto>>> GetPuesto()
        {
            var usuarioLoguedoId = HttpContext.User.Identity.Name;
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rol = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            //return await _context.Categorias.ToListAsync();
            return await _context.Puestos.OrderBy(c => c.NombrePuesto).ToListAsync();

        }

        [HttpPost]
        public async Task<ActionResult<Categoria>> PostPuesto(Puesto puesto)
        {

            var puestoExiste = await _context.Puestos.AnyAsync(p => p.NombrePuesto == puesto.NombrePuesto);

            if (puestoExiste == false)
            {
                _context.Puestos.Add(puesto);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPuesto", new { id = puesto.PuestoId }, puesto);

            }
            else
            {
                return BadRequest("La categoria ya existe");
            }


        }

        [HttpPost("asociar")]
        public async Task<ActionResult<Cliente>> PostAsociar(PuestoCategoria asociar)
        {
            _context.PuestoCategorias.Add(asociar);
            await _context.SaveChangesAsync();
            ;
            return Ok();
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> PutPuestos(int id, Puesto puesto)
        {
            if (id != puesto.PuestoId)
            {
                return BadRequest();
            }

            _context.Entry(puesto).State = EntityState.Modified;

            try
            {

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
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

        //habilitar puesto
        [HttpPut("activar/{id}")]
        public async Task<IActionResult> ActivarPuesto(int id)
        {
            var puesto = await _context.Puestos.FindAsync(id);
            if (puesto == null)
            {
                return NotFound();
            }


            puesto.Activo = false;
            await _context.SaveChangesAsync();
            return Ok();
        }

        //deshabilitar puesto
        [HttpPut("desactivar/{id}")]
        public async Task<IActionResult> DesactivarPuesto(int id)
        {
            var puesto = await _context.Puestos.FindAsync(id);
            if (puesto == null)
            {
                return NotFound();
            }

            puesto.Activo = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.CategoriaId == id);
        }

        [HttpGet("mostrarAsociadas")]
        public async Task<ActionResult<IEnumerable<PuestoCategoria>>> GetPuestoCategorias()
        {

            var datos = await _context.PuestoCategorias
                .Include(pc => pc.Puesto)
                .Include(pc => pc.Categoria)
                .Select(pc => new
                {
                    pc.PuestoCategoriaId,
                    nombrePuesto = pc.Puesto.NombrePuesto,
                    descripcionCategoria = pc.Categoria.Descripcion
                })
                .ToListAsync();

            
            var usuarioLoguedoId = HttpContext.User.Identity.Name;
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rol = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            //return await _context.Categorias.ToListAsync();

            return Ok(datos);

        }   ////cambio editar


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var puestoCategoria = await _context.PuestoCategorias.FindAsync(id);
            if (puestoCategoria == null)
            {
                return NotFound();
            }

            _context.PuestoCategorias.Remove(puestoCategoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}