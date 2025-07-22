// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonQL.NewtonsoftImplementations;

/// <summary>
/// Defines a contract for parsing JSON text into a <see cref="JToken"/> object
/// using specific settings provided via <see cref="JsonLoadSettings"/>.
/// </summary>
public interface ICustomNewtonsoftJToken
{
    /// <summary>
    /// Parses the provided JSON string into a <see cref="JToken"/> object
    /// using the specified <see cref="JsonLoadSettings"/>.
    /// </summary>
    /// <param name="json">The JSON string to parse.</param>
    /// <param name="settings">The settings to use when loading the JSON content.</param>
    /// <returns>A <see cref="JToken"/> object representing the parsed JSON content.</returns>
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
        
        JToken jsonToken = JToken.Load(reader, settings);

        while (reader.Read())
        {
            // Any content encountered here other than a comment will throw in the reader.
        }

        return jsonToken;
    }
}
