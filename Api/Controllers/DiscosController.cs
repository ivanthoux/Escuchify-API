using Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DiscosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/discos
        [HttpGet]
        public async Task<ActionResult<List<Disco>>> Get()
        {
            // Traemos también el dato del Artista dueño del disco y las canciones para el conteo
            return await _context.Discos
                                 .Include(d => d.Artista)
                                 .Include(d => d.Canciones)
                                 .ToListAsync();
        }

        // GET ESPECIAL: api/discos/por-artista/5
        // Reemplaza a: ObtenerDiscos(int idArtista)
        [HttpGet("por-artista/{artistaId}")]
        public async Task<ActionResult<List<Disco>>> GetPorArtista(int artistaId)
        {
            var discos = await _context.Discos
                                       .Where(d => d.ArtistaId == artistaId)
                                       .Include(d => d.Canciones) // Para que calcule la cantidad de canciones
                                       .ToListAsync();
            return discos;
        }

        // POST: api/discos
        // Reemplaza a: GuardarDisco(...)
        [HttpPost]
        public async Task<ActionResult<Disco>> Post(Disco disco)
        {
            // Validamos que el Artista exista antes de guardar el disco
            var existeArtista = await _context.Artistas.AnyAsync(a => a.Id == disco.ArtistaId);
            if (!existeArtista) return BadRequest($"No existe un artista con ID {disco.ArtistaId}");

            _context.Discos.Add(disco);
            await _context.SaveChangesAsync();
            return Ok(disco);
        }

        // PUT: api/discos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Disco disco)
        {
            if (id != disco.Id) return BadRequest();
            _context.Entry(disco).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/discos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var disco = await _context.Discos.FindAsync(id);
            if (disco == null) return NotFound();

            _context.Discos.Remove(disco);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}