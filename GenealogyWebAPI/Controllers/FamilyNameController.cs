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
        [HttpGet("{name}")]
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
