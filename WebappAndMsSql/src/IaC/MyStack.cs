

namespace WebappAndMsSql.IaC;

public class MyStack: Stack
{
    public MyStack()
    {
        var adminName = "appAdmin";
        var loginPass = "P@ssw0rd!";
        var azureConfig = new Config("azure-native");
        var location = azureConfig.Get("location") ?? "japaneast";
        var resourceName = azureConfig.Get("resourceName") ?? "hanaokaapps";
        // Create an Azure Resource Group
        var resourceGroup = new ResourceGroup($"rg-{resourceName}");
        
        // Create an Sqlserver
        var sqlServer = new SqlServer(resourceName, new SqlServerArgs
        {
            AdminName = adminName,
            ServerName = $"sql-{resourceName}",
            DatabaseName = $"sqldb-{resourceName}",
            ResourceGroupName = resourceGroup.Name,
            LoginPass = loginPass,
        });
        
        var webapps = new WebApps(resourceName, new AppsArgs
        {
            AdminName = adminName,
            DatabaseName = sqlServer.ServerName,
            ResourceGroupName = resourceGroup.Name,
            Location = location,
            LoginPass = loginPass
        });
        
    }
}
