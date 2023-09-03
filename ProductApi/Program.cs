using ProductManagement.Core;
using ProductManagement.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ProductApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var provider = builder.Configuration.GetSection("ActiveDbProvider").Value;
            
            if (provider == "MSSQL")
            {
                builder.Services.AddDbContext<ProductDbContext>(options =>
                        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly("ProductManagement.DataAccess.MSSQL")));
            }
            else if (provider == "PostgresSQL")
            {
                builder.Services.AddDbContext<ProductDbContext>(options =>
                        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection_PostgresSQL"),
                        b => b.MigrationsAssembly("ProductManagement.DataAccess.PostgreSQL")));
            }

            builder.Services.AddTransient<IProductService, ProductService>();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}