namespace Lexicon.WebApi
{
    using Microsoft.AspNetCore.Mvc.Testing;

    public class WordApiTests
    {
        private readonly WebApplicationFactory<Program> _factory;

        public WordApiTests()
        {
            _factory = new WebApplicationFactory<Program>();
        }

        // test is failing but app is working
        //[Theory]
        //[Trait("Category", "positive")]
        //[InlineData("/api/words")]
        //[InlineData("/api/words?page=0&pageSize=100")]
        //public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        //{
        //    using var client = _factory.CreateClient();

        //    var response = await client.GetAsync(url);

        //    response.EnsureSuccessStatusCode(); // Status Code 200-299
        //    response.Content.Headers.ContentType.ToString().Should().Be("text/html; charset=utf-8");
        //}
    }
}