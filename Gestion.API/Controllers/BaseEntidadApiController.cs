using AutoMapper;
using Gestion.API.Models;
using Gestion.Common.Domain;
using Gestion.Common.Utils;
using System.Linq;
using System.Collections.Generic;

namespace Gestion.API.Controllers
{
    public abstract class BaseEntidadApiController<TEntidad> : BaseApiController where TEntidad : Entidad
    {
        public BaseEntidadApiController(IMapper modelMapper)
            : base(modelMapper)
        {
        }

        protected abstract IQueryable<TEntidad> FilterQuery(IQueryable<TEntidad> query, FilterOption filter);

        #region FilterQuery

        protected IQueryable<TEntidad> FilterQuery(IQueryable<TEntidad> query, string filters)
        {
            var filterOptions = FilterOption.Parse(filters);
            return FilterQuery(query, filterOptions);
        }

        protected IQueryable<TEntidad> FilterQuery(IQueryable<TEntidad> query, FilterOption[] filters)
        {
            var result = query;
            foreach (var filter in filters)
            {
                result = FilterQuery(result, filter);
            }
            return result;
        }

        #endregion

        #region SortQuery

        protected IQueryable<TEntidad> SortQuery(IQueryable<TEntidad> query, string[] options)
        {
            return base.SortQuery<TEntidad, long>(query, options, x => x.Id);
        }

        #endregion

        #region PagedResult

        protected PagedResult<TModel> GetPagedResult<TModel>(IQueryable<TEntidad> query, int page, int count) where TModel : Model
        {
            return base.GetPagedResult<TEntidad, TModel>(query, page, count);
        }

        protected IEnumerable<TEntidad> GetPagedData(IQueryable<TEntidad> query, int page, int count)
        {
            return base.GetPagedData<TEntidad>(query, page, count);
        }

        #endregion
    }
}