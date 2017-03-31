using AutoMapper;
using Gestion.API.Models;
using Gestion.API.Models.Permisos;
using Gestion.Common.Domain.Seguridad;
using Gestion.Common.Utils;
using System.Linq;

namespace Gestion.API.Config.AutoMapperProfiles
{
    public class SeguridadModelProfile : Profile
    {
        public SeguridadModelProfile()
            : base("SeguridadModelProfile")
        {
            //ConfigureRolMapping();
            //ConfigureUsuarioMapping();
            ConfigurePermisoMapping();
        }

        private void ConfigurePermisoMapping()
        {
            CreateMap<Permiso, long>().ConvertUsing(src => src.Id);
            CreateMap<long, Permiso>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x));

            CreateMap<Permiso, PermisoModel>();
        }
    }
}