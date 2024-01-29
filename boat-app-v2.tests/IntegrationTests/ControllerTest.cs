using boat_app_v2.BusinessLogic;
using boat_app_v2.Entities.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;
using Assert = NUnit.Framework.Assert;
namespace boat_app_v2.tests.IntegrationTests;

/// <summary>
/// Basic integration tests using the services with a local test database.
/// </summary>
public abstract class ControllerTest  : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
{
    protected BoatContext Context { get; set; }

    protected readonly WebApplicationFactory<Startup> _factory;
    private readonly IConfiguration _configuration;
    protected DbContextOptions<BoatContext> Options { get; private set; }

    public ControllerTest(WebApplicationFactory<Startup> factory)
    {
        _configuration = InitConfiguration();
      //  _dbName = _configuration.GetValue<string>("DatabaseNames:ProductManApi");
       // _testDatabase = new TestDatabase(new Logger<TestDatabase>(new LoggerFactory()), _dbName);
            
        _factory = factory.WithWebHostBuilder(builder =>
        {
            // replace the actual services with the mocked ones
            builder.ConfigureTestServices(
                services =>
                {
                    // provide an instance instead of using DI to be able to manually modify that instance in the
                    // test preparation later on
               //     services.Replace(ServiceDescriptor.Singleton<DatabasePoolForMySql>(_testDatabase));
                    
                });
        });

        SetupInMemoryDatabase();
    }
    
    private void SetupInMemoryDatabase()
    {
        Options = new DbContextOptionsBuilder<BoatContext>()
            .UseInMemoryDatabase(databaseName: "boat_db")
            .Options;
        
        Context = new BoatContext(Options);
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
        
        // Run the test against one instance of the context
        

        // Use a separate instance of the context to verify correct data was saved to database
        using (var context = new BoatContext(Options))
        {
            Assert.AreEqual(1, context.Boats.Count());
        }
    }
    
    public static IConfiguration InitConfiguration()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .AddEnvironmentVariables() 
            .Build();
        return config;
    }
    
    public void Dispose()
    {
        Context.Database.EnsureDeleted();
    }
}