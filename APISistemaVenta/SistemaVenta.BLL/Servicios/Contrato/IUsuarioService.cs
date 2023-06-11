using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.DTO;


namespace SistemaVenta.BLL.Servicios.Contrato
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> lista();
        Task<SessionDTO> validarCredenciales(string correo, string clave);
        Task<UsuarioDTO> crear(UsuarioDTO modelo);
        Task<bool> editar(UsuarioDTO modelo);
        Task<bool> eliminar(int id);

    }
}
