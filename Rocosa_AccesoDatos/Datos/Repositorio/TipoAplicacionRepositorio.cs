using Rocosa_AccesoDatos.Datos.Repositorio.IRepositorio;
using Rocosa_Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocosa_AccesoDatos.Datos.Repositorio
{
    public class TipoAplicacionRepositorio : Repositorio<TipoAplicacion>, ITipoAplicacionRepositorio
    {
        private readonly ApplicationDbContext _db;

        public TipoAplicacionRepositorio(ApplicationDbContext db): base(db)
        {
            _db = db;
        }   

        public void Actualizar(TipoAplicacion tipoAplicacion)
        {
            var tipoAnterior = _db.TipoAplicacion.FirstOrDefault(c => c.Id == tipoAplicacion.Id);
            if(tipoAnterior != null)
            {
                tipoAnterior.Nombre = tipoAplicacion.Nombre;
            }
        }
    }
}
