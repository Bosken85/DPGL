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

        public ValidationException(Dictionary<string, IEnumerable<string>> modelState = null, string message = null, Exception exception = null)
            : base(message, exception)
        {
            ModelState = modelState ?? new Dictionary<string, IEnumerable<string>>();
        }
    }
}