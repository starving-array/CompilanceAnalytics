using System.Net.Http.Json;
using Xunit;
using ComplianceAnalytics.API;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ComplianceAnalytics.Tests.Integration
{
    public class AnalyticsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AnalyticsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAnalytics_ShouldReturnUnauthorized_IfNoToken()
        {
            var response = await _client.GetAsync("/api/reports/compliance/region?region=APAC&workflowType=KYC");
            Console.WriteLine("Response: " + response);
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetAnalytics_ShouldReturnData_WhenAuthorized()
        {
            // Arrange: normally you’d get a real token, for test you can stub it
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "FAKE_JWT_TOKEN");

            var response = await _client.GetAsync("/api/reports/compliance?region=APAC&workflowType=KYC");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadFromJsonAsync<object>();
            Assert.NotNull(json);
        }
    }
}
