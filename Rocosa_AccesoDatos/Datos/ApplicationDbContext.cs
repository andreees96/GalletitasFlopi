using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rocosa_Modelos;

namespace Rocosa_AccesoDatos.Datos
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }
        
        //agregar modelos
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<TipoAplicacion> TipoAplicacion { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<UsuarioAplicacion> UsuarioAplicacion { get; set; }
        public DbSet<Orden> Orden { get; set; }
        public DbSet<OrdenDetalle> OrdenDetalle { get; set;}
    }
}
