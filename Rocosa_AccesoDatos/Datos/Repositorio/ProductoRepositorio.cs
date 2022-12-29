using Microsoft.AspNetCore.Mvc.Rendering;
using Rocosa_AccesoDatos.Datos.Repositorio.IRepositorio;
using Rocosa_Modelos;
using Rocosa_Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocosa_AccesoDatos.Datos.Repositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {

        private readonly ApplicationDbContext _db;

        public ProductoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(Producto producto)
        {
            _db.Update(producto);
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropdownList(string obj)
        {
            if (obj == WC.CategoriaNombre)
            {
                return _db.Categoria.Select(c => new SelectListItem
                {
                    Text = c.NombreCategoria,
                    Value = c.Id.ToString()
                });
            }
            if (obj == WC.TipoAplicacionNombre)
            {
                return _db.TipoAplicacion.Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }
            return null;
        }
    }
}
