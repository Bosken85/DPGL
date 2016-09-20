using System;
using Microsoft.AspNetCore.Http;

namespace Digipolis.Web.Guidelines.Error
{
    public class ErrorManager : IErrorManager
    {
        public Error Error { get; }

        public ErrorManager(IHttpContextAccessor contextAccessor)
        {
            Error = new Error();
        }
    }
}
