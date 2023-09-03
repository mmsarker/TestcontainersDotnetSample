using System;
using ProductApiTest.Fixtures;
using FluentAssertions;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using ProductManagement.Data;

namespace ProductApiTest
{
    public class ProductControllerTestMSSQLContainer : IClassFixture<DockerMSSQLWebAppFactoryFixture>
    {
        private readonly DockerMSSQLWebAppFactoryFixture _webFactory;
        private readonly HttpClient _client;
        public ProductControllerTestMSSQLContainer(DockerMSSQLWebAppFactoryFixture webFactory)
        {
            this._webFactory = webFactory;
            this._client = this._webFactory.CreateClient();
        }

        [Fact]
        public async Task GetProducts_Should_Return_Products()
        {
            var response = await this._client.GetAsync("/product/products");
            var products = await response.Content.ReadFromJsonAsync<List<Product>>();
            products.Should().NotBeNull();
            products.Count.Should().Be(1);
        }
    }
}