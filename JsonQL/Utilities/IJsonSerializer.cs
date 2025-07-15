// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Text;
using JsonQL.JsonObjects;
using Newtonsoft.Json;

namespace JsonQL.Utilities;

/// <summary>
/// Defines methods for serializing objects into JSON format.
/// </summary>
public interface IJsonSerializer
{
    /// <summary>
    /// Serializes a given parsed JSON value into a string representation, optionally using specified serialization parameters.
    /// </summary>
    /// <param name="parsedValue">The parsed JSON value to serialize.</param>
    /// <param name="jsonSerializerParameters">Optional parameters to customize the JSON serialization process.</param>
    /// <returns>A string representation of the serialized JSON.</returns>
    string Serialize(IParsedValue parsedValue, IJsonSerializerParameters? jsonSerializerParameters = null);
}

/// <inheritdoc />
public class JsonSerializer : IJsonSerializer
{
    public static readonly IJsonSerializerParameters DefaultJsonSerializerParameters = new JsonSerializerParameters
    {
        IndentationFromParent = "  ",
        Minify = false,
        NewLineIndentation = string.Empty
    };

    /// <inheritdoc />
    public string Serialize(IParsedValue parsedValue, IJsonSerializerParameters? jsonSerializerParameters = null)
    {
        jsonSerializerParameters ??= DefaultJsonSerializerParameters;

        switch (parsedValue)
        {
            case IParsedJson parsedJson:
            {
                var serializedValue = new StringBuilder();
                SerializeParsedJson(serializedValue, parsedJson, jsonSerializerParameters, 0, false);
                return serializedValue.ToString();
            }

            case IParsedArrayValue parsedArrayValue:
            {
                var serializedValue = new StringBuilder();
                SerializeParsedArrayValue(serializedValue, parsedArrayValue, jsonSerializerParameters, 0, false);
                return serializedValue.ToString();
            }

            case IParsedSimpleValue parsedSimpleValue:
                return GetSerializedSimpleJsonValue(parsedSimpleValue);

            default:
                throw new ApplicationException($"Invalid json object type [{parsedValue.GetType()}].");
        }
    }

    private string GetSerializedSimpleJsonValue(IParsedSimpleValue parsedSimpleValue)
    {
        if (parsedSimpleValue.Value == null)
            return "null";
        
        if (parsedSimpleValue.IsString)
        {
            return JsonConvert.SerializeObject(parsedSimpleValue.Value);
        }

        return parsedSimpleValue.Value;
    }

    private void SerializeParsedValue(StringBuilder serializedValue, IParsedValue parsedValue, IJsonSerializerParameters jsonSerializerParameters,
        int nestedLevel, bool startedOnNewLine)
    {
        switch (parsedValue)
        {
            case IParsedJson parsedJson:
                SerializeParsedJson(serializedValue, parsedJson, jsonSerializerParameters, nestedLevel, startedOnNewLine);
                break;

            case IParsedArrayValue parsedArrayValue:
                SerializeParsedArrayValue(serializedValue, parsedArrayValue, jsonSerializerParameters, nestedLevel, startedOnNewLine);
                break;

            case IParsedSimpleValue parsedSimpleValue:
                if (startedOnNewLine)
                {
                    ApplyNewLineIndention(serializedValue, jsonSerializerParameters);
                    var indentation = CreateIndentation(nestedLevel, jsonSerializerParameters.IndentationFromParent);
                    serializedValue.Append(indentation);
                }
                else
                {
                    serializedValue.Append(' ');
                }

                serializedValue.Append(GetSerializedSimpleJsonValue(parsedSimpleValue));
                break;
            default:
                throw new ApplicationException($"Invalid json object type: [{parsedValue.GetType().FullName}].");
        }
    }

