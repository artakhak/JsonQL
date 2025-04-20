using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonQL.NewtonsoftImplementations;

public interface ICustomNewtonsoftJToken
{
    JToken Parse(string json, JsonLoadSettings? settings);
}

/// <summary>
/// Implementation of <see cref="ICustomNewtonsoftJToken"/>.
/// Should be kept in sync with <see cref="JToken.Parse(string, JsonLoadSettings?)"/>
/// </summary>
public class CustomNewtonsoftJToken : ICustomNewtonsoftJToken
{
    /// <inheritdoc />
    public JToken Parse(string json, JsonLoadSettings? settings)
    {
        using JsonReader reader = new JsonTextReader(new StringReader(json));

        reader.DateParseHandling = DateParseHandling.None;
       
        JToken t = JToken.Load(reader, settings);

        while (reader.Read())
        {
            // Any content encountered here other than a comment will throw in the reader.
        }

        return t;
    }
}