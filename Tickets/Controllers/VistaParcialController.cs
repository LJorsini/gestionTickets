using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using gestionTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Security.Claims;
using System.IO.Compression;
using gestionTickets.Models.Vistas;
using gestionTickets.ModelsVistas;


namespace gestionTickets.Controllers
{
    [Authorize]
    [Route("api/auth/[controller]")]
    [ApiController]

    public class VistaParcialController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VistaParcialController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("informe")]

        public async Task<ActionResult<IEnumerable<VistaCategorias>>> GetDesarrolladores()
        {
            List<VistaCatPuesto> vistaCatPuestos = new List<VistaCatPuesto>();

            var categorias = _context.PuestoCategorias
                            .Include(t => t.Puesto)
                            .Include(t => t.Categoria)
                            .AsQueryable();

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rol = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            foreach (var categoria in categorias)
            {
                var categoriaMostrar = vistaCatPuestos.FirstOrDefault(c => c.CategoriaId == categoria.CategoriaId);

                if (categoriaMostrar == null)
                {
                     categoriaMostrar = new VistaCatPuesto
                     {
                         CategoriaId = categoria.CategoriaId,
                         /* Descripcion = categoria.Descripcion, */
                         Puestos = new List<VistaPuesto>(),
                    };
                }
            } 
            return Ok();
        }
    }
}