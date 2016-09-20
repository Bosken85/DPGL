using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using Digipolis.Web.Guidelines.Error;

namespace Digipolis.Web.Guidelines.Api.Configuration
{
    public class ApiExceptionHandler : ExceptionHandler
    {
        public override void Configure()
        {
            CreateDefaultMap((x, ex) =>
            {
                x.Status = (int)HttpStatusCode.InternalServerError;
                x.Title = "Woeps";
            });

            CreateMap<NotImplementedException>((x, ex) =>
            {
                x.Status = (int) HttpStatusCode.BadRequest;
                x.Title = "This function cannot be called on the api";
            });

            CreateMap<Error.ValidationException>((x, ex) =>
            {
                x.Status = (int)HttpStatusCode.BadRequest;
                x.Title = "the model has some validation issues";
                x.ExtraParameters = ex.ModelState.ToDictionary(ms=> ms.Key, ms=> (object)ms.Value);
            });
        }
    }
}
