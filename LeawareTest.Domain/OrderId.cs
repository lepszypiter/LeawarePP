using System.Text.Json;
using System.Text.Json.Serialization;

namespace LeawareTest.Domain;

[JsonConverter(typeof(CategoryIdJsonConverter))]
public record OrderId(Guid Value)
{
    public static readonly OrderId Invalid = new(Guid.Empty);

    public static OrderId ParseFromString(string str) => new(Guid.Parse(str));
}
public class CategoryIdJsonConverter : JsonConverter<OrderId>
{
    public override OrderId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return new OrderId(Guid.Parse(reader.GetString()!));
        }
        catch (Exception)
        {
            return null;
        }
    }

    public override void Write(Utf8JsonWriter writer, OrderId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value.ToString("N"));
    }
}
