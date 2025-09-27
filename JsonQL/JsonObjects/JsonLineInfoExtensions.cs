using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.JsonObjects;

/// <summary>
/// Provides extension methods to work with instances of IJsonLineInfo.
/// </summary>
public static class JsonLineInfoExtensions
{
    /// <summary>
    /// Generates a relative position based on the specified JSON line information and the provided relative position offset.
    /// </summary>
    /// <param name="jsonLineInfo">An instance of <see cref="IJsonLineInfo"/> representing the current line and position in the JSON document.</param>
    /// <param name="relativePosition">The offset by which the position should be adjusted relative to the current line position.</param>
    /// <returns>A new instance of <see cref="IJsonLineInfo"/> containing the resulting line and position information.</returns>
    public static IJsonLineInfo GenerateRelativePosition(this IJsonLineInfo? jsonLineInfo, int relativePosition)
    {
        if (jsonLineInfo == null)
            return new JsonLineInfo(1, 1);

        return new JsonLineInfo(jsonLineInfo.LineNumber, jsonLineInfo.LinePosition + relativePosition);
    }

    /// <summary>
    /// Generates a relative position based on the specified JSON line information and the index provided by the expression item.
    /// </summary>
    /// <param name="jsonLineInfo">An instance of <see cref="IJsonLineInfo"/> representing the current line and position in the JSON document.</param>
    /// <param name="expressionItem">An instance of <see cref="IExpressionItemBase"/> representing the expression item containing the index information used to calculate the relative position.</param>
    /// <returns>A new instance of <see cref="IJsonLineInfo"/> containing the updated line and position information, or null if the original <paramref name="jsonLineInfo"/> is null.</returns>
    public static IJsonLineInfo? GenerateRelativePosition(this IJsonLineInfo? jsonLineInfo, IExpressionItemBase expressionItem)
    {
        return jsonLineInfo == null ? null : new JsonLineInfo(jsonLineInfo.LineNumber, jsonLineInfo.LinePosition + expressionItem.IndexInText);
    }
}