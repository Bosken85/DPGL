using System;
using System.Collections.Generic;
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
            if(!string.IsNullOrWhiteSpace(message)) AddMessage(message);
        }

        public ValidationException(string message, Exception exception = null) : base(message, exception)
        {
            AddMessage(message);
        }

        public void AddMessage(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
                AddMessages(string.Empty, new[] { message });
        }

        public void AddMessage(string key, string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
                AddMessages(key, new[] { message });
        }

        public void AddMessages(IEnumerable<string> messages)
        {
            AddMessages(string.Empty, messages);
        }

        public void AddMessages(string key, IEnumerable<string> messages)
        {
            if (messages == null || !messages.Any()) return;

            if (ModelState.ContainsKey(key))
            {
                var modelMessages = new List<string>(ModelState[key]);
                modelMessages.AddRange(messages);
                ModelState[key] = modelMessages;
            }
            else ModelState.Add(key, messages);
        }
    }
}