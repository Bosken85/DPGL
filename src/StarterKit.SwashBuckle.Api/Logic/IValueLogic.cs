using System.Collections.Generic;
using Digipolis.Web.Guidelines.Models;
using Digipolis.Web.Guidelines.Paging;
using StarterKit.SwashBuckle.Api.Data.Entiteiten;
using StarterKit.SwashBuckle.Api.Models;

namespace StarterKit.SwashBuckle.Api.Logic
{
    public interface IValueLogic
    {
        IEnumerable<ValueDto> GetAll(Query queryOptions, out int total);
        ValueDto GetById(int id);
        ValueDto Add(ValueDto value);
        ValueDto Update(int id, ValueDto value);
        void Delete(int id);
    }
}