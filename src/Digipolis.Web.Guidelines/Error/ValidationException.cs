using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Digipolis.Web.Guidelines.Error
{
    public class ValidationException : Exception
    {
        public Dictionary<string, IEnumerable<string>> ModelState { get; private set; } = new Dictionary<string, IEnumerable<string>>();

        public ValidationException(ModelStateDictionary modelState, string message = null, Exception exception = null)
            : base(message, exception)
        {
            if ( modelState != null ) ModelState = modelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage));
            AddMessage(message);
        }

        public ValidationException(string message, Exception exception = null) : base(message, exception)
        {
            AddMessage(message);
        }

        public void AddMessage(string message)
        {
            if ( !String.IsNullOrWhiteSpace(message) )
                AddMessages(String.Empty, new string[] { message });
        }

        public void AddMessage(string key, string message)
        {
            if ( !String.IsNullOrWhiteSpace(message))
                AddMessages(key, new string[] { message });
        }

        public void AddMessages(string key, IEnumerable<string> messages)
        {
            if ( messages.Count() == 0 ) return;

            if ( ModelState.ContainsKey(key) )
            {
                var modelMessages = new List<string>(ModelState[key]);
                modelMessages.AddRange(messages);
                ModelState[key] = modelMessages;
            }
            else
                ModelState.Add(key, messages);
        }
    }
}