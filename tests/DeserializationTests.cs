using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Honeycomb.AppService;

namespace tests;

public class DeserializationTests
{
    [Theory]
    [InlineData("httplogs.json")]
    [InlineData("httplogswithpropertiesobject.json")]
    public async Task HttpLog_WithStringProperties_HasObjectPopulated(string filename)
    {
        var path = GetCurrentPath();
        if (path == null)
            throw new Exception("Couldn't get current file path");

        var file = Path.Combine(path, "..", "example-data", filename);
        using (var sr = new FileStream(file, FileMode.Open))
        {
            var httpLogs = await JsonSerializer.DeserializeAsync<IngestRecords<HttpLogRecord>>(sr);
            Assert.NotNull(httpLogs);
            Assert.NotNull(httpLogs!.records[0].properties);
        }
    }

    private string? GetCurrentPath([CallerFilePath]string filepath = null!)
    {
        return new FileInfo(filepath).DirectoryName;
    }
}