using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using UoWAutofacParallelDbConnection;

namespace Test
{
    public class ParallelRequestsTests : IClassFixture<Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ParallelRequestsTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task ParallelRequests_ShouldHandleMultipleRequests()
        {
            // Set up tasks for 10 parallel POST requests to /api/sample/parallel-test
            var tasks = new Task<HttpResponseMessage>[10];

            for (int i = 0; i < 10; i++)
            {
                tasks[i] = _client.PostAsync("/api/sample/parallel-test", null);
            }

            // Wait for all tasks to complete
            var responses = await Task.WhenAll(tasks);

            // Assert that all responses were successful (Status Code 200)
            foreach (var response in responses)
            {
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            }
        }
    }
}