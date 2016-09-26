﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Digipolis.Web.Guidelines.Error
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly IExceptionMapper _mapper;
        private readonly ILogger<ExceptionHandler> _logger;
        private readonly IOptions<MvcJsonOptions> _options;

        public ExceptionHandler(IExceptionMapper mapper, ILogger<ExceptionHandler> logger,
            IOptions<MvcJsonOptions> options)
        {
            _mapper = mapper;
            _logger = logger;
            _options = options;
        }

        public async Task HandleAsync(HttpContext context, Exception ex)
        {
            var error = _mapper?.Resolve(ex);
            if (error == null) return;
            context.Response.Clear();
            context.Response.StatusCode = error.Status;
            context.Response.ContentType = "application/json";
            var json = JsonConvert.SerializeObject(error, _options?.Value?.SerializerSettings ?? new JsonSerializerSettings());
            await context.Response.WriteAsync(json);
            LogException(error, ex);
        }

        public void Handle(HttpContext context, Exception ex)
        {
            var error = _mapper?.Resolve(ex);
            if (error == null) return;
            context.Response.Clear();
            context.Response.StatusCode = error.Status;
            context.Response.ContentType = "application/json";
            var json = JsonConvert.SerializeObject(error, _options?.Value?.SerializerSettings ?? new JsonSerializerSettings());
            context.Response.WriteAsync(json).Wait();
            LogException(error, ex);
        }

        private void LogException(Error error, Exception exception)
        {
            var logMessage = new ExceptionLogMessage
            {
                Error = error,
                Exception = exception
            };

            var logAsJson = JsonConvert.SerializeObject(logMessage, _options?.Value?.SerializerSettings ?? new JsonSerializerSettings());
            if (error.Status >= 500 && error.Status <= 599)
                _logger?.LogError(logAsJson);
            else if (error.Status >= 400 && error.Status <= 499)
                _logger?.LogDebug(logAsJson);
            else _logger?.LogInformation(logAsJson);
        }
    }
}
