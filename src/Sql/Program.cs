// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Dapper;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace Sql;

public static class Program
{
    public static void Main(string[] args)
    {
        var connectionString =
            args.FirstOrDefault()
            ?? "Server=(local)\\SqlExpress; Database=MyApp; Trusted_connection=true";

        using var serviceProvider = CreateServices(connectionString);
        using var scope = serviceProvider.CreateScope();
        EnsureDatabase(connectionString, Constants.DbName);
        UpdateDatabase(scope.ServiceProvider);
    }

    /// <summary>
    /// Configure the dependency injection services
    /// </summary>
    private static ServiceProvider CreateServices(string connectionString)
    {
        return new ServiceCollection()
            // Add common FluentMigrator services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSqlServer()
                // Set the connection string
                .WithGlobalConnectionString(connectionString)
                // Define the assembly containing the migrations
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
            // Enable logging to console in the FluentMigrator way
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            // Build the service provider
            .BuildServiceProvider(false);
    }


    private static void EnsureDatabase(string connectionString, string name)
    {
        var parameters = new DynamicParameters();
        parameters.Add("name", name);
        using var connection = new SqlConnection(connectionString);
        var records = connection.Query("SELECT * FROM sys.databases WHERE name = @name",
            parameters);
        if (!records.Any())
        {
            connection.Execute($"CREATE DATABASE {name}");
        }
    }


    /// <summary>
    /// Update the database
    /// </summary>
    private static void UpdateDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}