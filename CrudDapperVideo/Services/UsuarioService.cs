using AutoMapper;
using CrudDapperVideo.Dto;
using CrudDapperVideo.Models;
using Dapper;
using System.Data.SqlClient;

namespace CrudDapperVideo.Services
{
    public class UsuarioService : IUsuarioInterface
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private SqlConnection _sqlConn;

        public UsuarioService(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ResponseModel<List<UsuarioListarDto>>> BuscarUsuarios()
        {
            ResponseModel<List<UsuarioListarDto>> response = new();
            await Conectar();
            IEnumerable<Usuario> usuariosBanco = await _sqlConn.QueryAsync<Usuario>("SELECT * FROM Usuarios");
            await Desconectar();
            if (!usuariosBanco.Any())
            {
                response.Status = false;
                response.Mensagem = "Nenhum usuário encontrado.";
                return response;
            }
            response.Dados = _mapper.Map<List<UsuarioListarDto>>(usuariosBanco);
            response.Mensagem = "Usuários encontrados com sucesso.";
            return response;
        }

        public async Task<ResponseModel<UsuarioListarDto>> BuscarUsuarioPorId(int id)
        {
            ResponseModel<UsuarioListarDto> response = new();
            await Conectar();
            Usuario? usuarioBanco = await _sqlConn.QueryFirstOrDefaultAsync<Usuario>("SELECT * FROM Usuarios WHERE Id = @Id", new { Id = id });
            await Desconectar();
            if (usuarioBanco == null)
            {
                response.Status = false;
                response.Mensagem = "Nenhum usuário encontrado.";
                return response;
            }
            response.Dados = _mapper.Map<UsuarioListarDto>(usuarioBanco);
            response.Mensagem = "Usuários encontrados com sucesso.";
            return response;
        }

        public async Task<ResponseModel<List<UsuarioListarDto>>> CriarUsuario(UsuarioCriarDto usuario)
        {
            ResponseModel<List<UsuarioListarDto>> response = new();
            await Conectar();
            Usuario usuarioBanco = _mapper.Map<Usuario>(usuario);
            string query = @"INSERT INTO Usuarios (NomeCompleto, Email, Cargo, Salario, CPF, Situacao, Senha) 
                             VALUES (@NomeCompleto, @Email, @Cargo, @Salario, @CPF, @Situacao, @Senha)";
            int linhasAfetadas = await _sqlConn.ExecuteAsync(query, usuarioBanco);
            await Desconectar();
            if (linhasAfetadas == 0)
            {
                response.Status = false;
                response.Mensagem = "Erro ao criar usuário.";
                return response;
            }
            response.Dados = await BuscarUsuarios().ContinueWith(t => t.Result.Dados);
            response.Mensagem = "Usuário criado com sucesso.";
            return response;
        }

        public async Task<ResponseModel<List<UsuarioListarDto>>> EditarUsuario(UsuarioEditarDto usuario)
        {
            ResponseModel<List<UsuarioListarDto>> response = new();
            await Conectar();
            int linhasAfetadas = await _sqlConn.ExecuteAsync(
                                    @"UPDATE Usuarios SET
                                        NomeCompleto = @NomeCompleto,      
                                        Email = @Email,
                                        Cargo = @Cargo,
                                        Salario = @Salario,
                                        CPF = @CPF,
                                        Situacao = @Situacao
                                    WHERE Id = @Id", 
                                    usuario
                                );
            await Desconectar();
            if (linhasAfetadas == 0)
            {
                response.Status = false;
                response.Mensagem = "Erro ao editar o usuário.";
                return response;
            }
            response.Dados = await BuscarUsuarios().ContinueWith(t => t.Result.Dados);
            response.Mensagem = "Usuário alterado com sucesso.";
            return response;
        }

        public async Task<ResponseModel<List<UsuarioListarDto>>> RemoverUsuarioPorId(int id)
        {
            ResponseModel<List<UsuarioListarDto>> response = new();
            await Conectar();
            int linhasAfetadas = await _sqlConn.ExecuteAsync(
                                            "DELETE FROM Usuarios WHERE Id = @Id", 
                                            new { Id = id }
                                        );
            await Desconectar();
            if (linhasAfetadas == 0)
            {
                response.Status = false;
                response.Mensagem = "Erro ao excluir o usuário.";
                return response;
            }
            response.Dados = await BuscarUsuarios().ContinueWith(t => t.Result.Dados);
            response.Mensagem = "Usuário excluído com sucesso.";
            return response;
        }

        private async Task Conectar()
        {
            _sqlConn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await _sqlConn.OpenAsync();
        }

        private async Task Desconectar()
        {
            if (_sqlConn != null)
            {
                if (_sqlConn.State == System.Data.ConnectionState.Open)
                {
                    await _sqlConn.CloseAsync();
                }
                await _sqlConn.DisposeAsync();
            }
        }
    }
}
