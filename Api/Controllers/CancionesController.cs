using Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CancionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CancionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/canciones
        [HttpGet]
        public async Task<ActionResult<List<Cancion>>> Get()
        {
            return await _context.Canciones.Include(c => c.Disco).ToListAsync();
        }

        // GET ESPECIAL: api/canciones/por-disco/10
        // Reemplaza a: ObtenerCanciones(int idDisco)
        [HttpGet("por-disco/{discoId}")]
        public async Task<ActionResult<List<Cancion>>> GetPorDisco(int discoId)
        {
            return await _context.Canciones
                                 .Where(c => c.DiscoId == discoId)
                                 .ToListAsync();
        }

        // POST: api/canciones
        // Reemplaza a: GuardarCancion(...)
        [HttpPost]
        public async Task<ActionResult<Cancion>> Post(Cancion cancion)
        {
            // Validar que el disco exista
            var existeDisco = await _context.Discos.AnyAsync(d => d.Id == cancion.DiscoId);
            if (!existeDisco) return BadRequest($"No existe el disco con ID {cancion.DiscoId}");

            _context.Canciones.Add(cancion);
            await _context.SaveChangesAsync();
            return Ok(cancion);
        }

        // PUT: api/canciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Cancion cancion)
        {
            if (id != cancion.Id) return BadRequest();
            _context.Entry(cancion).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/canciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cancion = await _context.Canciones.FindAsync(id);
            if (cancion == null) return NotFound();

            _context.Canciones.Remove(cancion);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}