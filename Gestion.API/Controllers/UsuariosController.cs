using AutoMapper;
using Gestion.API.Models;
using Gestion.API.Models.Usuarios;
using Gestion.Common.Services.Seguridad;
using Gestion.Common.Utils;
using Gestion.Common.Domain.Seguridad;
using Gestion.Common.Services;
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
    [RoutePrefix("api/usuarios")]
    public class UsuariosController : BaseEntidadApiController<Usuario>
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly IUsuarioService service;

        public UsuariosController(IUsuarioService service, IMapper modelMapper)
            : base(modelMapper)
        {
            this.service = service;
        }

        // GET: api/usuarios
        [CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = Constants.Cache.ENTITY_DEFAULT_SERVERTIMESPAN, MustRevalidate = true)]
        public IEnumerable<UsuarioMiniListModel> GetAll()
        {
            logger.Debug("GetAll");

            var query = this.service.GetAll(false);

            var vm = base.ModelMapper.Map<IEnumerable<UsuarioMiniListModel>>(query);
            return vm;
        }

        // GET: api/usuarios?count=10&page=1&sort=-nombre
        [CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = Constants.Cache.ENTITY_DEFAULT_SERVERTIMESPAN, MustRevalidate = true)]
        [ResourceAuthorize(Resources.UsuariosActions.Listar, Resources.Usuarios)]
        [ResponseType(typeof(PagedResult<UsuarioListModel>))]
        public Task<IHttpActionResult> GetList(int page, int count, bool includeDeleted = false, [FromUri] string[] sort = null, string search = null)
        {
            try
            {
                logger.Debug("GetList - page: {0} - count: {1} - sort: {2} - search: {3} - includeDeleted: {4}", page, count, sort, search, includeDeleted);

                var query = this.service.GetAll(includeDeleted);
                query = base.FilterQuery(query, search);
                query = base.SortQuery(query, sort);

                var result = base.GetPagedResult<UsuarioListModel>(query, page, count);

                return Task.FromResult<IHttpActionResult>(Ok(result));
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Task.FromResult<IHttpActionResult>(InternalServerError(ex));
            }
        }

        // GET: api/usuarios/5
        [ResourceAuthorize(Resources.UsuariosActions.Listar, Resources.Usuarios)]
        [ResponseType(typeof(UsuarioModel))]
        [CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = Constants.Cache.ENTITY_DEFAULT_SERVERTIMESPAN, MustRevalidate = true)]
        public async Task<IHttpActionResult> GetById(long id)
        {
            try
            {
                logger.Debug("GetById - id: {0}", id);

                var entity = await this.service.GetAsync(id);
                if (entity != null)
                {
                    var vm = base.ModelMapper.Map<Usuario, UsuarioModel>(entity);
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

        // POST: api/usuarios
        [ResourceAuthorize(Resources.UsuariosActions.Crear, Resources.Usuarios)]
        [ResponseType(typeof(UsuarioModel))]
        public async Task<IHttpActionResult> Post(PostUsuarioModel model)
        {
            try
            {
                logger.Debug("Post", model);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var entity = base.ModelMapper.Map<Usuario>(model);

                var result = this.service.Add(entity);
                logger.Info(result);

                if (!result.Succeeded)
                {
                    return base.GetErrorResult(result);
                }

                var resultSaveChanges = await this.service.SaveChangesAsync(ServiceChangeCommentBuilder.GetCreateComment<Usuario>());
                logger.Info(resultSaveChanges);

                if (resultSaveChanges.Succeeded)
                {
                    return CreatedAtRoute("DefaultApi", new { id = result.Result.Id }, base.ModelMapper.Map<UsuarioModel>(result.Result));
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

        // POST: api/usuarios/5
        [ResourceAuthorize(Resources.UsuariosActions.Editar, Resources.Usuarios)]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Put(long id, PutUsuarioModel model)
        {
            try
            {
                logger.Debug("Put - Id: {0} - Model: {1}", id, model);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var entity = base.ModelMapper.Map<Usuario>(model);

                var result = await this.service.Update(id, entity);
                logger.Info(result);
                if (!result.Succeeded)
                {
                    return base.GetErrorResult(result);
                }

                var resultSaveChanges = await this.service.SaveChangesAsync(ServiceChangeCommentBuilder.GetUpdateComment<Usuario>(result.Result));
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

        // POST: api/usuarios/5/recover
        [ResourceAuthorize(Resources.UsuariosActions.Eliminar, Resources.Usuarios)]
        [ResponseType(typeof(void))]
        [Route("{id}/recover")]
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

                var resultSaveChanges = await this.service.SaveChangesAsync(ServiceChangeCommentBuilder.GetRecoverComment<Usuario>(result.Result));
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

        // DELETE: api/usuarios/5
        [ResourceAuthorize(Resources.UsuariosActions.Eliminar, Resources.Usuarios)]
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

                var resultSaveChanges = await this.service.SaveChangesAsync(ServiceChangeCommentBuilder.GetDeleteComment<Usuario>(result.Result));
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

        protected override IQueryable<Usuario> FilterQuery(IQueryable<Usuario> query, FilterOption filter)
        {
            return QueryFilterHelper.FilterQueryUsuario(query, filter);
        }
    }
}