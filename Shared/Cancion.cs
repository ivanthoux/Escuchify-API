using System.ComponentModel.DataAnnotations;

namespace Shared
{
    public class Cancion
    {
        // Identificador único de la entidad (por convención se usa como PK en EF Core)
        public int Id { get; set; }

        // Nombre de la canción.
        // [Required] indica que el valor es obligatorio; si está vacío o es null,
        // la validación de modelo fallará y se devolverá el ErrorMessage.
        [Required(ErrorMessage = "El nombre de la canción es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        // Duración de la canción en segundos.
        // [Range(1, int.MaxValue)] obliga a que el valor sea >= 1 (no puede ser 0 ni negativo).
        [Range(1, int.MaxValue, ErrorMessage = "La duración debe ser mayor a 0")]
        public int Duracion { get; set; } // En segundos

        // --- Claves Foráneas ---

        // DiscoId: propiedad que almacena la llave foránea hacia la tabla/entidad Disco.
        // Necesaria para persistir la relación en la base de datos.
        public int DiscoId { get; set; } // Llave foránea necesaria para la BD

        // Objeto de navegación: referencia opcional a la entidad Disco.
        // Al ser nullable (Disco?), indica que la relación puede no estar cargada.
        // EF Core la poblá cuando se haga Include(...) o carga perezosa (si está habilitada).
        public Disco? Disco { get; set; } // Objeto de navegación
    }
}