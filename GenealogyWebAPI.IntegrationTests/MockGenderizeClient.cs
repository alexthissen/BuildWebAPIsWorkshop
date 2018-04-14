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
        public Task<string> GetGenderForName(string name, [AliasAs("apikey")] string key)
        {
            return Task.FromResult<string>(@"{""name"":""alex"",""gender"":""male"",""probability"":0.87,""count"":5856}");
        }
    }
}
