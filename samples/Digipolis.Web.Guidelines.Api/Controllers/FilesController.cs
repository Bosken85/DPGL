using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Digipolis.Web.Guidelines.Api.Logic;
using Digipolis.Web.Guidelines.Error;
using Digipolis.Web.Guidelines.Models;
using Digipolis.Web.Versioning;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.PlatformAbstractions;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Digipolis.Web.Guidelines.Api.Controllers
{
    [Route("values/{valueId:int}/[controller]")]
    public class FilesController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IErrorManager _errorManager;
        private readonly IFileLogic _fileLogic;

        public FilesController(IHostingEnvironment hostingEnvironment, IErrorManager errorManager, IFileLogic fileLogic)
        {
            _hostingEnvironment = hostingEnvironment;
            _errorManager = errorManager;
            _fileLogic = fileLogic;
        }

        // GET: api/values
        [HttpGet]
        [ProducesResponseType(typeof(MultipartFormDataContent), 200)]
        public IActionResult Get(int valueId, [FromQuery]Query queryOptions)
        {
            int total;
            var values = _fileLogic.GetAll(valueId, queryOptions, out total);
            var result = queryOptions.ToPagedResult(values, total, "Get", "Files");
            return Ok(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int valueId, int id)
        {
            throw new NotImplementedException();
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post(int valueId, [FromBody]IFormFileCollection files)
        {
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    if (fileName.EndsWith(".txt")) // Important for security if saving in webroot
                    {
                        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, @"\uploads\", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            await fileStream.FlushAsync();
                        }
                    }
                    
                }
            }
            return Ok();
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
