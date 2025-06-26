using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using gestionTickets.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace gestionTickets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuariosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }


        [HttpGet("usuario-actual")]
        public async Task<ActionResult<object>> GetUsuarioActual()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var usuario = await _userManager.FindByIdAsync(userId);

            if (usuario == null)
                return NotFound();

            var usuarioLogueado = new
            {
                usuario.Email,
                usuario.NombreCompleto,


            };

            return Ok(usuarioLogueado);
        }

        [HttpPost("editar-usuario")]

        [HttpPost]
        public async Task<ActionResult<Cliente>> EditarUsuario(string email, string nombreCompleto, string password)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var usuario = await _userManager.FindByIdAsync(userId);

            if (usuario == null)
                return NotFound();

            usuario.Email = email;
            usuario.NombreCompleto = nombreCompleto;
            usuario.PasswordHash = password;

            var result = await _userManager.UpdateAsync(usuario);

            if (!result.Succeeded)
                return BadRequest(result.Errors);
            {


                return Ok();
            }


        }
    }
}