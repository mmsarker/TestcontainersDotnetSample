using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProductManagement.Data;
using ProductManagement.DataAccess;
using System;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Xunit;

namespace ProductApiTest.Fixtures
{
    public class DockerPGSQLWebAppFactoryFixture : WebApplicationFactory<ProductApi.Program> ,IAsyncLifetime
    {
        private PostgreSqlContainer _dbContainer;        

        public DockerPGSQLWebAppFactoryFixture()
        {
            this._dbContainer = new PostgreSqlBuilder().Build();            
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {            
            var connectionString = _dbContainer.GetConnectionString();
            base.ConfigureWebHost(builder);
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ProductDbContext>));
                services.AddDbContext<ProductDbContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                });
            });
        }
  
        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            using (var scope = Services.CreateScope())
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
            await _dbContainer.StopAsync();
        }
    }
}
