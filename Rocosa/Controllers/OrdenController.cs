using Microsoft.AspNetCore.Mvc;
using Rocosa_AccesoDatos.Datos.Repositorio.IRepositorio;
using Rocosa_Modelos.ViewModels;

namespace GalletitasFlopi.Controllers
{
    public class OrdenController : Controller
    {
        private readonly IOrdenRepositorio _ordenRepo;
        private readonly IOrdenDetalleRepositorio _ordenDetalleRepo;

        [BindProperty]
        public OrdenVM OrdenVM { get; set; }

        public OrdenController(IOrdenRepositorio ordenRepo, IOrdenDetalleRepositorio ordenDetalleRepo)
        {
            _ordenRepo = ordenRepo;
            _ordenDetalleRepo = ordenDetalleRepo;
        }

        public IActionResult Index()
        {

            return View();
        }

        //public IActionResult Detalle(int id) 
        //{
        //    OrdenVM = new OrdenVM()
        //    {
        //        Orden = _ordenRepo.ObtenerPrimero(o => o.Id == id),
        //        OrdenDetalles = _ordenRepo.ObtenerTodos(d => d.OrdenId == id, incluirPropiedades: "Producto")
        //    };

        //    return View();
        //}

        #region APIs
        [HttpGet]
        public IActionResult ObtenerListaOrdenes() 
        {
            return Json(new { data = _ordenRepo.ObtenerTodos() });
        }
        #endregion

    }
}
