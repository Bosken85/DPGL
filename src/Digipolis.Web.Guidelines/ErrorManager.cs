using Digipolis.Errors;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Digipolis.Web.Guidelines
{
    /// <summary>
    /// The ErrorManager can be injected into the entire stack of the application.
    /// It is instanciated once per request and will keep track of all the error messages
    /// added throughout all the layers of the application
    /// </summary>
    public interface IErrorManager
    {
        Error Error { get; }

        bool HasErrors { get; }
    }

    public class ErrorManager : IErrorManager
    {
        private const string ItemId = "context:items:errorManager";

        public Error Error { get; }

        public bool HasErrors => Error.Messages.Any();

        public ErrorManager(IHttpContextAccessor contextAccessor)
        {
            Error = new Error();
            contextAccessor.HttpContext.Items.Add(ItemId, Error);
        }
    }
}
