using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GenealogyWebAPI.Proxies;
using Refit;
using Microsoft.Extensions.Configuration;

namespace GenealogyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyNameController : ControllerBase
    {
        private readonly IConfiguration Configuration;

        public FamilyNameController(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        // GET api/familyname/name
        [HttpGet("{name}")]
        public async Task<ActionResult<string>> Get(string name)
        {
            string result = null;
            try
            {
                string baseUrl = Configuration["GenderizeBaseUrl"];
                result = await RestService.For<IGenderizeClient>(baseUrl).GetGenderForName(name);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }
    }
}
