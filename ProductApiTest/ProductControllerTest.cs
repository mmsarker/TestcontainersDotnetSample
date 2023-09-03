using DotNet.Testcontainers.Builders;
using ProductApiTest.Fixtures;
using System;
using FluentAssertions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using ProductManagement.Data;

namespace ProductApiTest
{
    public class ProductControllerTest : IClassFixture<WebAppFactoryFixture>
    {        
        private readonly WebAppFactoryFixture _webFactory;

        public ProductControllerTest(WebAppFactoryFixture webFactory)
        {
            this._webFactory = webFactory;
        }

        [Fact]
        public async Task GetProducts_Should_Return_Products()
        {
            var response = await _webFactory.httpClient.GetAsync("/product/products");
            var products = await response.Content.ReadFromJsonAsync<List<Product>>();
            products.Should().NotBeNull();
            products.Count.Should().Be(1);
        }
    }
}