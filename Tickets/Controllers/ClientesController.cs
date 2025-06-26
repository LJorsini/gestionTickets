using gestionTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gestionTickets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetCliente()
        {
            return await _context.Clientes.ToListAsync();
        }   ////cambio editar

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            var cuitExiste = await _context.Clientes.AnyAsync(c => c.Cuit == cliente.Cuit && c.ClienteId != cliente.ClienteId);

            if (!cuitExiste)
            {
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest("El cuit ya está en uso por otro cliente.");
            }




            return Ok();
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<Cliente>> PutCliente(int id, Cliente cliente)
        {
            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }

        //habilitar categoria
        [HttpPut("activar/{id}")]
        public async Task<IActionResult> ActivarCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }


            cliente.Eliminado = false;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //deshabilitar categoria
        [HttpPut("desactivar/{id}")]
        public async Task<IActionResult> DesactivarCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            cliente.Eliminado = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    
}
        
        

            


        
    


    