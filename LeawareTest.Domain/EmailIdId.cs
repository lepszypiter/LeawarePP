using System.Text.Json;
using System.Text.Json.Serialization;

namespace LeawareTest.Domain;

[JsonConverter(typeof(EmailIdJsonConverter))]
public record EmailId(Guid Value)
{
    public static readonly EmailId Invalid = new(Guid.Empty);

    public static EmailId ParseFromString(string str) => new(Guid.Parse(str));
}
public class EmailIdJsonConverter : JsonConverter<EmailId>
{
    public override EmailId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return new EmailId(Guid.Parse(reader.GetString()!));
        }
        catch (Exception)
        {
            return null;
        }
    }

    public override void Write(Utf8JsonWriter writer, EmailId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value.ToString("N"));
    }
}
