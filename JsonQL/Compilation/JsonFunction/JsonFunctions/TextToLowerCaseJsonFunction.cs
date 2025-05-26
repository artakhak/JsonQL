using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a JSON function that transforms text to lowercase.
/// </summary>
/// <remarks>
/// This class inherits from <c>TextTransformationJsonFunctionAbstr</c> and provides an implementation
/// for converting text input to lowercase format. It is part of the JsonFunction hierarchy and works
/// within the context of JSON data processing and manipulation.
/// </remarks>
/// <example>
/// This class transforms input strings by converting all characters to their lowercase equivalent.
/// It requires an input JSON function, a function evaluation context, and optional line information.
/// </example>
public class TextToLowerCaseJsonFunction : TextTransformationJsonFunctionAbstr
{
    /// <summary>
    /// Represents a JSON function implementation that performs a text transformation to convert a string to lower case.
    /// </summary>
    /// <param name="functionName">Function name.</param>
    /// <param name="stringJsonFunction">The JSON function representing the string input to be transformed.</param>
    /// <param name="jsonFunctionContext">The context for evaluating JSON function values.</param>
    /// <param name="lineInfo">Optional line information for error or debugging purposes.</param>
    public TextToLowerCaseJsonFunction(string functionName, IJsonFunction stringJsonFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, stringJsonFunction, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<string> ConvertString(string value)
    {
        return new ParseResult<string>(value.ToLower());
    }
}