using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

            /* var puestos = await _context.Puestos
                .Include(p => p.PuestosCategorias)
                .Select(p => new
                {
                    p.PuestoId,
                    p.NombrePuesto,
                    p.Activo,
                    PuestoCategoria = p.PuestosCategorias.Select(pc => new
                    {
                        pc.PuestoCategoriaId,
                    })
                })
                .OrderBy(p => p.NombrePuesto)
                .ToListAsync();

            return Ok(puestos); */
        }

        [HttpPost]
        public async Task<ActionResult<Categoria>> PostPuesto(Puesto puesto)
        {
            try
            {
                var puestoExiste = await _context.Puestos.AnyAsync(p => p.NombrePuesto == puesto.NombrePuesto);

                if (puestoExiste)
                {
                    return Conflict("El puesto ya existe"); //devuelve un 409, el registro ya existe
                }

                _context.Puestos.Add(puesto);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPuesto), new { id = puesto.PuestoId }, puesto); //devuelve un 201, se creo el regiatro

            }

            catch (DbUpdateException ex) //error 500, errores en la base de datos
            {
                return StatusCode(500, $"Error al guardar en la base de datos: {ex.InnerException?.Message ?? ex.Message}");
            }

            catch (Exception ex)
            {
                // Errores inesperados
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }

        }

        /* [HttpPost("asociar")]
        public async Task<ActionResult> PostAsociar([FromBody] PuestoCategoria asociar)
        {


            try
            {
                var asociacionExiste = await _context.PuestoCategorias.AnyAsync(pc => pc.PuestoId == asociar.PuestoId && pc.CategoriaId == asociar.CategoriaId);

                if (asociacionExiste)
                {
                    return Conflict("La asociacion ya existe"); 
                }

                Console.WriteLine($"PuestoId: {asociar.PuestoId}, CategoriaId: {asociar.CategoriaId}");

                _context.PuestoCategorias.Add(asociar);
                await _context.SaveChangesAsync();

                return Ok();
                
                
            }
            catch
            {
                return StatusCode(500, "Error al guardar en la base de datos");
            }
            

           
            
            
        }
 */
[HttpPost("asociar")]
public async Task<ActionResult> PostAsociar([FromBody] PuestoCategoria asociar)
{
    if (asociar == null)
        return BadRequest("El body está vacío");

    Console.WriteLine($"PuestoId: {asociar.PuestoId}, CategoriaId: {asociar.CategoriaId}");

    try
    {
        var existe = await _context.PuestoCategorias
            .AnyAsync(pc => pc.PuestoId == asociar.PuestoId && pc.CategoriaId == asociar.CategoriaId);

        if (existe)
            return Conflict("La asociación ya existe");

        _context.PuestoCategorias.Add(asociar);
        await _context.SaveChangesAsync();

        return Ok(asociar); // devuelve el objeto creado
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Error al guardar en la base de datos: {ex.Message}");
    }
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

        [HttpGet("mostrarAsociadas/{puestoId}")]
        public async Task<ActionResult<IEnumerable<PuestoCategoria>>> GetPuestoCategorias(int puestoId)
        {


            var usuarioLoguedoId = HttpContext.User.Identity.Name;
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rol = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            //return await _context.Categorias.ToListAsync();

            
            var datos = await _context.PuestoCategorias
                .Where(pc => pc.PuestoId == puestoId)
                .Include(pc => pc.Puesto)
                .Include(pc => pc.Categoria)
                .Select(pc => new
                {
                    pc.PuestoCategoriaId,
                    nombrePuesto = pc.Puesto.NombrePuesto,
                    descripcionCategoria = pc.Categoria.Descripcion
                })
                .ToListAsync();



            return Ok(datos);

        }


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