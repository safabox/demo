using AutoMapper;
using Gestion.API.Models.Permisos;
using Gestion.Common.Domain.Seguridad;
using Gestion.Security;
using Gestion.Services.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Thinktecture.IdentityModel.WebApi;



namespace Gestion.API.Controllers
{
    //[Authorize]
    public class PermisosController : BaseApiController
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly IPermisoService service;

        public PermisosController(IPermisoService service, IMapper modelMapper)
            : base(modelMapper)
        {
            this.service = service;
        }

        // GET: api/permisos/
        //[CacheOutputUntilToday(23, 59, 59)]
        //[ResourceAuthorize(Resources.RolesActions.Listar, Resources.Roles)]
        public IEnumerable<PermisoModel> GetAll()
        {
            logger.Debug("GetAll");

            var query = this.service.GetAll();
            return GetSortedList(query);
        }

        // GET: api/permisos/5
        //[ResourceAuthorize(Resources.RolesActions.Listar, Resources.Roles)]
        //[CacheOutputUntilToday(23, 59, 59, MustRevalidate = true)]
        [ResponseType(typeof(PermisoModel))]
        public async Task<IHttpActionResult> GetById(long id)
        {
            try
            {
                logger.Debug("GetById - id: {0}", id);

                var entity = await this.service.GetAsync(id);
                if (entity != null)
                {
                    var vm = base.ModelMapper.Map<Permiso, PermisoModel>(entity);
                    return Ok(vm);
                }

                logger.Info("GetById - id: {0} - Not Found", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return InternalServerError(ex);
            }
        }

        private IEnumerable<PermisoModel> GetSortedList(IQueryable<Permiso> query)
        {
            var data = query
                .OrderBy(o => o.Recurso.Descripcion)
                .ThenBy(o => o.Descripcion)
                .ToList();

            var result = base.ModelMapper.Map<IEnumerable<PermisoModel>>(data);
            return result;
        }
    }
}