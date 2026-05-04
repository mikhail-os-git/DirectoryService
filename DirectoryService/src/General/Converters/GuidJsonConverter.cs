using System.Text.Json;
using System.Text.Json.Serialization;

namespace General.Converters;

public class GuidJsonConverter: JsonConverter<Guid>
{
    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return Guid.TryParse(value, out var guid) ? guid : Guid.Empty;
    }

    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}