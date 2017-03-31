using AutoMapper;
using Gestion.API.Models;
using Gestion.Common;
using Gestion.Common.Utils;
using Gestion.Common.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;

namespace Gestion.API.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        protected IMapper ModelMapper { get; private set; }

        public BaseApiController(IMapper modelMapper)
        {
            this.ModelMapper = modelMapper;
        }

        protected IHttpActionResult GetErrorResult(OperationResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        protected IQueryable<TSource> SortQuery<TSource, TKey>(IQueryable<TSource> query, string[] options, Expression<Func<TSource, TKey>> defaultOrder) where TSource : class
        {
            var sortingOptions = SortingOption.Parse(options);
            if (sortingOptions.Length > 0)
            {
                return query.OrderByProperties(sortingOptions);
            }
            return query.OrderBy(defaultOrder);
        }

        protected PagedResult<TModel> GetPagedResult<TSource, TModel>(IQueryable<TSource> query, int page, int count)
            where TModel : Model
            where TSource : class
        {

            var data = query
                .Skip(count * (page - 1))
                .Take(count)
                .ToList();

            return new PagedResult<TModel>()
            {
                Data = Mapper.Map<IEnumerable<TModel>>(data),
                Total = query.Count()
            };
        }

        [AllowAnonymous]
        [AcceptVerbs("OPTIONS")]
        public virtual IHttpActionResult Preflight()
        {
            return Ok();
        }

    }
}