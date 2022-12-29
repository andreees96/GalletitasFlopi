using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocosa_AccesoDatos.Datos;
using Rocosa_Modelos;
using System.Data;
using Rocosa_Utilidades;
using Rocosa_AccesoDatos.Datos.Repositorio.IRepositorio;

namespace Rocosa.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoriaController : Controller
    {
        private readonly ICategoriaRepositorio _catRepo;
        public CategoriaController(ICategoriaRepositorio catRepo)
        {
            _catRepo = catRepo;
        }
    
        public IActionResult Index()
        {
            IEnumerable<Categoria> lista = _catRepo.ObtenerTodos();
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
        public IActionResult Crear(Categoria categoria)
        {
            if(ModelState.IsValid)
            {
                _catRepo.Agregar(categoria);
                _catRepo.Grabar();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);          
        }


        //GET EDITAR
        public IActionResult Editar(int? Id)
        {
            if(Id == null || Id == 0) 
            { 
                return NotFound();
            }
            var obj = _catRepo.Obtener(Id.GetValueOrDefault());
            if(obj == null) 
            {
                return NotFound();  
            }

            return View(obj);
        }
        //POST EDITAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _catRepo.Actualizar(categoria);
                _catRepo.Grabar();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }


        //GET ELIMINAR
        public IActionResult Eliminar(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _catRepo.Obtener(Id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        //POST ELIMINAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(Categoria categoria)
        {
            if (categoria == null)
            {
                return NotFound();
            }
            _catRepo.Remover(categoria);
            _catRepo.Grabar();
            return RedirectToAction(nameof(Index));
            return View(categoria);
        }
    }
}
