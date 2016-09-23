using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Internal;

namespace Digipolis.Web.Guidelines.Mvc
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class ExcludeSwaggerResonseAttribute :  Attribute
    {
        public int[] HttpCodes { get; set; }

        public ExcludeSwaggerResonseAttribute(params int[] httpCodes)
        {
            HttpCodes = httpCodes;
        }
    }
}
