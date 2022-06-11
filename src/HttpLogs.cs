using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Honeycomb.Models;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Honeycomb.AppService
{
    public class HttpLogs
    {
        private const string DataSetName = "HttpIngestLogs";
        private readonly IHoneycombService _honeycombService;
        private readonly IOptions<HoneycombApiSettings> _apiSettings;

        public HttpLogs(IHoneycombService honeycombService, IOptions<HoneycombApiSettings> apiSettings)
        {
            _honeycombService = honeycombService;
            _apiSettings = apiSettings;
        }

        [FunctionName("HttpLogs")]
        public async Task Run([EventHubTrigger("%EventHub:Name%", Connection = "AppServiceDiagnosticsHub")] EventData[] events, ILogger log)
        {
            var exceptions = new List<Exception>();
            int eventsProcessed = 0;
            foreach (EventData eventData in events)
            {
                try
                {
                    using var stream = new MemoryStream(eventData.Body.Array);
                    var httplogs = await JsonSerializer.DeserializeAsync<IngestRecords<HttpLogRecord>>(stream);
                    foreach (var httpLog in httplogs.records)
                    {
                        var logLine = JsonSerializer.Deserialize<HttpLogProperties>(httpLog.properties);

                        var ev = new HoneycombEvent();
                        ev.EventTime = httpLog.time;

                        ev.ApplyAzureResourceProperties(httpLog);
                        ev.ApplyLogLineProperties(logLine);
                        ev.DataSetName = $"{DataSetName}-{ev.Data["azure.appservice_name"]}";

                        _honeycombService.QueueEvent(ev);
                        eventsProcessed++;
                    }
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            await _honeycombService.Flush();
            log.LogInformation("Processed {eventsProcessed}", eventsProcessed);

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
    }
}
