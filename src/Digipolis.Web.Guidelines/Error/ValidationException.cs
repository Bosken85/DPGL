using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Digipolis.Web.Guidelines.Error
{
    public class ValidationException : Exception
    {
        public Dictionary<string, IEnumerable<string>> ModelState { get; private set; }

        public ValidationException(ModelStateDictionary modelState, string message = null, Exception exception = null)
            : base(message, exception)
        {
            ModelState = modelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage));
        }
    }
}