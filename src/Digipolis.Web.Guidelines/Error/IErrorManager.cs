namespace Digipolis.Web.Guidelines.Error
{
    /// <summary>
    /// The ErrorManager can be injected into the entire stack of the application.
    /// It is instanciated once per request and will keep track of all the error messages
    /// added throughout all the layers of the application
    /// </summary>
    public interface IErrorManager
    {
        Error Error { get; }
    }
}