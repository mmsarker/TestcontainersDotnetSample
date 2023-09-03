using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ProductManagement.Data;
using ProductManagement.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProductApiTest.Fixtures
{
    public class WebAppFactoryFixture : IAsyncLifetime
    {
        private WebApplicationFactory<ProductApi.Program> _webApplicationFactory;
        private string connectionString = "Host=localhost; Database=ProductDb; Username=postgres; Password=admin";

        public HttpClient httpClient { get; private set; }

        public WebAppFactoryFixture()
        {
            _webApplicationFactory = new WebApplicationFactory<ProductApi.Program>().WithWebHostBuilder(builder =>
             {
                 builder.ConfigureTestServices(services =>
                 {
                     services.RemoveAll(typeof(DbContextOptions<ProductDbContext>));
                     services.AddDbContext<ProductDbContext>(options =>
                     {
                         options.UseNpgsql(connectionString);
                     });
                 });
             });

            httpClient = _webApplicationFactory.CreateClient();
        }

        public async Task InitializeAsync()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<ProductDbContext>();

                await cntx.Database.EnsureCreatedAsync();

                cntx.Products.Add(new Product
                {
                    ProductId = Guid.NewGuid().ToString(),
                    Name = "Test Product",
                    Description = "Test Product Description"
                });
                
                await cntx.SaveChangesAsync();
            }
        }

        public async Task DisposeAsync()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<ProductDbContext>();
                var products = cntx.Products;
                cntx.RemoveRange(products);
                await cntx.Database.EnsureDeletedAsync();
            }
        }
    }
}
