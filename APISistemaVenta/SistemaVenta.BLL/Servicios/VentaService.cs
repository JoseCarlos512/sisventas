using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class VentaService:IVentaService
    {
        private readonly IVentaRepository _VentaRepository;
        private readonly IGenericRepository<DetalleVenta> _detalleVentaRepositorio;
        private readonly IMapper _mapper;

        public VentaService(IVentaRepository ventaRepository, IGenericRepository<DetalleVenta> detalleVentaRepositorio, IMapper mapper)
        {
            _VentaRepository = ventaRepository;
            _detalleVentaRepositorio = detalleVentaRepositorio;
            _mapper = mapper;
        }

        public async Task<VentaDTO> registrar(VentaDTO modelo)
        {
            try
            {

                var ventaGenerada = await _VentaRepository.Registrar(_mapper.Map<Venta>(modelo));

                if (ventaGenerada.IdVenta == 0) {
                    throw new TaskCanceledException("No se pudo crear");
                }

                return _mapper.Map<VentaDTO>(ventaGenerada);

            } catch
            {
                throw;
            }

        }
        public async Task<List<VentaDTO>> historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin)
        {
            IQueryable<Venta> query = await _VentaRepository.Consultar();
            var ListaResultado = new List<Venta>();

            try
            {

                if (buscarPor == "fecha") {
                    DateTime fech_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-PE"));
                    DateTime fech_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-PE"));

                    ListaResultado = await query.Where(v =>
                        v.FechaRegistro.Value.Date >= fech_inicio.Date &&
                        v.FechaRegistro.Value.Date <= fech_fin.Date
                    ).Include(dv=>dv.DetalleVenta)
                    .ThenInclude(p=>p.IdProductoNavigation)
                    .ToListAsync();
                } else {
                    ListaResultado = await query.Where(v => v.NumeroDocumento == numeroVenta
                    ).Include(dv => dv.DetalleVenta)
                    .ThenInclude(p => p.IdProductoNavigation)
                    .ToListAsync();
                }
            }
            catch
            {
                throw;
            }

            return _mapper.Map<List<VentaDTO>>(ListaResultado);
        }

        public async Task<List<ReporteDTO>> reporte(string fechaInicio, string fechaFin)
        {
            IQueryable<DetalleVenta> query = await _detalleVentaRepositorio.Consultar();
            var ListaResultado = new List<DetalleVenta>();

            try
            {
                DateTime fech_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-PE"));
                DateTime fech_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-PE"));

                ListaResultado = await query
                    .Include(p => p.IdProductoNavigation)
                    .Include(v => v.IdVentaNavigation)
                    .Where(dv =>
                        dv.IdVentaNavigation.FechaRegistro.Value.Date >= fech_inicio.Date &&
                        dv.IdVentaNavigation.FechaRegistro.Value.Date <= fech_fin.Date
                    ).ToListAsync();
            }
            catch
            {
                throw;
            }

            return _mapper.Map<List<ReporteDTO>>(ListaResultado);
        }
    }
}
