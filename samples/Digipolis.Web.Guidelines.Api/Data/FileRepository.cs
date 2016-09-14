using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Digipolis.Errors.Exceptions;
using Digipolis.Web.Guidelines.Api.Data.Entiteiten;
using Digipolis.Web.Guidelines.Models;
using File = Digipolis.Web.Guidelines.Api.Data.Entiteiten.File;

namespace Digipolis.Web.Guidelines.Api.Data
{
    public class FileRepository : IFileRepository
    {
        private readonly IErrorManager _errorManager;

        private static readonly List<File> Files = new List<File>(new[]
        {
            new File(1, 1),
            new File(1,2),
            new File(1,3),
            new File(1,4),
            new File(1,5)
        });

        public FileRepository(IErrorManager errorManager)
        {
            _errorManager = errorManager;
        }


        public IEnumerable<File> GetAll(int valueId, Query queryOptions, out int total)
        {
            var result = Files.Where(x => x.ValueId == valueId);
            total = result.Count();
            return result.Skip((queryOptions.Page - 1) * queryOptions.PageSize).Take(queryOptions.PageSize);
        }

        public File GetById(int valueId, int id)
        {
            try
            {
                if (Files.All(x => x.ValueId == valueId && x.Id != id))
                    throw new NotFoundException();
                return Files.FirstOrDefault(x => x.ValueId == valueId && x.Id == id);
            }
            catch (NotFoundException)
            {
                _errorManager.Error.AddMessage(nameof(id), "No value found for this id");
                throw;
            }
        }

        public File Add(int valueId, File value)
        {
            value.Id = Files.Max(x => x.Id) + 1;
            Files.Add(value);
            return value;
        }

        public File Update(int valueId, int id, File value)
        {
            try
            {
                //Mimic DB exception thrown by no record found
                if (Files.All(x => x.ValueId == valueId && x.Id != id))
                    throw new NotFoundException();

                var dbValue = Files.Find(x => x.ValueId == valueId && x.Id == id);
                dbValue.Stream = value.Stream;
                return dbValue;
            }
            catch (NotFoundException)
            {
                _errorManager.Error.AddMessage(nameof(id), "No value was found by that Id");
                throw;
            }
        }

        public void Delete(int valueId, int id)
        {
            try
            {
                //Mimic DB exception thrown by no record found
                if (Files.All(x => x.ValueId == valueId && x.Id != id))
                    throw new NotFoundException();

                Files.Remove(Files.Find(x => x.ValueId == valueId && x.Id == id));
            }
            catch (NotFoundException)
            {
                _errorManager.Error.AddMessage(nameof(id), "No value was found by that Id");
                throw;
            }
        }
    }
}
