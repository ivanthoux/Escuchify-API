using Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArtistasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/artistas
        // Reemplaza a: ObtenerArtistas()
        [HttpGet]
        public async Task<ActionResult<List<Artista>>> Get()
        {
            // Include(a => a.Discos) es opcional, úsalo si quieres que traiga los discos al pedir el artista
            return await _context.Artistas.Include(a => a.Discos).ToListAsync();
        }

        // GET: api/artistas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Artista>> Get(int id)
        {
            var artista = await _context.Artistas
                                        .Include(a => a.Discos)
                                        .FirstOrDefaultAsync(a => a.Id == id);

            if (artista == null) return NotFound("Artista no encontrado");
            return artista;
        }

        // POST: api/artistas
        // Reemplaza a: GuardarArtista(...)
        [HttpPost]
        public async Task<ActionResult<Artista>> Post(Artista artista)
        {
            try 
            {
                // EF Core ignora el ID que envíes y genera uno nuevo automático
                _context.Artistas.Add(artista);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { id = artista.Id }, artista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno al guardar el artista: {ex.Message} {ex.InnerException?.Message}");
            }
        }

        // PUT: api/artistas/5
        // Reemplaza a: ActualizarArtista(...)
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Artista artista)
        {
            if (id != artista.Id) return BadRequest("El ID de la URL no coincide con el del cuerpo");

            _context.Entry(artista).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Artistas.Any(a => a.Id == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // DELETE: api/artistas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var artista = await _context.Artistas.FindAsync(id);
            if (artista == null) return NotFound();

            _context.Artistas.Remove(artista);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}