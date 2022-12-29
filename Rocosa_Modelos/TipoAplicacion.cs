using System.ComponentModel.DataAnnotations;

namespace Rocosa_Modelos
{
    public class TipoAplicacion
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="El nombre del tipo del aplicación es obligatorio")]
        public string Nombre { get; set; }
    }
}
