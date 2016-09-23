using System;
using Digipolis.Web.Guidelines.Error;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Digipolis.Web.Guidelines
{
    public class ApiExtensionOptions
    {
        public bool EnableVersioning { get; set; }

        public bool EnableGlobalErrorHandling { get; set; }

        [JsonIgnore]
        public IExceptionHandler ExceptionHandler { get; set; }

        public int PageSize { get; set; }
    }
}