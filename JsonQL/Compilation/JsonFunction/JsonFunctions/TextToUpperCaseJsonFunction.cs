using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

public class TextToUpperCaseJsonFunction : TextTransformationJsonFunctionAbstr
{
    public TextToUpperCaseJsonFunction(IJsonFunction stringJsonFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(JsonFunctionNames.StringToUpperCase, stringJsonFunction, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<string> ConvertString(string value)
    {
        return new ParseResult<string>(value.ToUpper());
    }
}