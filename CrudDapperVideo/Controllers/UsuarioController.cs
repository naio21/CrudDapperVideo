using CrudDapperVideo.Dto;
using CrudDapperVideo.Services;
using Microsoft.AspNetCore.Mvc;

namespace CrudDapperVideo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioInterface _usuarioInterface;

        public UsuarioController(IUsuarioInterface usuarioInterface)
        {
            _usuarioInterface = usuarioInterface;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarUsuarios()
        {
            var usuarios = await _usuarioInterface.BuscarUsuarios();
            if (usuarios.Status == false)
            {
                return NotFound(usuarios);
            }
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarUsuarioPorId(int id)
        {
            var usuario = await _usuarioInterface.BuscarUsuarioPorId(id);
            if (usuario.Status == false)
            {
                return NotFound(usuario);
            }
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario(UsuarioCriarDto novo)
        {
            var usuario = await _usuarioInterface.CriarUsuario(novo);
            if (usuario.Status == false)
            {
                return BadRequest(usuario);
            }
            return Ok(usuario);
        }

        [HttpPut]
        public async Task<IActionResult> EditarUsuario(UsuarioEditarDto usuario)
        {
            var resultado = await _usuarioInterface.EditarUsuario(usuario);
            if (resultado.Status == false)
            {
                return BadRequest(resultado);
            }
            return Ok(resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverUsuarioPorId(int id)
        {
            var resultado = await _usuarioInterface.RemoverUsuarioPorId(id);
            if (resultado.Status == false)
            {
                return NotFound(resultado);
            }
            return Ok(resultado);
        }
    }
}