using Newtonsoft.Json;

namespace Gestion.API.Models
{
    public abstract class Model
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}