    private string CreateIndentation(int nestedLevel, string indentation)
    {
        if (nestedLevel == 0)
            return string.Empty;

        var indentationStrBuilder = new StringBuilder(nestedLevel * indentation.Length);
        for (var i = 0; i < nestedLevel; ++i)
        {
            indentationStrBuilder.Append(indentation);
        }

        return indentationStrBuilder.ToString();
    }

    void ApplyNewLineIndention(StringBuilder serializedText, IJsonSerializerParameters jsonSerializerParameters)
    {
        if (!jsonSerializerParameters.Minify && jsonSerializerParameters.NewLineIndentation.Length > 0)
        {
            serializedText.Append(jsonSerializerParameters.NewLineIndentation);
        }
    }

    private void SerializeParsedJson(StringBuilder serializedText, IParsedJson parsedJson, IJsonSerializerParameters jsonSerializerParameters,
        int nestedLevel, bool startedOnNewLine)
    {
        var parsedJsonIndention = CreateIndentation(nestedLevel, jsonSerializerParameters.IndentationFromParent);

        if (startedOnNewLine)
        {
            if (!jsonSerializerParameters.Minify)
            {
                ApplyNewLineIndention(serializedText, jsonSerializerParameters);
                serializedText.Append(parsedJsonIndention);
            }
        }

        serializedText.Append("{");

        if (parsedJson.KeyValues.Count > 0)
        {
            var indentation = string.Empty;

            if (!jsonSerializerParameters.Minify)
                indentation = CreateIndentation(nestedLevel + 1, jsonSerializerParameters.IndentationFromParent);

            for (var i = 0; i < parsedJson.KeyValues.Count; ++i)
            {
                if (!jsonSerializerParameters.Minify)
                {
                    serializedText.AppendLine();
                    ApplyNewLineIndention(serializedText, jsonSerializerParameters);
                    serializedText.Append(indentation);
                }

                var keyValue = parsedJson.KeyValues[i];

                serializedText.Append("\"").Append(keyValue.Key).Append("\"").Append(":");

                if (!jsonSerializerParameters.Minify)
                    serializedText.Append(' ');

                SerializeParsedValue(serializedText, keyValue.Value, jsonSerializerParameters, nestedLevel + 1, false);

                if (i < parsedJson.KeyValues.Count - 1)
                    serializedText.Append(',');
            }
        }

        if (!jsonSerializerParameters.Minify)
        {
            serializedText.AppendLine();
            ApplyNewLineIndention(serializedText, jsonSerializerParameters);
            serializedText.Append(parsedJsonIndention);
        }

        serializedText.Append("}");
    }

    private void SerializeParsedArrayValue(StringBuilder serializedText, IParsedArrayValue parsedArrayValue, IJsonSerializerParameters jsonSerializerParameters,
        int nestedLevel, bool startedOnNewLine)
    {
        var parsedArrayIndention = CreateIndentation(nestedLevel, jsonSerializerParameters.IndentationFromParent);

        if (startedOnNewLine)
        {
            if (!jsonSerializerParameters.Minify)
            {
                ApplyNewLineIndention(serializedText, jsonSerializerParameters);
                serializedText.Append(parsedArrayIndention);
            }
        }

        serializedText.Append("[");

        if (parsedArrayValue.Values.Count > 0)
        {
            for (var i = 0; i < parsedArrayValue.Values.Count; ++i)
            {
                if (!jsonSerializerParameters.Minify)
                {
                    serializedText.AppendLine();
                }

                var value = parsedArrayValue.Values[i];

                SerializeParsedValue(serializedText, value, jsonSerializerParameters, nestedLevel + 1, !jsonSerializerParameters.Minify);

                if (i < parsedArrayValue.Values.Count - 1)
                    serializedText.Append(',');
            }
        }

        if (!jsonSerializerParameters.Minify)
        {
            serializedText.AppendLine();
            ApplyNewLineIndention(serializedText, jsonSerializerParameters);
            serializedText.Append(parsedArrayIndention);
        }

        serializedText.Append("]");
    }
}