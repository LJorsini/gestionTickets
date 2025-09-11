using System.Security.Claims;
using gestionTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gestionTickets.Controllers
{
    [Authorize]
    [Route("api/auth/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientesController(
                ApplicationDbContext context,
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                RoleManager<IdentityRole> rolManager,
                IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            
        }
       /*  [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetCliente()
        {
            return await _context.Clientes.ToListAsync();
        }  */  ////cambio editar

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VistaCliente>>> GetCliente()
        {
            List<VistaCliente> vistaClientes = new List<VistaCliente>();

            var clientes = await _context.Clientes.ToListAsync();

            foreach (var cliente in clientes)
            {
                var mostrarCliente = new VistaCliente
                {
                    ClineteId = cliente.ClienteId,
                    Nombre = cliente.Nombre,
                    Email = cliente.Email,
                    Telefono = cliente.Telefono,
                    Cuit = cliente.Cuit,
                    Observaciones = cliente.Observaciones,

                };
                
                vistaClientes.Add(mostrarCliente);
            }
            return vistaClientes.ToList();
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cuitExiste = await _context.Clientes.AnyAsync(c => c.Cuit == cliente.Cuit && c.ClienteId != cliente.ClienteId);

            if (!cuitExiste)
            {
                
                var usuarioCliente = new ApplicationUser
                {
                    UserName = cliente.Email,
                    Email = cliente.Email,
                    NombreCompleto = cliente.Nombre,

                };

                var resultado = await _userManager.CreateAsync(usuarioCliente, "Ezpeleta2025");
                if (resultado.Succeeded)
                {
                    await _userManager.AddToRoleAsync(usuarioCliente, "CLIENTE");

                    cliente.UsuarioClienteID = usuarioCliente.Id;

                    _context.Clientes.Add(cliente);
                    await _context.SaveChangesAsync();
                }
                
                
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
        
        

            


        
    


    