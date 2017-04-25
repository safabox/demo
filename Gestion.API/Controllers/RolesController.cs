using AutoMapper;
using Gestion.API.Models;
using Gestion.API.Models.Permisos;
using Gestion.API.Models.Roles;
using Gestion.Common.Services;
using Gestion.Common.Utils;
using Gestion.Common.Domain.Seguridad;
using Gestion.Common.Services.Seguridad;
using Gestion.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Thinktecture.IdentityModel.WebApi;
using WebApi.OutputCache.V2;

namespace Gestion.API.Controllers
{
    [Authorize]
    [AutoInvalidateCacheOutput]
    public class RolesController : BaseEntidadApiController<Rol>
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly IRolService service;

        public RolesController(IRolService service, IMapper modelMapper)
            : base(modelMapper)
        {
            this.service = service;
        }

        // GET: api/roles
        [ResourceAuthorize(Resources.RolesActions.Listar, Resources.Roles)]
        [CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = Constants.Cache.ENTITY_DEFAULT_SERVERTIMESPAN, MustRevalidate = true)]
        public IEnumerable<RolListModel> GetAll()
        {
            logger.Debug("GetAll");

            var query = this.service.GetAll();
            var data = query.ToList();
            var result = base.ModelMapper.Map<IEnumerable<RolListModel>>(data);
            return result;
        }

        // GET: api/roles?count=10&page=1&sort=-nombre
        [ResourceAuthorize(Resources.RolesActions.Listar, Resources.Roles)]
        [CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = Constants.Cache.ENTITY_DEFAULT_SERVERTIMESPAN, MustRevalidate = true)]
        [ResponseType(typeof(PagedResult<RolListModel>))]
        public Task<IHttpActionResult> GetList(int page, int count, bool includeDeleted = false, [FromUri] string[] sort = null, string search = null)
        {
            try
            {
                logger.Debug("GetList - page: {0} - count: {1} - sort: {2} - search: {3} - includeDeleted: {4}", page, count, sort, search, includeDeleted);

                var query = this.service.GetAll(includeDeleted);
                query = base.FilterQuery(query, search);
                query = base.SortQuery(query, sort);

                var result = base.GetPagedResult<RolListModel>(query, page, count);

                return Task.FromResult<IHttpActionResult>(Ok(result));
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Task.FromResult<IHttpActionResult>(InternalServerError(ex));
            }
        }

        // GET: api/roles/5
        [ResourceAuthorize(Resources.RolesActions.Listar, Resources.Roles)]
        [CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = Constants.Cache.ENTITY_DEFAULT_SERVERTIMESPAN, MustRevalidate = true)]
        [ResponseType(typeof(RolModel))]
        public async Task<IHttpActionResult> GetById(long id)
        {
            try
            {
                logger.Debug("GetById - id: {0}", id);

                var entity = await this.service.GetAsync(id);
                if (entity != null)
                {
                    var vm = base.ModelMapper.Map<Rol, RolModel>(entity);
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

        // POST: api/roles
        [ResourceAuthorize(Resources.RolesActions.Crear, Resources.Roles)]
        [InvalidateCacheOutput("GetById", typeof(UsuariosController))]
        [InvalidateCacheOutput("GetPermisos", typeof(UsuariosController))]
        [ResponseType(typeof(RolModel))]
        public async Task<IHttpActionResult> Post(PostRolModel model)
        {
            try
            {
                logger.Debug("Post", model);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var entity = base.ModelMapper.Map<Rol>(model);

                var result = this.service.Add(entity);
                logger.Info(result);

                if (!result.Succeeded)
                {
                    return base.GetErrorResult(result);
                }

                var resultSaveChanges = await this.service.SaveChangesAsync(ServiceChangeCommentBuilder.GetCreateComment<Rol>());
                logger.Info(resultSaveChanges);

                if (resultSaveChanges.Succeeded)
                {
                    return CreatedAtRoute("DefaultApi", new { id = result.Result.Id }, base.ModelMapper.Map<RolModel>(result.Result));
                }
                else
                {
                    return base.GetErrorResult(resultSaveChanges);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return InternalServerError(ex);
            }
        }

        // POST: api/roles/5
        [ResourceAuthorize(Resources.RolesActions.Editar, Resources.Roles)]
        [InvalidateCacheOutput("GetById", typeof(UsuariosController))]
        [InvalidateCacheOutput("GetPermisos", typeof(UsuariosController))]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Put(long id, PostRolModel model)
        {
            try
            {
                logger.Debug("Put - Id: {0} - Model: {1}", id, model);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var entity = base.ModelMapper.Map<Rol>(model);

                var result = await this.service.Update(id, entity);
                logger.Info(result);
                if (!result.Succeeded)
                {
                    return base.GetErrorResult(result);
                }

                var resultSaveChanges = await this.service.SaveChangesAsync(ServiceChangeCommentBuilder.GetUpdateComment<Rol>(result.Result));
                logger.Info(resultSaveChanges);

                if (resultSaveChanges.Succeeded)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                else
                {
                    return base.GetErrorResult(resultSaveChanges);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return InternalServerError(ex);
            }
        }

        // POST: api/roles/5/recover
        [ResourceAuthorize(Resources.RolesActions.Eliminar, Resources.Roles)]
        [ResponseType(typeof(void))]
        [InvalidateCacheOutput("GetById", typeof(UsuariosController))]
        [InvalidateCacheOutput("GetPermisos", typeof(UsuariosController))]
        [Route("api/roles/{id}/recover")]
        [HttpPut]
        public async Task<IHttpActionResult> Recover(long id)
        {
            try
            {
                logger.Debug("Recover - Id: {0}", id);

                var result = await this.service.Recover(id);
                logger.Info(result);
                if (!result.Succeeded)
                {
                    return base.GetErrorResult(result);
                }

                var resultSaveChanges = await this.service.SaveChangesAsync(ServiceChangeCommentBuilder.GetRecoverComment<Rol>(result.Result));
                logger.Info(resultSaveChanges);

                if (resultSaveChanges.Succeeded)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                else
                {
                    return base.GetErrorResult(resultSaveChanges);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return InternalServerError(ex);
            }
        }

        // DELETE: api/roles/5
        [ResourceAuthorize(Resources.RolesActions.Eliminar, Resources.Roles)]
        [InvalidateCacheOutput("GetById", typeof(UsuariosController))]
        [InvalidateCacheOutput("GetPermisos", typeof(UsuariosController))]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Delete(long id)
        {
            try
            {
                logger.Debug("Delete - Id: {0}", id);

                var result = await this.service.Delete(id);
                logger.Info(result);
                if (!result.Succeeded)
                {
                    return base.GetErrorResult(result);
                }

                var resultSaveChanges = await this.service.SaveChangesAsync(ServiceChangeCommentBuilder.GetDeleteComment<Rol>(result.Result));
                logger.Info(resultSaveChanges);

                if (resultSaveChanges.Succeeded)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                else
                {
                    return base.GetErrorResult(resultSaveChanges);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return InternalServerError(ex);
            }
        }

        // GET: api/roles/1/permisos
        [ResourceAuthorize(Resources.RolesActions.Listar, Resources.Roles)]
        [CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = Constants.Cache.ENTITY_DEFAULT_SERVERTIMESPAN, MustRevalidate = true)]
        [Route("api/roles/{id}/permisos")]
        public async Task<IEnumerable<PermisoModel>> GetPermisos(long id)
        {
            logger.Debug("GetPermisos - Id: {0}", id);

            var entity = await this.service.GetAsync(id);
            if (entity != null)
            {
                var data = entity.Permisos
                    .OrderBy(o => o.Recurso.Descripcion)
                    .ThenBy(o => o.Descripcion)
                    .ToList();

                var result = base.ModelMapper.Map<IEnumerable<PermisoModel>>(data);
                return result;
            }
            return null;
        }

        // GET: api/roles/1/usuarios
        [ResourceAuthorize(Resources.RolesActions.Listar, Resources.Roles)]
        [CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = Constants.Cache.ENTITY_DEFAULT_SERVERTIMESPAN, MustRevalidate = true)]
        [Route("api/roles/{id}/usuarios")]
        public Task<IEnumerable<RolMembresiaListModel>> GetUsuarios(long id)
        {
            logger.Debug("GetUsuarios - Id: {0}", id);

            var membresias = this.service.GetMembresiasActivas(id);
            if (membresias != null)
            {
                var data = membresias
                    .OrderBy(o => o.Usuario.Apellido)
                    .ThenBy(o => o.Usuario.Nombre)
                    .AsEnumerable();

                var result = base.ModelMapper.Map<IEnumerable<RolMembresiaListModel>>(data);
                return Task.FromResult(result);
            }
            return null;
        }

        protected override IQueryable<Rol> FilterQuery(IQueryable<Rol> query, FilterOption filter)
        {
            return QueryFilterHelper.FilterQueryRol(query, filter);
        }
    }
}