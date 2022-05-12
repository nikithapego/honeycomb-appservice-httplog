using Honeycomb.AppService;
using Honeycomb.Models;

namespace tests;

public class ExtensionTests
{
    [Fact]
    public void ValidResourceId_ApplyAllResourceProperties()
    {
        var validEvent = new HoneycombEvent();
        var subscriptionId = "D8573CE5-2AA6-49AB-8FDE-FC970C5FFCE1";
        var resourceGroup = "test-resource-group";
        var siteName = "test-site";
        var validResourceId = $"/SUBSCRIPTIONS/{subscriptionId}/RESOURCEGROUPS/{resourceGroup}/PROVIDERS/MICROSOFT.WEB/SITES/{siteName}";
        var logRecord = new HttpLogRecord {
            resourceId = validResourceId
        };
        validEvent.ApplyAzureResourceProperties(logRecord);

        Assert.Equal(subscriptionId, validEvent.Data["azure.subscription"]);
        Assert.Equal(resourceGroup, validEvent.Data["azure.resource_group"]);
        Assert.Equal(siteName, validEvent.Data["azure.appservice_name"]);
    }
}