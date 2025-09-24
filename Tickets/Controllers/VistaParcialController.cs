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

        /* [HttpGet("informeCerrados")]

        public async Task<ActionResult<IEnumerable<VistaTicket>>> GetInformesCerrados()
        {
            List<VistaDesarrollador> vistaCerrados = new List<VistaDesarrollador>();

            

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rol = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;


            var tickets = _context.Tickets.Where(t => t.UsuarioClienteID == userId);

            var desarrolladores = _context.Desarrolladores
                                .Include(t => t.Tickets)
                                .AsQueryable();
                                




            foreach (var desarrollador in desarrolladores)
            {
                var vistaDesarrollador = new VistaDesarrollador
                {
                    DesarrolladorId = desarrollador.DesarrolladorId,
                    NombreCompleto = desarrollador.NombreCompleto,
                    TicketsCerrados = new List<VistaTicket>()
                };

                vistaCerrados.Add(vistaDesarrollador);
            }

            var ticketsmostrar = new VistaTicket
            {
                 
            };

                return Ok();


        } */

        [HttpGet("informe")]
        public async Task<ActionResult<IEnumerable<VistaPuestoCategoria>>> GetDesarrolladores()
        {
            List<VistaPuestoCategoria> vistaCatPuestos = new List<VistaPuestoCategoria>();

            var categorias = _context.PuestoCategorias

                            .Include(t => t.Categoria)
                            .Include(t => t.Puesto)
                             .ThenInclude(p => p.Desarrolladores)
                            .AsQueryable();

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rol = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            foreach (var categoria in categorias)
            {
                var categoriaMostrar = vistaCatPuestos.FirstOrDefault(c => c.CategoriaId == categoria.CategoriaId);

                if (categoriaMostrar == null)
                {
                    categoriaMostrar = new VistaPuestoCategoria
                    {
                        CategoriaId = categoria.CategoriaId,
                        NombreCategoria = categoria.Categoria.Descripcion,
                        Puestos = new List<VistaPuesto>()
                    };
                    vistaCatPuestos.Add(categoriaMostrar);
                }

                var puestoMostrar = new VistaPuesto
                {
                    PuestoId = categoria.PuestoId,
                    NombrePuesto = categoria.Puesto.NombrePuesto,
                    Desarrollador = new List<VistaDesarrollador>(),
                };

                categoriaMostrar.Puestos.Add(puestoMostrar);

                foreach (var dev in categoria.Puesto.Desarrolladores)
                {
                    var desarrolladorMostrar = new VistaDesarrollador
                    {
                        DesarrolladorId = dev.DesarrolladorId,
                        NombreCompleto = dev.NombreCompleto,


                    };

                    puestoMostrar.Desarrollador.Add(desarrolladorMostrar);
                }
            }
            return vistaCatPuestos.ToList();
        }
    }
}