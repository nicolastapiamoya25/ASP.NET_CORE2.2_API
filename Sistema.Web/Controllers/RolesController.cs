using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Datos;
using Sistema.Entidades.Usuarios;
using Sistema.Web.Models.Usuarios;

namespace Sistema.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public RolesController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Roles/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<RolViewModel>> Listar()
        {
            var rol = await _context.Roles.ToListAsync();

            return rol.Select(r => new RolViewModel
            {
                idrol = r.idrol,
                nombre = r.nombre,
                descripcion = r.descripcion,
                condicion = r.condicion
            });
        }

        // GET: api/Roles/Select
        [HttpGet("[action]")]
        public async Task<IEnumerable<SelectViewModel>> Select()
        {
            var rol = await _context.Roles.Where(r => r.condicion == true).ToListAsync();

            return rol.Select(r => new SelectViewModel
            {
                idrol = r.idrol,
                nombre = r.nombre,
            });
        }


        // GET: api/Roles/Mostrar/5
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Mostrar([FromRoute]int id)
        {
            var rol = await _context.Roles.FindAsync(id);

            if (rol == null)
            {
                return NotFound();
            }

            return Ok(new RolViewModel
            {
                idrol = rol.idrol,
                nombre = rol.nombre,
                descripcion = rol.descripcion,
                condicion = rol.condicion
            });
        }

        // PUT: api/Roles/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.idrol <= 0)
            {
                return BadRequest();
            }

            var rol = await _context.Roles.FirstOrDefaultAsync(r => r.idrol == model.idrol);

            if (rol == null)
            {
                return NotFound();
            }

            rol.nombre = model.nombre;
            rol.descripcion = model.descripcion;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //guardar Exception
                return BadRequest();
            }

            return Ok();
        }

        // POST: api/Roles/Crear
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Rol rol = new Rol
            {
                nombre = model.nombre,
                descripcion = model.descripcion,
                condicion = true
            };
            _context.Roles.Add(rol);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok();
        }

        // PUT: api/Roles/Desactivar/5
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var rol = await _context.Roles.FirstOrDefaultAsync(r => r.idrol == id);

            if (rol == null)
            {
                return NotFound();
            }

            rol.condicion = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //guardar Exception
                return BadRequest();
            }

            return Ok();
        }

        // PUT: api/Roles/Activar/5
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var rol = await _context.Roles.FirstOrDefaultAsync(r => r.idrol == id);

            if (rol == null)
            {
                return NotFound();
            }

            rol.condicion = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //guardar Exception
                return BadRequest();
            }

            return Ok();
        }

        private bool RolExists(int id)
        {
            return _context.Roles.Any(e => e.idrol == id);
        }
    }
}
