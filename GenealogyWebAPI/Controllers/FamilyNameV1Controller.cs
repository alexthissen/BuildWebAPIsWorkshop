using GenealogyWebAPI.Model;
using GenealogyWebAPI.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly.Timeout;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GenealogyWebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [AdvertiseApiVersions("2.0")]
    public class FamilyNameController : ControllerBase
    {
        // GET api/familyname/name
        /// <summary>
        /// Retrieve profile of person based on name.
        /// </summary>
        /// <param name="name">Name of person.</param>
        /// <returns>Detailed information regarding profile.</returns>
        /// <response code="200">The profile was successfully retrieved.</response>
        /// <response code="400">The request parameters were invalid or a timeout while retrieving profile occurred.</response>
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public ActionResult<string> Get(string name)
        {
            var version = HttpContext.GetRequestedApiVersion();
            return Ok(version);
        }
    }
}
