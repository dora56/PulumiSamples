
using Deployment = Pulumi.Deployment;
using Resource = Pulumi.Resource;

namespace WebappAndMsSql.IaC.Test;

[TestFixture]
public class DevStackTest
{
    private static Task<ImmutableArray<Resource>> TestAsync()
    {
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
        var resources = await TestAsync();

        var sqlServers = resources.OfType<Server>().ToList();
        var name = await sqlServers.FirstOrDefault()!.Name.GetValueAsync();
        name.Should().Be("sql-hanaokaapps");
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
        var resources = await TestAsync();

        var sqlDatabases = resources.OfType<Database>().ToList();
        var name = await sqlDatabases.FirstOrDefault()!.Name.GetValueAsync();
        name.Should().Be("sqldb-hanaokaapps");
    }
}
