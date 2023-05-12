
using Deployment = Pulumi.Deployment;
using Resource = Pulumi.Resource;

namespace WebappAndMsSql.IaC.Test;

[TestFixture]
public class MyStackTest
{
    private static Task<ImmutableArray<Resource>> TestAsync(Dictionary<string, object>? config = null)
    {
        if (config is not null)
        {
            var values = System.Text.Json.JsonSerializer.Serialize(config, new System.Text.Json.JsonSerializerOptions());

            Environment.SetEnvironmentVariable("PULUMI_CONFIG", values);
        }
        else
        {
            Environment.SetEnvironmentVariable("PULUMI_CONFIG", "");
        }
        
        return Deployment.TestAsync<MyStack>(new Mocks(), new TestOptions {IsPreview = false});
    }
    
    [Test]
    public async Task SingleResourceGroupExists()
    {
        // Arrange
        var resources = await TestAsync();
        
        // Act
        var resourceGroups = resources.OfType<ResourceGroup>().ToList();
        
        // Assert
        resourceGroups.Count.Should().Be(1, "a single resource group is expected");
    }
    
    [Test]
    public async Task SingleSqlServerExists()
    {
        // Arrange
        var resources = await TestAsync();
        
        // Act
        var sqlServers = resources.OfType<Server>().ToList();
        
        // Assert
        sqlServers.Count.Should().Be(1, "a single sql server is expected");
    }
    
    [Test]
    public async Task SingleSqlServerName()
    {
        // Arrange
        var config = new Dictionary<string, object>();
        config.Add("azure-native:resourceName","testWeb");
        var resources = await TestAsync(config);

        // Act
        var sqlServers = resources.OfType<Server>().ToList();
        var name = await sqlServers.FirstOrDefault()!.Name.GetValueAsync();
        
        // Assert
        name.Should().Be("sql-testWeb");
    }
    // Create SqlDatabase Test
    [Test]
    public async Task SingleSqlDatabaseExists()
    {
        // Arrange
        var resources = await TestAsync();
        
        // Act
        var sqlDatabases = resources.OfType<Database>().ToList();
        
        // Assert
        sqlDatabases.Count.Should().Be(1, "a single sql database is expected");
    }
    
    [Test]
    public async Task SingleSqlDatabaseName()
    {
        // Arrange
        var config = new Dictionary<string, object>();
        config.Add("azure-native:resourceName","testWeb");
        var resources = await TestAsync(config);
        
        // Act
        var sqlDatabases = resources.OfType<Database>().ToList();
        var name = await sqlDatabases.FirstOrDefault()!.Name.GetValueAsync();
        
        // Assert
        name.Should().Be("sqldb-testWeb");
    }
    
    [Test]
    public async Task SingleWebAppExists()
    {
        // Arrange
        var resources = await TestAsync();
        
        // Act
        var webApps = resources.OfType<WebApp>().ToList();
        
        // Assert
        webApps.Count.Should().Be(1, "a single web app is expected");
    }
    
    [Test]
    public async Task SingleWebAppName()
    {
        // Arrange
        var config = new Dictionary<string, object>();
        config.Add("azure-native:resourceName","testWeb");
        var resources = await TestAsync(config);

        // Act
        var webApps = resources.OfType<WebApp>().ToList();
        var name = await webApps.FirstOrDefault()!.Name.GetValueAsync();
        
        // Assert
        name.Should().Be("webapp-testWeb");
    }
}
