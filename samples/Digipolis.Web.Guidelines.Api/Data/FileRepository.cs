using System.Collections.Generic;
using System.Linq;
using Digipolis.Errors.Exceptions;
using Digipolis.Web.Guidelines.Error;
using Digipolis.Web.Guidelines.Mvc;
using File = Digipolis.Web.Guidelines.Api.Data.Entiteiten.File;

namespace Digipolis.Web.Guidelines.Api.Data
{
    public class FileRepository : IFileRepository
    {
        private static readonly List<File> Files = new List<File>(new[]
        {
            new File(1, 1),
            new File(1,2),
            new File(1,3),
            new File(1,4),
            new File(1,5)
        });

        public FileRepository()
        {
        }


        public IEnumerable<File> GetAll(int valueId, PageOptions queryOptions, out int total)
        {
            var result = Files.Where(x => x.ValueId == valueId);
            total = result.Count();
            return result.Skip((queryOptions.Page - 1) * queryOptions.PageSize).Take(queryOptions.PageSize);
        }

        public File GetById(int valueId, int id)
        {
            if (Files.All(x => x.ValueId == valueId && x.Id != id))
                throw new NotFoundException();
            return Files.FirstOrDefault(x => x.ValueId == valueId && x.Id == id);
        }

        public File Add(int valueId, File value)
        {
            value.Id = Files.Max(x => x.Id) + 1;
            Files.Add(value);
            return value;
        }

        public File Update(int valueId, int id, File value)
        {
            //Mimic DB exception thrown by no record found
            if (Files.All(x => x.ValueId == valueId && x.Id != id))
                throw new NotFoundException();

            var dbValue = Files.Find(x => x.ValueId == valueId && x.Id == id);
            dbValue.Stream = value.Stream;
            return dbValue;
        }

        public void Delete(int valueId, int id)
        {
            //Mimic DB exception thrown by no record found
            if (Files.All(x => x.ValueId == valueId && x.Id != id))
                throw new NotFoundException();

            Files.Remove(Files.Find(x => x.ValueId == valueId && x.Id == id));
        }
    }
}
