using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocosa_AccesoDatos.Datos;
using Rocosa_AccesoDatos.Datos.Repositorio.IRepositorio;
using Rocosa_Modelos;
using Rocosa_Utilidades;

namespace Rocosa.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class TipoAplicacionController : Controller
    {
        private readonly ITipoAplicacionRepositorio _tipoRepo;
        public TipoAplicacionController(ITipoAplicacionRepositorio tipoRepo)
        {
            _tipoRepo = tipoRepo;
        }
    
        public IActionResult Index()
        {
            IEnumerable<TipoAplicacion> lista = _tipoRepo.ObtenerTodos();
            return View(lista);
        }

        //GET CREAR
        public IActionResult Crear()
        {
            return View();
        }
        //POST CREAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(TipoAplicacion tipoAplicacion)
        {
            if(ModelState.IsValid)
            {
                _tipoRepo.Agregar(tipoAplicacion);
                _tipoRepo.Grabar();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoAplicacion);          
        }


        //GET EDITAR
        public IActionResult Editar(int? Id)
        {
            if(Id == null || Id == 0) 
            { 
                return NotFound();
            }
            var obj = _tipoRepo.Obtener(Id.GetValueOrDefault());
            if(obj == null) 
            {
                return NotFound();  
            }

            return View(obj);
        }
        //POST EDITAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(TipoAplicacion tipoAplicacion)
        {
            if (ModelState.IsValid)
            {
                _tipoRepo.Actualizar(tipoAplicacion);
                _tipoRepo.Grabar();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoAplicacion);
        }


        //GET ELIMINAR
        public IActionResult Eliminar(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _tipoRepo.Obtener(Id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        //POST ELIMINAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(TipoAplicacion tipoAplicacion)
        {
            if (tipoAplicacion == null)
            {
                return NotFound();
            }
            _tipoRepo.Remover(tipoAplicacion);
            _tipoRepo.Grabar();
            return RedirectToAction(nameof(Index));
            return View(tipoAplicacion);
        }
    }
}
