using Sql = Pulumi.AzureNative.Sql.V20220801Preview;

namespace WebappAndMsSql.IaC.Components;

public class SqlServerArgs
{
    public required Input<string> ServerName { get; set; }
    public required Input<string> DatabaseName { get; set; }
    public required Input<string> ResourceGroupName { get; set; }
    public required Input<string> AdminName { get; set; }
    public required Input<string> LoginPass { get; set; }
    public Input<string> Collation { get; set; } = "Japanese_CI_AS";
    public Input<int> SkuCapacity { get; set; } = 2;
    public Input<string> SkuFamily { get; set; } = "Gen5";
    public Input<string> SkuName { get; set; }  = "GP";
}

public class SqlServer: ComponentResource
{
    public Sql.Server Server;
    public Sql.Database Database;
    public Output<string> ServerName;

    public SqlServer(string name, SqlServerArgs args, ComponentResourceOptions? opts = null)
        : base("WebappAndMsSql:Components:SqlServer", name, opts)
    {
        var serverArgs = new Sql.ServerArgs
        {
            AdministratorLogin = args.AdminName,
            AdministratorLoginPassword = args.LoginPass,
            ResourceGroupName = args.ResourceGroupName,
            ServerName = args.ServerName,
            MinimalTlsVersion = "1.2",
            PublicNetworkAccess = "Enabled"
        };
        Server = new Sql.Server($"sql-{name}", serverArgs);
        
        ServerName = Server.Name.Apply(servername => $"{servername}.database.windows.net");
        
        var databaseArgs = new Sql.DatabaseArgs
        {
            ResourceGroupName = args.ResourceGroupName,
            ServerName = Server.Name,
            DatabaseName = args.DatabaseName,
            Collation = args.Collation,
            Sku = new Sql.Inputs.SkuArgs
            {
                Capacity = args.SkuCapacity,
                Family = args.SkuFamily,
                Name = args.SkuName, /*Serverless*/
            },
            MaxSizeBytes = 34359738368
        };
        
        this.Database = new Sql.Database($"sqldb-{name}", databaseArgs);
        
        RegisterOutputs(new Dictionary<string, object?>
        {
            {"SqlServerName", ServerName}
        });
    }


}
