using System;
using System.Collections.Generic;
using System.Linq;
using Digipolis.Errors.Exceptions;
using Digipolis.Web.Guidelines.Api.Data.Entiteiten;
using Digipolis.Web.Guidelines.Helpers;
using Digipolis.Web.Guidelines.Models;
using ValueType = Digipolis.Web.Guidelines.Api.Data.Entiteiten.ValueType;

namespace Digipolis.Web.Guidelines.Api.Data
{
    public class ValueRepository : IValueRepository
    {
        private readonly IErrorManager _errorManager;

        private static readonly List<Value> Values = new List<Value>(new[]
        {
            new Value {Id = 1, Name = "value 1", ValueType = ValueType.Store, CreationDate = DateTime.UtcNow},
            new Value {Id = 2, Name = "value 2", ValueType = ValueType.Unknown, CreationDate = DateTime.UtcNow},
            new Value {Id = 3, Name = "value 3", ValueType = ValueType.Store, CreationDate = DateTime.UtcNow},
            new Value {Id = 4, Name = "value 4", ValueType = ValueType.Unknown, CreationDate = DateTime.UtcNow},
            new Value {Id = 5, Name = "value 5", ValueType = ValueType.Store, CreationDate = DateTime.UtcNow}
        });

        private static IQueryable<Value> Table => Values.AsQueryable();

        public ValueRepository(IErrorManager errorManager)
        {
            _errorManager = errorManager;
        }

        public IEnumerable<Value> GetAll(Query queryOptions, out int total)
        {
            total = Table.Count();
            var query = Table.OrderByQuery(queryOptions).Skip((queryOptions.Page - 1)*queryOptions.PageSize).Take(queryOptions.PageSize);
            return query;
        }

        public Value GetById(int id)
        {
            try
            {
                if(Table.All(x => x.Id != id))
                    throw new NotFoundException();
                return Table.FirstOrDefault(x => x.Id == id);
            }
            catch (NotFoundException)
            {
                _errorManager.Error.AddMessage(nameof(id), "No value found for this id");
                throw;
            }
        }

        public Value Add(Value value)
        {
            try
            {
                //Mimic DB exception thrown by Unique Constraint
                if (Table.Any(x => x.Name.Equals(value.Name, StringComparison.CurrentCultureIgnoreCase)))
                    throw new InvalidOperationException();

                value.Id = Values.Max(x => x.Id) + 1;
                value.CreationDate = DateTime.UtcNow;
                Values.Add(value);
                return value;
            }
            catch (InvalidOperationException)
            {
                _errorManager.Error.AddMessage(nameof(value.Name), "Name has to be unique between all values.");
                throw;
            }
        }

        public Value Update(int id, Value value)
        {
            try
            {
                //Mimic DB exception thrown by no record found
                if (Table.All(x => x.Id != id))
                    throw new NotFoundException();

                //Mimic DB exception thrown by Unique Constraint
                if (Table.Any(x => x.Name.Equals(value.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != id))
                    throw new InvalidOperationException();

                var dbValue = Values.Find(x => x.Id == id);
                dbValue.Name = value.Name;
                return dbValue;
            }
            catch (NotFoundException)
            {
                _errorManager.Error.AddMessage(nameof(id), "No value was found by that Id");
                throw;
            }
            catch (InvalidOperationException)
            {
                _errorManager.Error.AddMessage(nameof(value.Name), "Name has to be unique between all values.");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                //Mimic DB exception thrown by no record found
                if (Table.All(x => x.Id != id))
                    throw new NotFoundException();

                Values.Remove(Values.Find(x => x.Id == id));
            }
            catch (NotFoundException)
            {
                _errorManager.Error.AddMessage(nameof(id), "No value was found by that Id");
                throw;
            }
        }
    }
}