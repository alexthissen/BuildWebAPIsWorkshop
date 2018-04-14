using GenealogyWebAPI.ClientSdk;
using GenealogyWebAPI.Proxies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

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

        [TestMethod]
        public async Task OpenApiDocumentationAvailable()
        {
            // Act
            var response = await client.GetAsync("/swagger/index.html?url=/swagger/v1/swagger.json");

            // Assert
            response.EnsureSuccessStatusCode();
            string responseHtml = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseHtml.Contains("swagger"));
        }

        [TestMethod]
        public async Task GetFamilyName()
        {
            // Arrange 
            string name = "alex";

            // Act
            var result = await proxy.FamilyName.GetWithHttpMessagesAsync(name);

            // Assert
            Assert.IsNotNull(result, "Should have received a response.");
            Assert.AreEqual(HttpStatusCode.OK, result.Response.StatusCode, "Status code should be 200 OK");
            string responseBody = await result.Response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseBody.Contains(name), "Response body should contain original name");
        }
    }
}
