using GenealogyWebAPI.Model;
using GenealogyWebAPI.Proxies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Polly.Timeout;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GenealogyWebAPI.Controllers.V2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
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
        [ProducesResponseType(typeof(FamilyProfile), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<FamilyProfile>> Get(string name)
        {
            GenderizeResult result = null;
            FamilyProfile profile;

            try
            {
                string baseUrl = genderizeOptions.Value.BaseUrl;
                string key = genderizeOptions.Value.DeveloperApiKey;

                result = await genderizeClient.GetGenderForName(name, key);
                Gender gender;
                profile = new FamilyProfile() {
                    Name = name,
                    Gender = Enum.TryParse<Gender>(result.Gender, true, out gender)
                        ? gender : Gender.Unknown
                };
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
            return Ok(profile);
        }
    }
}
