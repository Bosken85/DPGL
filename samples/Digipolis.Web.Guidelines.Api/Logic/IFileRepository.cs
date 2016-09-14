using System.Collections.Generic;
using System.IO;
using Digipolis.Web.Guidelines.Api.Models;
using Digipolis.Web.Guidelines.Models;

namespace Digipolis.Web.Guidelines.Api.Logic
{
    public interface IFileLogic
    {
        IEnumerable<FileDto> GetAll(int valueId, Query queryOptions, out int total);
        FileDto GetById(int valueId, int id);
        FileDto Add(int valueId, FileDto value);
        FileDto Update(int valueId, int id, FileDto value);
        void Delete(int valueId, int id);
    }
}