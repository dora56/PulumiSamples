using Pulumi.AzureNative.Insights;


namespace WebappAndMsSql.IaC.Components;

public class AppsArgs
{
    public Input<string> ResourceGroupName { get; set; } = null!;
    public Input<string> Location { get; set; } = null!;
    public Input<string> SeverName { get; set; } = null!;
    public Input<string> DatabaseName { get; set; } = null!;
    public Input<string> AdminName { get; set; } = null!; 
    public Input<string> LoginPass { get; set; } = null!;
}
public class WebApps : ComponentResource
{
    public WebApps(string name, AppsArgs args, ComponentResourceOptions? opts = null)
        : base("WebappAndMsSql:Components:WebApp", name, opts)
    {
        var appServicePlan = new AppServicePlan($"asp-{name}", new AppServicePlanArgs
        {
            ResourceGroupName = args.ResourceGroupName,
            Location = args.Location,
            Sku = new SkuDescriptionArgs
            {
                Name = "F1",
                Tier = "Free"
            },
            Kind = "Linux"
        });

        var appInsights = new Component($"appi-{name}", new ComponentArgs
        {
            ResourceGroupName = args.ResourceGroupName,
            Location = args.Location,
            ApplicationType = "web",
            Kind = "web"
        });
        var webApp = new WebApp($"webapp-{name}", new WebAppArgs
        {
            ResourceGroupName = args.ResourceGroupName,
            ServerFarmId = appServicePlan.Id,
            SiteConfig = new SiteConfigArgs
            {
                AppSettings =
                {
                    new NameValuePairArgs{
                        Name = "APPINSIGHTS_INSTRUMENTATIONKEY",
                        Value = appInsights.InstrumentationKey
                    },
                    new NameValuePairArgs{
                        Name = "APPLICATIONINSIGHTS_CONNECTION_STRING",
                        Value = appInsights.InstrumentationKey.Apply(key => $"InstrumentationKey={key}"),
                    },
                    new NameValuePairArgs{
                        Name = "ApplicationInsightsAgent_EXTENSION_VERSION",
                        Value = "~3",
                    },
                },
                ConnectionStrings =
                {
                    new ConnStringInfoArgs
                    {
                        Name = "DatabaseConnection",
                        Type = ConnectionStringType.SQLAzure,
                        ConnectionString = $"Server= tcp:{args.SeverName};initial catalog={args.DatabaseName};userID={args.AdminName};password={args.LoginPass};Min Pool Size=0;Max Pool Size=30;Persist Security Info=true;"
                    }
                }
            }
        });
        RegisterOutputs(new Dictionary<string, object?>
        {
            {"WebAppUrl", webApp.Urn},
            {"AppInsightsInstrumentationKey", appInsights.InstrumentationKey}
        });
    }
}
