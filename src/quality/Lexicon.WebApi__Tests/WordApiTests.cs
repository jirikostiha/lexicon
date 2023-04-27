namespace Lexicon.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FakeItEasy;
    using FluentAssertions;
    using Lexicon.EntityModel;
    using Lexicon.TestReady;
    using Lexicon.WebApi.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;
    using System.Security.Policy;

    public class WordApiTests
    {
        private readonly WebApplicationFactory<Program> _factory;

        public WordApiTests()
        {
            _factory = new WebApplicationFactory<Program>();
        }

        [Theory]
        [Trait("Category", "positive")]
        [InlineData("/api/words")]
        [InlineData("/api/words?page=0&pageSize=100")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            using var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            response.Content.Headers.ContentType.ToString().Should().Be("text/html; charset=utf-8");
        }
    }
}
