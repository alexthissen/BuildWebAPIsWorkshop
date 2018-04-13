using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GenealogyWebAPI.Proxies;
using Refit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Polly.Timeout;
using Microsoft.AspNetCore.Http;

namespace GenealogyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyNameController : ControllerBase
    {
        private readonly IGenderizeClient genderizeClient;
        private readonly IOptionsSnapshot<GenderizeApiOptions> genderizeOptions;

        public FamilyNameController(IGenderizeClient genderizeClient, IOptionsSnapshot<GenderizeApiOptions> genderizeOptions)
        {
            this.genderizeClient = genderizeClient;
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

                result = await genderizeClient.GetGenderForName(name, key);
            }
            catch (HttpRequestException)
            {
                return StatusCode(StatusCodes.Status502BadGateway, "Failed request to external resource.");
            }
            catch (TimeoutRejectedException)
            {
                return StatusCode(StatusCodes.Status504GatewayTimeout, "Timeout on external web request.");
            }
            catch (Exception)
            {
                // Exception shielding for all other exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, "Request could not be processed.");
            }
            return Ok(result);
        }
    }
}
