using Azure.Identity;
using Microsoft.Azure.Cosmos;
using System.Configuration;
using System.Diagnostics;

static class Globals
{
    public static CosmosClient Client = new
    (
        accountEndpoint: ConfigurationManager.AppSettings["CosmosDbAccountEndpoint"],

        // Using identity-based auth via Azure CLI
        tokenCredential: new DefaultAzureCredential(new DefaultAzureCredentialOptions
        {
            ExcludeVisualStudioCredential = true,
            ExcludeVisualStudioCodeCredential = true,
            ExcludeWorkloadIdentityCredential = true,
            ExcludeAzurePowerShellCredential = true,
            ExcludeEnvironmentCredential = true,
            ExcludeSharedTokenCacheCredential = true,
            ExcludeInteractiveBrowserCredential = true,
            ExcludeManagedIdentityCredential = true,
        }),

        clientOptions: new CosmosClientOptions
        {
            CosmosClientTelemetryOptions = new CosmosClientTelemetryOptions
            {
                DisableDistributedTracing = false,

                CosmosThresholdOptions = new CosmosThresholdOptions
                {
                    PointOperationLatencyThreshold = TimeSpan.FromMilliseconds(1),
                    NonPointOperationLatencyThreshold = TimeSpan.FromMilliseconds(1),
                }
            }
        }
    );

    public static ActivitySource ActivitySource = new ActivitySource("demo");
}
