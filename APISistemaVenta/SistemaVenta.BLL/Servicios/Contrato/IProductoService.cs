using SistemaVenta.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios.Contrato
{
    public interface IProductoService
    {
        Task<List<ProductoDTO>> lista();
        Task<ProductoDTO> crear(ProductoDTO modelo);
        Task<bool> editar(ProductoDTO modelo);
        Task<bool> eliminar(int id);
    }
}
