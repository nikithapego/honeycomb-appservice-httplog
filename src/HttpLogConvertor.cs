using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Honeycomb.AppService;

public class LogPropertiesConvertor : JsonConverter<HttpLogProperties>
{
    public override HttpLogProperties Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            return JsonSerializer.Deserialize<HttpLogProperties>(ref reader, options);
        }
        return JsonSerializer.Deserialize<HttpLogProperties>(reader.GetString());
    }

    public override void Write(Utf8JsonWriter writer, HttpLogProperties value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}