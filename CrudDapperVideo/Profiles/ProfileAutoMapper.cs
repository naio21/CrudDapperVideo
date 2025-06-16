using AutoMapper;

namespace CrudDapperVideo.Profiles
{
    public class ProfileAutoMapper : Profile
    {
        public ProfileAutoMapper() 
        {
            CreateMap<Models.Usuario, Dto.UsuarioListarDto>().ReverseMap();
            CreateMap<Models.Usuario, Dto.UsuarioCriarDto>().ReverseMap();
            CreateMap<Models.Usuario, Dto.UsuarioEditarDto>().ReverseMap();
        }
    }
}
