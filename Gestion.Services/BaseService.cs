using Gestion.Common;
using Gestion.Common.Data;
using Gestion.Security;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Gestion.Services
{
    public abstract class BaseService<TContext> where TContext : IUnitOfWork
    {
        protected NLog.Logger logger;

        protected readonly TContext context;
        protected readonly ISecurityContext securityContext;

        public BaseService(TContext context, ISecurityContext securityContext)
        {
            logger = NLog.LogManager.GetLogger(this.GetType().Name);

            this.context = context;
            this.securityContext = securityContext;
        }

        protected IPrincipal GetPrincipal()
        {
            return this.securityContext.GetPrincipal();
        }

        protected string GetPrincipalUserName()
        {
            return this.GetPrincipal().Identity.Name;
        }

        public Task<OperationResult<int>> SaveChangesAsync(string comments = null)
        {
            return SaveChangesAsync(comments, null);
        }

        public async Task<OperationResult<int>> SaveChangesAsync(string comments = null, string userName = null)
        {
            try
            {
                logger.Debug("SaveChangesAsync");

                if (string.IsNullOrEmpty(userName))
                {
                    userName = GetPrincipalUserName();
                }

                var result = await this.context.SaveChangesAsync(userName, comments);

                this.ExecutePostCommitTasks();

                return OperationResult<int>.Success(result);
            }
            catch (DbUpdateException ex)
            {
                logger.Info(ex, "SaveChangesAsync");

                return OperationResult<int>.Failed("Error al guardar cambios en base de datos");
            }
            catch (DbEntityValidationException ex)
            {
                logger.Info(ex, "SaveChangesAsync");

                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage).ToArray();

                return OperationResult<int>.Failed(errorMessages);
            }
            catch (Exception ex)
            {
                logger.Info(ex, "SaveChangesAsync");
                return OperationResult<int>.Failed(ex);
            }
        }

        protected virtual void ExecutePostCommitTasks()
        {
        }
    }
}
