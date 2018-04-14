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
        public Task<string> GetGenderForName(string name, string key)
        {
            return Task.FromResult<string>("'alex'");
        }
    }
}
