using Gestion.Common;
using Gestion.Common.Data;
using Gestion.Common.Domain;
using Gestion.Common.Domain.Seguridad;
using Gestion.Security;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion.Services
{
    public abstract class BaseEntidadModificableService<TEntidad, TContext> : BaseService<TContext>
           where TContext : IEntidadModificableDataContext
           where TEntidad : EntidadModificable
    {
        protected abstract string NotFoundMessage { get; }
        protected abstract DbSet<TEntidad> EntitySource { get; }

        public BaseEntidadModificableService(TContext context, ISecurityContext securityContext)
            : base(context, securityContext)
        {
        }

        protected Usuario GetCurrentUser()
        {
            var userName = GetPrincipalUserName();
            return this.context.GetUsuarioByNombreUsuario(userName);
        }

        public virtual Task<TEntidad> GetAsync(long id)
        {
            return this.EntitySource.FindAsync(id);
        }

        public virtual IQueryable<TEntidad> GetAll(bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return this.EntitySource;
            }
            return this.EntitySource.Where(x => x.Estado != EstadosEntidad.Eliminada);
        }

        public OperationResult<TEntidad> Add(TEntidad entity)
        {
            try
            {
                string[] validationErrors;
                if (this.ValidateAdd(entity, out validationErrors))
                {
                    var usuario = this.GetCurrentUser();
                    var entityDB = ExecuteAdd(entity);

                    entityDB.FechaCreacion = DateTime.UtcNow;
                    entityDB.UsuarioCreacion = usuario;

                    return OperationResult<TEntidad>.Success(entityDB);
                }
                return OperationResult<TEntidad>.Failed(validationErrors);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return OperationResult<TEntidad>.Failed(ex);
            }
        }

        public async Task<OperationResult<TEntidad>> Update(long id, TEntidad entity)
        {
            try
            {
                var usuario = this.GetCurrentUser();

                string[] validationErrors;
                var entityDB = await this.GetAsync(id);
                if (entityDB != null)
                {
                    if (this.ValidateUpdate(id, entity, entityDB, out validationErrors))
                    {
                        UpdateAuditFields(entityDB, usuario);
                        this.UpdateFields(entityDB, entity);
                        ExecutePostUpdate(entityDB, entity, usuario);

                        return OperationResult<TEntidad>.Success(entityDB);
                    }
                    return OperationResult<TEntidad>.Failed(validationErrors);
                }
                return OperationResult<TEntidad>.Failed(this.NotFoundMessage);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return OperationResult<TEntidad>.Failed(ex);
            }
        }

        protected virtual void ExecutePostUpdate(TEntidad entityDB, TEntidad entity, Usuario usuario)
        {
        }

        protected void UpdateAuditFields(TEntidad entityDB)
        {
            var usuario = this.GetCurrentUser();
            UpdateAuditFields(entityDB, usuario);
        }

        protected void UpdateAuditFields(TEntidad entityDB, Usuario usuario)
        {
            entityDB.FechaUltimaModificacion = DateTime.UtcNow;
            entityDB.UsuarioUltimaModificacion = GetCurrentUser();
        }

        public virtual async Task<OperationResult<TEntidad>> Delete(long id)
        {
            try
            {
                var entityDB = await this.EntitySource.FindAsync(id);

                if (entityDB != null)
                {
                    var usuario = GetCurrentUser();

                    entityDB.Eliminar(usuario);
                    return OperationResult<TEntidad>.Success(entityDB);
                }
                return OperationResult<TEntidad>.Failed(this.NotFoundMessage);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return OperationResult<TEntidad>.Failed(ex);
            }
        }

        public virtual async Task<OperationResult<TEntidad>> Recover(long id)
        {
            try
            {
                var entityDB = await this.EntitySource.FindAsync(id);

                if (entityDB != null)
                {
                    var usuario = GetCurrentUser();

                    entityDB.Recuperar(usuario);
                    return OperationResult<TEntidad>.Success(entityDB);
                }
                return OperationResult<TEntidad>.Failed(this.NotFoundMessage);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return OperationResult<TEntidad>.Failed(ex);
            }
        }

        protected virtual TEntidad ExecuteAdd(TEntidad entity)
        {
            return this.EntitySource.Add(entity);
        }

        protected virtual bool ValidateAdd(TEntidad entity, out string[] validationErrors)
        {
            validationErrors = null;
            return true;
        }

        protected virtual void UpdateFields(TEntidad entityDB, TEntidad entity)
        {
        }

        protected virtual bool ValidateUpdate(long id, TEntidad entity, TEntidad entityDB, out string[] validationErrors)
        {
            validationErrors = null;
            return true;
        }
    }
}
