using GenealogyWebAPI.Model;
using GenealogyWebAPI.Proxies;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GenealogyWebAPI.IntegrationTests
{
    internal class MockGenderizeClient: IGenderizeClient
    {
        public Task<GenderizeResult> GetGenderForName(string name, string key)
        {
            return Task.FromResult<GenderizeResult>(new GenderizeResult() { Name = "alex", Gender = "male" });
        }
    }
}
