using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Rocosa_AccesoDatos.Datos;
using Rocosa_AccesoDatos.Datos.Repositorio.IRepositorio;
using Rocosa_Modelos;
using Rocosa_Modelos.ViewModels;
using Rocosa_Utilidades;
using System.Security.Claims;
using System.Text;

namespace Rocosa.Controllers
{
    [Authorize]
    public class CarroController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IEmailSender _emailSender;
        private readonly IProductoRepositorio _productoRepo;
        private readonly IUsuarioAplicacionRepositorio _usuarioRepo;
        private readonly IOrdenRepositorio _ordenRepo;
        private readonly IOrdenDetalleRepositorio _ordenDetalleRepo;

        [BindProperty]
        public ProductoUsuarioVM productoUsuarioVM { get; set; }

        public CarroController(IWebHostEnvironment webHostEnviroment, IEmailSender emailSender,
                               IProductoRepositorio productoRepo,
                               IUsuarioAplicacionRepositorio usuarioRepo,
                               IOrdenRepositorio ordenRepo,
                               IOrdenDetalleRepositorio ordenDetalleRepo)
        {
            _productoRepo = productoRepo;
            _usuarioRepo = usuarioRepo;
            _ordenRepo = ordenRepo;
            _ordenDetalleRepo = ordenDetalleRepo;
            _webHostEnviroment = webHostEnviroment;
            _emailSender = emailSender;
        }

        //GET INDEX
        public IActionResult Index()
        {
            List<CarroCompra> carroCompraList = new List<CarroCompra>();

            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null && 
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroCompraList = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }

            List<int> prodEnCarro = carroCompraList.Select(i => i.ProductoId).ToList();
            //IEnumerable<Producto> prodList = _db.Producto.Where(p => prodEnCarro.Contains(p.Id));
            IEnumerable<Producto> prodList = _productoRepo.ObtenerTodos(p => prodEnCarro.Contains(p.Id));

            return View(prodList);
        }

        //POST INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Resumen)); 
        }

        public IActionResult Resumen()
        {
            //traer usuario conectado
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<CarroCompra> carroCompraList = new List<CarroCompra>();

            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroCompraList = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }

            List<int> prodEnCarro = carroCompraList.Select(i => i.ProductoId).ToList();
            //IEnumerable<Producto> prodList = _db.Producto.Where(p => prodEnCarro.Contains(p.Id));
            IEnumerable<Producto> prodList = _productoRepo.ObtenerTodos(p => prodEnCarro.Contains(p.Id));

            productoUsuarioVM = new ProductoUsuarioVM()
            {
                //UsuarioAplicacion = _db.UsuarioAplicacion.FirstOrDefault(u => u.Id == claim.Value),
                UsuarioAplicacion = _usuarioRepo.ObtenerPrimero(u => u.Id == claim.Value),
                ProductoLista= prodList.ToList()
            };
            return View(productoUsuarioVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Resumen")]
        public async Task<IActionResult> ResumenPost(ProductoUsuarioVM productoUsuarioVM)
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);

            var rutaTemplate = _webHostEnviroment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                                +"templates"+Path.DirectorySeparatorChar.ToString()
                                + "PlantillaOrden.html";

            var subject = "Nueva Orden";
            string HtmlBody = "";

            using (StreamReader sr = System.IO.File.OpenText(rutaTemplate))
            {
                HtmlBody= sr.ReadToEnd();
            }

        

            StringBuilder productoListaSB = new StringBuilder();

            foreach(var prod in productoUsuarioVM.ProductoLista)
            {
                productoListaSB.Append($" - Nombre: {prod.NombreProducto} <span style='font-size:14px;'> (ID: {prod.Id})</span><br />");
            }
            string messageBody = string.Format(HtmlBody,
                                        productoUsuarioVM.UsuarioAplicacion.NombreCompleto,
                                        productoUsuarioVM.UsuarioAplicacion.Email,
                                        productoUsuarioVM.UsuarioAplicacion.PhoneNumber,
                                        productoListaSB.ToString());

            await _emailSender.SendEmailAsync(WC.EmailAdmin, subject, messageBody);

            //grabar orden y detalle en bd
            Orden orden = new Orden()
            {
                UsuarioAplicacionId = claim.Value,
                NombreCompleto = productoUsuarioVM.UsuarioAplicacion.NombreCompleto,
                Email = productoUsuarioVM.UsuarioAplicacion.Email,
                Telefono = productoUsuarioVM.UsuarioAplicacion.PhoneNumber,
                FechaOrden = DateTime.Now
            };

            _ordenRepo.Agregar(orden);
            _ordenRepo.Grabar();

            foreach (var prod in productoUsuarioVM.ProductoLista)
            {
                OrdenDetalle ordenDetalle = new OrdenDetalle()
                {
                    OrdenId = orden.Id,
                    ProductoId = prod.Id
                };
                _ordenDetalleRepo.Agregar(ordenDetalle);
            }
            _ordenDetalleRepo.Grabar();

            return RedirectToAction(nameof(Confirmacion));
        }

        public IActionResult Confirmacion()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult Remover(int Id)
        {
            List<CarroCompra> carroCompraList = new List<CarroCompra>();

            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroCompraList = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }

            carroCompraList.Remove(carroCompraList.FirstOrDefault(p => p.ProductoId == Id));
            HttpContext.Session.Set(WC.SessionCarroCompras, carroCompraList);

            return RedirectToAction(nameof(Index));
        }

    }
}
