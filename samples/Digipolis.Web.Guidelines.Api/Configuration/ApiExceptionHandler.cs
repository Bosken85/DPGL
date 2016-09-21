using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Digipolis.Errors.Exceptions;
using Digipolis.Web.Guidelines.Error;
using ValidationException = Digipolis.Web.Guidelines.Error.ValidationException;

namespace Digipolis.Web.Guidelines.Api.Configuration
{
    public class ApiExceptionHandler : ExceptionHandler
    {
        public override void Configure()
        {
            CreateMap<ValidationException>((x, ex) =>
            {
                x.Status = (int)HttpStatusCode.BadRequest;
                x.Title = "the model has some validation issues";
                x.ExtraParameters = ex.ModelState.ToDictionary(ms => ms.Key, ms => (object)ms.Value);
            });

            CreateMap<NotImplementedException>((x, ex) =>
            {
                x.Status = (int) HttpStatusCode.BadRequest;
                x.Title = "This function cannot be called on the api";
            });

            CreateMap<UnauthorizedAccessException>((x, ex) =>
            {
                x.Status = (int)HttpStatusCode.Unauthorized;
                x.Title = "Access restricted";
            });

            CreateMap<HttpRequestException>((x, ex) =>
            {
                x.Status = (int)HttpStatusCode.NotFound;
                x.Title = "The resource you requested does not exist";
            });

        }

        public override void CreateDefaultMap(Error.Error error, Exception exception)
        {
            error.Status = (int)HttpStatusCode.InternalServerError;
            error.Title = "Woeps";
        }
    }
}
