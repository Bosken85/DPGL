﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using Digipolis.Errors.Exceptions;
using Digipolis.Web.Filters;
using Digipolis.Web.Guidelines;
using Digipolis.Web.Guidelines.Models;
using Digipolis.Web.Guidelines.Paging;
using Digipolis.Web.Guidelines.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterKit.SwashBuckle.Api.Data;
using StarterKit.SwashBuckle.Api.Data.Entiteiten;
using StarterKit.SwashBuckle.Api.Logic;
using StarterKit.SwashBuckle.Api.Models;
using Swashbuckle.SwaggerGen.Annotations;

namespace StarterKit.SwashBuckle.Api.Controllers
{
    /// <summary>
    /// test
    /// </summary>
    /// <remarks>
    /// ## Documentatie
    /// 
    /// <a href="https://www.google.be" target="_blank">Google</a>
    /// 
    /// </remarks>
    [Route("/{version}/[controller]")]
    [Authorize]
    public class ValuesController : Controller
    {
        private readonly IErrorManager _errorManager;
        private readonly IValueLogic _valueLogic;

        public ValuesController(IErrorManager errorManager, IValueLogic valueLogic)
        {
            _errorManager = errorManager;
            _valueLogic = valueLogic;
        }

        /// <summary>
        /// Get all values 
        /// </summary>
        /// <remarks>
        /// ## Documentatie
        /// 
        /// <a href="https://www.google.be" target="_blank">Google</a>
        /// 
        /// </remarks>
        /// <param name="queryOptions">Query options from uri</param>
        /// <returns>An array of value objects</returns>
        [HttpGet()]
        [ProducesResponseType(typeof(PagedResult<ValueDto>), 200)]
        [AllowAnonymous]
        [Versions("v1", "v2")]
        public IActionResult Get([FromQuery]Query queryOptions)
        {
            try
            {
                int total;
                var values = _valueLogic.GetAll(queryOptions, out total);
                var result = queryOptions.ToPagedResult(values, total, "Get", "Values", new { test = 0 });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, _errorManager.Error);
            }
        }

        /// <summary>
        /// Get a value by id
        /// </summary>
        /// <param name="id">The id of the value</param>
        /// <returns>A value object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ValueDto), 200)]
        [ProducesResponseType(typeof(ValueDto), 401)]
        [AllowAnonymous]
        [Versions("v1", "v2")]
        public IActionResult Get(int id)
        {
            try
            {
                var value = _valueLogic.GetById(id);
                return Ok(value);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest(_errorManager.Error);
            }
            catch (NotFoundException)
            {
                return NotFound(_errorManager.Error);
            }
        }

        /// <summary>
        /// Add a new value
        /// </summary>
        /// <param name="value">A value object</param>
        /// <returns>The created value object</returns>
        [HttpPost]
        [ValidateModelState]
        [ProducesResponseType(typeof(ValueDto), 201)]
        [AllowAnonymous]
        [Versions("v1", "v2")]
        public IActionResult Post([FromBody, Required] ValueDto value)
        {
            try
            {
                value = _valueLogic.Add(value);
                return CreatedAtAction("Get", new {id = value.Id}, value);
            }
            catch (Exception)
            {
                return BadRequest(_errorManager.Error);
            }
        }

        /// <summary>
        /// Update an existing value object
        /// </summary>
        /// <param name="id">The id of the value</param>
        /// <param name="value">The updated value object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ValidateModelState]
        [AllowAnonymous]
        [Versions("v1", "v2")]
        public IActionResult Put(int id, [FromBody, Required] ValueDto value)
        {
            try
            {
                value = _valueLogic.Update(id, value);
                return Ok(value);
            }
            catch (NotFoundException)
            {
                return NotFound(_errorManager.Error);
            }
            catch (Exception ex)
            {
                return BadRequest(_errorManager.Error);
            }
        }

        /// <summary>
        /// Delete a value by it's Id
        /// </summary>
        /// <param name="id">The value's Id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Versions("v2")]
        public IActionResult Delete(int id)
        {
            try
            {
                _valueLogic.Delete(id);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound(_errorManager.Error);
            }
        }
    }
}
