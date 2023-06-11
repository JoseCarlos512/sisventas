using SistemaVenta.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios.Contrato
{
    public interface IVentaService
    {

        Task<VentaDTO> registrar(VentaDTO modelo);
        Task<List<VentaDTO>> historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin);
        Task<List<ReporteDTO>> reporte(string fechaInicio, string fechaFin);
    }
}
