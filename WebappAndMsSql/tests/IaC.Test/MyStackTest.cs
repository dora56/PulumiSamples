
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
        var resources = await TestAsync();

        var resourceGroups = resources.OfType<ResourceGroup>().ToList();
        resourceGroups.Count.Should().Be(1, "a single resource group is expected");
    }
    
    [Test]
    public async Task SingleSqlServerExists()
    {
        var resources = await TestAsync();

        var sqlServers = resources.OfType<Server>().ToList();
        sqlServers.Count.Should().Be(1, "a single sql server is expected");
    }
    
    [Test]
    public async Task SingleSqlServerName()
    {
        var config = new Dictionary<string, object>();
        config.Add("azure-native:resourceName","testWeb");
        var resources = await TestAsync(config);

        var sqlServers = resources.OfType<Server>().ToList();
        var name = await sqlServers.FirstOrDefault()!.Name.GetValueAsync();
        name.Should().Be("sql-testWeb");
    }
    // Create SqlDatabase Test
    [Test]
    public async Task SingleSqlDatabaseExists()
    {
        var resources = await TestAsync();

        var sqlDatabases = resources.OfType<Database>().ToList();
        sqlDatabases.Count.Should().Be(1, "a single sql database is expected");
    }
    
    [Test]
    public async Task SingleSqlDatabaseName()
    {
        var config = new Dictionary<string, object>();
        config.Add("azure-native:resourceName","testWeb");
        var resources = await TestAsync(config);

        var sqlDatabases = resources.OfType<Database>().ToList();
        var name = await sqlDatabases.FirstOrDefault()!.Name.GetValueAsync();
        name.Should().Be("sqldb-testWeb");
    }
}
