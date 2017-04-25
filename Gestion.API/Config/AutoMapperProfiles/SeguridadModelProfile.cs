using AutoMapper;
using Gestion.API.Models.Roles;
using Gestion.API.Models.Permisos;
using Gestion.API.Models.Usuarios;
using Gestion.Common.Domain.Seguridad;



namespace Gestion.API.Config.AutoMapperProfiles
{
    public class SeguridadModelProfile : Profile
    {
        public SeguridadModelProfile()
            : base("SeguridadModelProfile")
        {
            ConfigureRolMapping();
            ConfigureUsuarioMapping();
            ConfigurePermisoMapping();
        }

        private void ConfigurePermisoMapping()
        {
            CreateMap<Permiso, long>().ConvertUsing(src => src.Id);
            CreateMap<long, Permiso>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x));

            CreateMap<Permiso, PermisoModel>();
        }

        private void ConfigureRolMapping()
        {
            CreateMap<Rol, long>()
                .ConvertUsing(src => src.Id);

            CreateMap<Rol, RolModel>();
            CreateMap<Rol, RolListModel>();
            CreateMap<PostRolModel, Rol>();
        }

        private void ConfigureUsuarioMapping()
        {
            CreateMap<Usuario, long>()
                .ConvertUsing(src => src.Id);

            CreateMap<Usuario, UsuarioModel>();

            CreateMap<Usuario, UsuarioListModel>();

            CreateMap<PostUsuarioModel, Usuario>();
            CreateMap<PutUsuarioModel, Usuario>();

            CreateMap<Usuario, UsuarioMiniListModel>();
        }
    }
}