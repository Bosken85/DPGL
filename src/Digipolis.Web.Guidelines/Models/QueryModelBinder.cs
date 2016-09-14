using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Digipolis.Web.Guidelines.Models
{
    public class QueryModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            throw new System.NotImplementedException();
        }
    }
}