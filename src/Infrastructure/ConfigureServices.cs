using System.Data.Common;
using BallastLaneTestAssignment.Application.Common.Interfaces;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database;
using BallastLaneTestAssignment.Application.Common.Interfaces.Database.Repositories;
using BallastLaneTestAssignment.Application.Common.Interfaces.Identity;
using BallastLaneTestAssignment.Infrastructure.Files;
using BallastLaneTestAssignment.Infrastructure.Identity;
using BallastLaneTestAssignment.Infrastructure.Persistence;
using BallastLaneTestAssignment.Infrastructure.Persistence.Repositories;
using BallastLaneTestAssignment.Infrastructure.Services;
using Dapper.FluentMap;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BallastLaneTestAssignment.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("BallastLaneTestAssignmentDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        // services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddScoped<DapperDbInitializer>();

        services
            .AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

        services.AddAuthentication()
            .AddIdentityServerJwt();

        services.AddAuthorization(options =>
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));
        
        services.AddDapperSqlServer(configuration.GetConnectionString("DefaultConnection"));

        return services;
    }
    
    private static void AddDapperSqlServer(this IServiceCollection services, string? connectionString)
    {
        if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
        
        services.AddScoped<DbConnection>(provider => 
            new SqlConnection(connectionString));  
        
        // FluentMapper.Initialize((config) => 
        // {
        //     config.AddMap(new DapperPrescriptionListMappings());
        // });
        
        services.AddScoped<IPrescriptionItemsRepository, PrescriptionItemsRepository>();
        services.AddScoped<IPrescriptionListRepository, PrescriptionListRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

    }  
}
