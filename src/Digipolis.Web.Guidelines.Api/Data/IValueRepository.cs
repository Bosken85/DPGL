using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Digipolis.Errors;
using Digipolis.Web.Guidelines.Models;
using Digipolis.Web.Guidelines.Paging;
using StarterKit.SwashBuckle.Api.Data.Entiteiten;
using StarterKit.SwashBuckle.Api.Models;

namespace StarterKit.SwashBuckle.Api.Data
{
    public interface IValueRepository
    {
        IEnumerable<Value> GetAll(Query queryOptions, out int total);
        Value GetById(int id);
        Value Add(Value value);
        Value Update(int id, Value value);
        void Delete(int id);
    }
}
