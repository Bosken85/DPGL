using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Digipolis.Web.Guidelines.Error
{
    public abstract class ExceptionHandler : IExceptionHandler
    {
        private readonly Dictionary<Type, Action<Error, Exception>> _errorMappings;

        protected ExceptionHandler()
        {
            _errorMappings = new Dictionary<Type, Action<Error, Exception>>();
            _errorMappings.Add(typeof(Exception), CreateDefaultMap);
            Configure();
        }

        public abstract void Configure();

        public abstract void CreateDefaultMap(Error error, Exception exception);

        public void CreateMap<TException>(Action<Error, TException> configError) where TException : Exception
        {
            Action<Error,Exception> action = (x, y) => configError(x, (TException)y);
            _errorMappings.Add(typeof(TException), action);
        }

        protected Error Map(Exception exception)
        {
            var type = exception.GetType();
            var error = new Error();

            if (_errorMappings.ContainsKey(type))
                _errorMappings[type](error, exception);
            else if (_errorMappings.ContainsKey(typeof(Exception)))
                _errorMappings[typeof(Exception)](error, exception);
            else
                error = null;

            return error;
        }

        public virtual Error Resolve(Exception exception)
        {
            var error = Map(exception);

            var validationException = exception as ValidationException;
            if (validationException != null)
                error.ExtraParameters = validationException.ModelState.ToDictionary(x=> x.Key, x=> (object)x.Value);

            return error;
        }
    }
}