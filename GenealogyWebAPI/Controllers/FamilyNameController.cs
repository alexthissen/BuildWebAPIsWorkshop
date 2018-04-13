using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GenealogyWebAPI.Proxies;
using Refit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace GenealogyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyNameController : ControllerBase
    {
        private readonly IOptionsSnapshot<GenderizeApiOptions> genderizeOptions;

        public FamilyNameController(IOptionsSnapshot<GenderizeApiOptions> genderizeOptions)
        {
            this.genderizeOptions = genderizeOptions;
        }

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
        public async Task<ActionResult<string>> Get(string name)
        {
            string result = null;

            try
            {
                string baseUrl = genderizeOptions.Value.BaseUrl;
                string key = genderizeOptions.Value.DeveloperApiKey;

                result = await RestService.For<IGenderizeClient>(baseUrl).GetGenderForName(name, key);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }
    }
}
