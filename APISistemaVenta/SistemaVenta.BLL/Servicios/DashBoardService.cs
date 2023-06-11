using AutoMapper;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios
{
    internal class DashBoardService:IDashBoardService
    {
        private readonly IVentaRepository _VentaRepository;
        private readonly IGenericRepository<Producto> _productoRepositorio;
        private readonly IMapper _mapper;

        public DashBoardService(IVentaRepository ventaRepository, IGenericRepository<Producto> productoRepositorio, IMapper mapper)
        {
            _VentaRepository = ventaRepository;
            _productoRepositorio = productoRepositorio;
            _mapper = mapper;
        }

        private IQueryable<Venta> retornarVentas(IQueryable<Venta> tableVenta, int restarCantidadDias) {

            DateTime? ultimaFecha = tableVenta.OrderByDescending(v => v.FechaRegistro).Select(v => v.FechaRegistro).First();

            ultimaFecha = ultimaFecha.Value.AddDays(restarCantidadDias);

            return tableVenta.Where(v => v.FechaRegistro.Value.Date >= ultimaFecha.Value.Date);

        }

        public async Task<int> totalVentasUltimaSemana() {
            int total = 0;
            IQueryable<Venta> _ventaQuery = await _VentaRepository.Consultar();

            if (_ventaQuery.Count()>0) {
                var tableVenta = retornarVentas(_ventaQuery, -7);
                total = tableVenta.Count();
            }

            return total;
        }

        public async Task<string> totalIngresosUltimaSemana() {
            decimal resultado = 0;

            IQueryable<Venta> _ventaQuery = await _VentaRepository.Consultar();

            if (_ventaQuery.Count() > 0)
            {
                var tableVenta = retornarVentas(_ventaQuery, -7);
                resultado = tableVenta.Select(v=>v.Total).Sum(v=>v.Value);
            }

            return Convert.ToString(resultado, new CultureInfo("es-PE"));
        }

        public async Task<int> totalProductos() {

            IQueryable<Producto> _productoQuery = await _productoRepositorio.Consultar();

            int total = _productoQuery.Count;
            return total;
        }

        public async Task<Dictionary<string, int>> ventasUltimaSemana() {

            Dictionary<string, int> resultado = new Dictionary<string, int>();

            IQueryable<Venta> _ventaQuery = await _VentaRepository.Consultar();

            if (_ventaQuery.Count() > 0) {

                var tableVenta = retornarVentas(_ventaQuery, -7);

                resultado = tableVenta
                    .GroupBy(v => v.FechaRegistro.Value.Date).OrderBy(g => g.Key) // Agrupa por el campo fecharegistro y order by por fecharegistro
                    .Select(dv => new { fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() }) // Selecciona 2 campos de la tabla
                    .ToDictionary(keySelector: r => r.fecha, elementSelector: r => r.total); //Parsea resultado a formato dictionary
            }

            return resultado;
        }

        public async Task<DashBoardDTO> Resumen()
        {

            DashBoardDTO vmDashBoardDTO = new DashBoardDTO(); 
            try {
                vmDashBoardDTO.TotalVentas = await totalVentasUltimaSemana();
                vmDashBoardDTO.TotalIngresos = await totalIngresosUltimaSemana();
                vmDashBoardDTO.totalProductos = await totalProductos();

                List<VentasSemanaDTO> listaVentaSemana = new List<VentasSemanaDTO>();

                foreach(KeyValuePair<string, int> item in await ventasUltimaSemana()) {

                    listaVentaSemana.Add(new VentasSemanaDTO() {
                        Fecha = item.Key,
                        Total = item.Value
                    });
                }

                vmDashBoardDTO.ventasUltimaSemana = listaVentaSemana;

            } catch {
                throw;
            }

            return vmDashBoardDTO;
        }

    }
}
