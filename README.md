# Ingestion of AppService HTTP logs to Honeycomb

This function will parse logs from Azure AppService using Azure EventHubs and ingest them into (Honeycomb.io)[http://honeycomb.io] in near realtime.

## Structure

### src

This is the main code for the function that will be deployed

### infra

This is the code to create the function in Azure using Pulumi.

### example-data

This is the example of the data that comes into the function so that you can see what is available.

## Settings

You can use Environment Variables for your function when deployed, or locally, you cah use a `local.settings.json` file.

```json
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "EventHub__Name": "insights-logs-appservicehttplogs",
        "HoneycombApiSettings__WriteKey": "<writekey from your honeycomb environment>",
        "AppServiceDiagnosticsHub": "<connectionstring>"
    }
}
```