using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rocosa_Modelos
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Nombre del Producto es requerido")]
        public string NombreProducto { get; set; }
        [Required(ErrorMessage ="Descripción Corta es Requerida")]
        public string DescripcionCorta { get; set; }
        [Required(ErrorMessage ="Descripción del Producto es Requerida")]
        public string DescripcionProducto { get; set; }
        [Required(ErrorMessage ="El Precio del Producto es Requerido")]
        [Range(1, double.MaxValue, ErrorMessage ="El Precio debe ser Mayor a Cero")]
        public double Precio { get; set; }
        public string? ImagenUrl { get; set; }

        //Foreing Key
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public virtual Categoria? Categoria { get; set; }
        public int TipoAplicacionId { get; set; }
        [ForeignKey("TipoAplicacionId")]
        public virtual TipoAplicacion? TipoAplicacion { get; set; }
    }
}
