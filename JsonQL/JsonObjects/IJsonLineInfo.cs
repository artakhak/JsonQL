using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.JsonObjects;

public interface IJsonLineInfo
{
    /// <summary>
    /// Gets the current line number.
    /// </summary>
    /// <value>The current line number or 0 if no line information is available (for example, when <see cref="HasLineInfo"/> returns <c>false</c>).</value>
    int LineNumber { get; }

    /// <summary>
    /// Gets the current line position.
    /// </summary>
    /// <value>The current line position or 0 if no line information is available (for example, when <see cref="HasLineInfo"/> returns <c>false</c>).</value>
    int LinePosition { get; }
}

public static class JsonLineInfoExtensions
{
    public static IJsonLineInfo GenerateRelativePosition(this IJsonLineInfo? jsonLineInfo, int relativePosition)
    {
        if (jsonLineInfo == null)
            return new JsonLineInfo(1, 1);

        return new JsonLineInfo(jsonLineInfo.LineNumber, jsonLineInfo.LinePosition + relativePosition);
    }

    public static IJsonLineInfo? GenerateRelativePosition(this IJsonLineInfo? jsonLineInfo, IExpressionItemBase expressionItem)
    {
        return jsonLineInfo == null ? null : new JsonLineInfo(jsonLineInfo.LineNumber, jsonLineInfo.LinePosition + expressionItem.IndexInText);
    }
}

public class JsonLineInfo : IJsonLineInfo

{
    public JsonLineInfo(int lineNumber, int linePosition)
    {
        LineNumber = lineNumber;
        LinePosition = linePosition;
    }

    public int LineNumber { get; }
    public int LinePosition { get; }
    public override string ToString()
    {
        return $"({this.LineNumber},{this.LinePosition}), {base.ToString()}";
    }
}