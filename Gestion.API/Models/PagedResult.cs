using System.Collections.Generic;

namespace Gestion.API.Models
{
    public class PagedResult<T> where T : Model
    {
        public IEnumerable<T> Data { get; set; }
        public long Total { get; set; }
    }
}