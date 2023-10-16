using System.Data.Common;
using BallastLaneTestAssignment.Application.Common.Interfaces;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;
using BallastLaneTestAssignment.Infrastructure.Persistence;
using BallastLaneTestAssignment.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BallastLaneTestAssignment.Application.IntegrationTests;

using static Testing;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            var integrationConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            configurationBuilder.AddConfiguration(integrationConfig);
        });

        builder.ConfigureServices((builder, services) =>
        {

            var connectionString =
                builder.Configuration.GetConnectionString("DefaultConnection");
            
            services
                .Remove<ICurrentUserService>()
                .AddTransient(provider => Mock.Of<ICurrentUserService>(s =>
                    s.UserId == GetCurrentUserId()));

            // services
            //     .Remove<DbContextOptions<ApplicationDbContext>>()
            //     .AddDbContext<ApplicationDbContext>((sp, options) =>
            //         options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            //             builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.Remove<DbConnection>()
                .AddScoped<DbConnection>(provider => 
                new SqlConnection(connectionString)); 
        });
    }
}
