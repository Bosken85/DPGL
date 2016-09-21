using System.Collections.Generic;
using Digipolis.Web.Guidelines.Api.Data.Entiteiten;
using Digipolis.Web.Guidelines.Mvc;

namespace Digipolis.Web.Guidelines.Api.Data
{
    public interface IFileRepository
    {
        IEnumerable<File> GetAll(int valueId, PageFilter queryOptions, out int total);
        File GetById(int valueId, int id);
        File Add(int valueId, File value);
        File Update(int valueId, int id, File value);
        void Delete(int valueId, int id);
    }
}