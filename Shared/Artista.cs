using System.Collections.Generic; // Colecciones (List<T>)
using System.ComponentModel.DataAnnotations; // Atributos de validación como [Required]
using System.ComponentModel.DataAnnotations.Schema; // Added for [NotMapped]
using System.Text.Json.Serialization; // Para atributos de serialización JSON como [JsonIgnore]

namespace Shared
{
    // Representa un artista en el dominio/aplicación
    public class Artista
    {
        // EF Core por convención considera una propiedad llamada "Id" como la clave primaria
        public int Id { get; set; }

        // [Required] indica que esta propiedad es obligatoria en validaciones de modelo.
        // ErrorMessage se usa para mostrar un mensaje amigable si falta el valor.
        // Se inicializa a string.Empty para evitar valores nulos (mejor para bindings/serialización).
        [Required(ErrorMessage = "El nombre artístico es obligatorio")]
        public string NombreArtistico { get; set; } = string.Empty;

        // Igual que arriba: propiedad obligatoria para el nombre real del artista.
        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        public string NombreCompleto { get; set; } = string.Empty;

        // Año de inicio de la carrera. Al ser int no permite null (valor por defecto 0).
        public int AnioInicio { get; set; }

        // Nacionalidad del artista. Inicializada a cadena vacía para evitar nulls.
        public string Nacionalidad { get; set; } = string.Empty;

        // Discográfica asociada. También inicializada a cadena vacía.
        public string Discografica { get; set; } = string.Empty;

        // Relación: un artista puede tener muchos discos.
        // Se marca con [JsonIgnore] para evitar bucles infinitos cuando se serializa a JSON:
        // sin esto, al serializar Artista -> Discos -> cada Disco podría referenciar al Artista de nuevo.
        [JsonIgnore]
        public List<Disco>? Discos { get; set; } // Puede ser null si no se cargaron los discos

        // Propiedad calculada (read-only). No se mapea como columna en la BD por defecto.
        // Se usa un campo de respaldo para permitir la serialización/deserialización.
        // [NotMapped] evita que EF Core intente buscar esta columna en la BD.
        private int _cantidadDiscos;
        [NotMapped]
        public int CantidadDiscos
        {
            get => Discos != null ? Discos.Count : _cantidadDiscos;
            set => _cantidadDiscos = value;
        }
    }
}