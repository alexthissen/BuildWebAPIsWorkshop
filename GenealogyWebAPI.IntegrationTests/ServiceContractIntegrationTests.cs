using GenealogyWebAPI.ClientSdk;
using GenealogyWebAPI.Proxies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;

namespace GenealogyWebAPI.IntegrationTests
{
    [TestClass]
    public class ServiceContractIntegrationTests
    {
        TestServer server;
        HttpClient client;
        GenealogyAPI proxy;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>()
                .ConfigureTestServices(services =>
                {
                    services.AddTransient<IGenderizeClient, MockGenderizeClient>();
                });

            // Create test stack
            server = new TestServer(builder);
            client = server.CreateClient();
            proxy = new GenealogyAPI(client);
        }
    }
}
