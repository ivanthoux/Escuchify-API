using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared
{
    public class Disco
    {
        // Identificador primario (convención: Id será la PK en EF Core)
        public int Id { get; set; }

        // Validación: nombre obligatorio. El mensaje se usa por ejemplo en MVC/Blazor al validar el modelo.
        [Required(ErrorMessage = "El nombre del disco es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        // Año de lanzamiento (int simple; no valida rango aquí)
        public int AnioLanzamiento { get; set; }

        // Duración total del disco (se usa int; aclarar unidad: segundos/minutos)
        // GOTCHA: usar int es simple pero ambiguo; considerar TimeSpan para mayor claridad.
        public int DuracionTotal { get; set; } // En segundos o minutos, según tu lógica

        // Tipo de disco (por ejemplo "LP", "EP", "Single"). Inicializado a cadena vacía para evitar null.
        public string TipoDisco { get; set; } = string.Empty;

        // --- Claves Foráneas (Relaciones) ---

        // 1. Relación con Artista (Muchos Discos tienen 1 Artista)
        // ArtistaId actúa como llave foránea en EF Core.
        public int ArtistaId { get; set; } // La llave foránea

        // Navegación hacia el artista. Es nullable (puede no estar cargado).
        // GOTCHA: si usas "Artista" sin cargar la relación, será null hasta que EF la incluya.
        public Artista? Artista { get; set; } // La navegación (el objeto completo)

        // 2. Relación con Canciones (Un Disco tiene muchas Canciones)
        // JsonIgnore evita que la lista de canciones se serialice al convertir a JSON (evita ciclos o payloads grandes).
        [JsonIgnore]
        public List<Cancion>? Canciones { get; set; }

        // Propiedad calculada: devuelve la cantidad de canciones.
        // Se usa un campo de respaldo para permitir la serialización/deserialización.
        // [NotMapped] evita que EF Core intente buscar esta columna en la BD.
        private int _cantidadCanciones;
        [NotMapped]
        public int CantidadCanciones
        {
            get => Canciones != null ? Canciones.Count : _cantidadCanciones;
            set => _cantidadCanciones = value;
        }
    }
}