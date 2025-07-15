// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;
using UniversalExpressionParser;

namespace JsonQL.Compilation;

/// <summary>
/// The CompilationHelpers class provides methods to convert parse errors into different types of error representations,
/// facilitating the mapping of raw parsing errors into domain-specific error structures.
/// </summary>
internal static class CompilationHelpers
{
    /// <summary>
    /// Converts a list of parse error items into a list of <see cref="CompilationErrorItem"/> objects.
    /// This method maps raw parsing errors to domain-specific error structures for further processing.
    /// </summary>
    /// <param name="jsonObjectData">The JSON object data containing details about the JSON text being parsed.</param>
    /// <param name="parsedSimpleValue">The parsed simple value that provides line information for error positioning.</param>
    /// <param name="parseErrorItems">The list of parse error items to be converted.</param>
    /// <returns>A list of <see cref="CompilationErrorItem"/> containing error details such as the JSON text identifier, error messages, and line information.</returns>
    internal static IReadOnlyList<CompilationErrorItem> Convert(IJsonObjectData jsonObjectData, IParsedSimpleValue parsedSimpleValue, IReadOnlyList<IParseErrorItem> parseErrorItems)
    {
        return parseErrorItems.Select(parsedErrorItem =>
            new CompilationErrorItem(jsonObjectData.JsonTextData.TextIdentifier, parsedErrorItem.ErrorMessage,
                parsedSimpleValue.LineInfo.GenerateRelativePosition(parsedErrorItem.ErrorIndexInParsedText))).ToList();
    }

    /// <summary>
    /// Converts a list of parse error items into a list of <see cref="IJsonObjectParseError"/> objects.
    /// This method maps raw parsing errors to JSON-specific error representations, including error messages and line information.
    /// </summary>
    /// <param name="jsonObjectData">The JSON object data providing context about the JSON structure being parsed.</param>
    /// <param name="parsedSimpleValue">The parsed simple value containing line information for calculating relative positions.</param>
    /// <param name="parseErrorItems">The list of parse error items to be converted into JSON object parse errors.</param>
    /// <returns>A list of <see cref="IJsonObjectParseError"/> containing detailed error information such as error messages and line positions.</returns>
    internal static IReadOnlyList<IJsonObjectParseError> ConvertToJsonObjectParseError(IJsonObjectData jsonObjectData, IParsedSimpleValue parsedSimpleValue, IReadOnlyList<IParseErrorItem> parseErrorItems)
    {
        return parseErrorItems.Select(parsedErrorItem =>
            new JsonObjectParseError(parsedErrorItem.ErrorMessage,
                parsedSimpleValue.LineInfo.GenerateRelativePosition(parsedErrorItem.ErrorIndexInParsedText))).ToList();
    }
}