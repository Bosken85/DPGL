﻿using System;
using System.Collections.Generic;
using System.Linq;
using Digipolis.Web.Guidelines.Models;
using Digipolis.Web.Guidelines.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Generator;
using Digipolis.Web.Guidelines.Error;

namespace Digipolis.Web.Guidelines.Swagger
{
    public class ResponseDefinitions : IOperationFilter
    {
        protected IEnumerable<Attribute> ActionAttributes { get; private set; }

        protected IEnumerable<Attribute> ControllerAttributes { get; private set; }

        protected IEnumerable<Attribute> CombinedAttributes { get; private set; }

        public void Apply(Operation operation, OperationFilterContext context)
        {
            ActionAttributes = context.ApiDescription.GetActionAttributes().OfType<Attribute>();
            ControllerAttributes = context.ApiDescription.GetControllerAttributes().OfType<Attribute>();
            CombinedAttributes = ActionAttributes.Union(ControllerAttributes);
            ConfigureResponses(operation, context);
            ExcludeSwaggerResonse(operation);
        }

        protected virtual void ConfigureResponses(Operation operation, OperationFilterContext context)
        {
            UnauthorizedResponse(operation, context);
            InternalServerErrorResponse(operation, context);
            BadRequestResponse(operation, context);
            NoContentResponse(operation, context);
            CreatedResponse(operation, context);
            OkResponse(operation, context);
            NotFoundResponse(operation, context);
            AddReturnSchemaToVersionEndpoint(operation, context);
        }

        protected virtual void UnauthorizedResponse(Operation operation, OperationFilterContext context)
        {
            var allowsAnonymous = ActionAttributes.OfType<AllowAnonymousAttribute>().Any();
            if (operation.Responses.ContainsKey("401"))
            {
                if (allowsAnonymous) operation.Responses.Remove("401");
                else operation.Responses["401"].Description = "Unauthorized";
                return;
            };

            if (!CombinedAttributes.OfType<AuthorizeAttribute>().Any() || allowsAnonymous) return;
            operation.Responses.Add("401", new Response
            {
                Description = "Unauthorized",
                Schema = context.SchemaRegistry.GetOrRegister(typeof(Error.Error))
            });
        }

        protected virtual void BadRequestResponse(Operation operation, OperationFilterContext context)
        {
            if (operation.Responses.ContainsKey("400")) return;
            if (!CombinedAttributes.Any(x => x is ValidateModelAttribute || x is HttpPostAttribute || x is HttpPutAttribute || x is HttpPatchAttribute))
                return;

            operation.Responses.Add("400", new Response
            {
                Description = "Validation error",
                Schema = context.SchemaRegistry.GetOrRegister(typeof(Error.Error))
            });
        }

        protected virtual void InternalServerErrorResponse(Operation operation, OperationFilterContext context)
        {
            if (operation.Responses.ContainsKey("500")) return;
            operation.Responses.Add("500", new Response
            {
                Description = "Technical error",
                Schema = context.SchemaRegistry.GetOrRegister(typeof(Error.Error)),
            });
        }

        protected virtual void NoContentResponse(Operation operation, OperationFilterContext context)
        {
            if (!CombinedAttributes.OfType<HttpDeleteAttribute>().Any()) return;
            operation.Responses.Remove("200");
            if (operation.Responses.ContainsKey("204")) return;

            operation.Responses.Add("204", new Response
            {
                Description = "Removed"
            });
        }

        protected virtual void CreatedResponse(Operation operation, OperationFilterContext context)
        {
            if (!CombinedAttributes.OfType<HttpPostAttribute>().Any()) return;
            if (!operation.Responses.ContainsKey("201")) return;
            var response = operation.Responses["201"];
            if (response.Description.Equals("Success", StringComparison.CurrentCultureIgnoreCase))
                response.Description = "Created";
        }

        protected virtual void OkResponse(Operation operation, OperationFilterContext contexts)
        {
            if (!operation.Responses.ContainsKey("200")) return;
            var response = operation.Responses["200"];
            if (response.Description.Equals("Success", StringComparison.CurrentCultureIgnoreCase))
                response.Description = "Ok";
        }

        protected virtual void NotFoundResponse(Operation operation, OperationFilterContext context)
        {
            if (operation.Responses.ContainsKey("404"))
            {
                var response = operation.Responses["404"];
                if (response.Description.Equals("Client Error", StringComparison.CurrentCultureIgnoreCase))
                    response.Description = "Not found";
                return;
            }

            if (!context.ApiDescription.ParameterDescriptions.Where(x => !x.Name.Equals("apiVersion") && x.Source.Id.Equals("Path")).Any())
                return;

            operation.Responses.Add("404", new Response
            {
                Description = "Not found",
                Schema = context.SchemaRegistry.GetOrRegister(typeof(Error.Error))
            });
        }

        protected virtual void AddReturnSchemaToVersionEndpoint(Operation operation, OperationFilterContext context)
        {
            var operationId = context.ApiDescription.RelativePath.Contains("apiVersion") ? "ByApiVersionStatusVersionGet" : "StatusVersionGet";
            if (!operation.OperationId.Equals(operationId)) return;
            operation.Responses["200"].Schema = context.SchemaRegistry.GetOrRegister(typeof(VersionInfo));
            operation.Summary = "Get the version of the application";
            operation.Produces.Add("application/json");
        }

        private void ExcludeSwaggerResonse(Operation operation)
        {
            var excludes = CombinedAttributes.OfType<ExcludeSwaggerResonseAttribute>();
            if (!excludes.Any()) return;

            var codes = excludes.SelectMany(x => x.HttpCodes).Distinct().Select(x=> x.ToString());
            foreach (var code in codes)
            {
                if (!operation.Responses.ContainsKey(code)) continue;
                operation.Responses.Remove(code);
            }
        }
    }
}