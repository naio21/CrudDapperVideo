using CrudDapperVideo.Dto;
using CrudDapperVideo.Models;

namespace CrudDapperVideo.Services
{
    public interface IUsuarioInterface
    {
        Task<ResponseModel<List<UsuarioListarDto>>> BuscarUsuarios();
        Task<ResponseModel<UsuarioListarDto>> BuscarUsuarioPorId(int id);
        Task<ResponseModel<List<UsuarioListarDto>>> CriarUsuario(UsuarioCriarDto usuario);
        Task<ResponseModel<List<UsuarioListarDto>>> EditarUsuario(UsuarioEditarDto usuario);
    }
}
