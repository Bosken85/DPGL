using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Digipolis.Web.Guidelines.Api.Logic;
using Digipolis.Web.Guidelines.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Digipolis.Web.Guidelines.Api.Controllers
{
    [Route("values/{valueId:int}/[controller]")]
    public class FilesController : Controller
    {
        private readonly IErrorManager _errorManager;
        private readonly IFileLogic _fileLogic;

        public FilesController(IErrorManager errorManager, IFileLogic fileLogic)
        {
            _errorManager = errorManager;
            _fileLogic = fileLogic;
        }

        // GET: api/values
        [HttpGet]
        [ProducesResponseType(typeof(MultipartContent), 200)]
        public IActionResult Get(int valueId, [FromQuery]Query queryOptions)
        {
            try
            {
                int total;
                var values = _fileLogic.GetAll(valueId, queryOptions, out total);
                var result = queryOptions.ToPagedResult(values, total, "Get", "Files");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, _errorManager.Error);
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int valueId, int id)
        {
            throw new NotImplementedException();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post(int valueId, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int valueId, int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int valueId, int id)
        {
            throw new NotImplementedException();
        }
    }
}
