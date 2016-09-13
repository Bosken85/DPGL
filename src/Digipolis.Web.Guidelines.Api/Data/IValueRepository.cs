﻿using System.Collections.Generic;
using Digipolis.Web.Guidelines.Api.Data.Entiteiten;
using Digipolis.Web.Guidelines.Models;

namespace Digipolis.Web.Guidelines.Api.Data
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
