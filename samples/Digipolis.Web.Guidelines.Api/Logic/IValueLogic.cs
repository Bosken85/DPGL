using System.Collections.Generic;
using Digipolis.Web.Guidelines.Api.Models;
using Digipolis.Web.Guidelines.Models;

namespace Digipolis.Web.Guidelines.Api.Logic
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