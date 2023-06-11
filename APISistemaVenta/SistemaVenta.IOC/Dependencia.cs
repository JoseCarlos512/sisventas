using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SistemaVenta.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DAL.Repositorios;
using SistemaVenta.Utility;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.BLL.Servicios;

namespace SistemaVenta.IOC
{
    public static class Dependencia
    { 
        // Metodo de extencion
        public static void inyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbventaContext>(Options =>
            {
                // Regresando la cadena de conexion a DBVentaContext (Se quito pero se agrega de manera segura)
                Options.UseSqlServer(configuration.GetConnectionString("cadenaSQL"));
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IVentaRepository, VentaRepository>();

            // Se agrego la dependencia de automaper de clases (modelos) -> dto y dto -> clases (modelos)
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddScoped<IRolService,RolService> ();
            services.AddScoped<IUsuarioService,UsuarioService> ();
            services.AddScoped<ICategoriaService, CategoriaService> ();
            services.AddScoped<IProductoService,ProductoService> ();
            services.AddScoped<IVentaService,VentaService> ();
            services.AddScoped<IDashBoardService,DashBoardService> ();
            services.AddScoped<IMenuService,MenuService> ();
        }

    }
}
