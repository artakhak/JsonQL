using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.JsonFunctions;

public class TextToLowerCaseJsonFunction : TextTransformationJsonFunctionAbstr
{
    public TextToLowerCaseJsonFunction(IJsonFunction stringJsonFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : 
        base(JsonFunctionNames.StringToLowerCase, stringJsonFunction, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<string> ConvertString(string value)
    {
        return new ParseResult<string>(value.ToLower());
    }
}