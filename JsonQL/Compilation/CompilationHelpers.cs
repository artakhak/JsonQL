using JsonQL.JsonObjects;
using UniversalExpressionParser;

namespace JsonQL.Compilation;

internal class CompilationHelpers
{
    internal static IReadOnlyList<CompilationErrorItem> Convert(IJsonObjectData jsonObjectData, IParsedSimpleValue parsedSimpleValue, IReadOnlyList<IParseErrorItem> parseErrorItems)
    {
        return parseErrorItems.Select(parsedErrorItem =>
            new CompilationErrorItem(jsonObjectData.JsonTextData.TextIdentifier, parsedErrorItem.ErrorMessage,
                parsedSimpleValue.LineInfo.GenerateRelativePosition(parsedErrorItem.ErrorIndexInParsedText))).ToList();
    }

    internal static IReadOnlyList<IJsonObjectParseError> ConvertToJsonObjectParseError(IJsonObjectData jsonObjectData, IParsedSimpleValue parsedSimpleValue, IReadOnlyList<IParseErrorItem> parseErrorItems)
    {
        return parseErrorItems.Select(parsedErrorItem =>
            new JsonObjectParseError(parsedErrorItem.ErrorMessage,
                parsedSimpleValue.LineInfo.GenerateRelativePosition(parsedErrorItem.ErrorIndexInParsedText))).ToList();
    }
}