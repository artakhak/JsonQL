using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.JsonFunction;

public interface IJsonFunction
{
    string FunctionName { get; }
    IJsonLineInfo? LineInfo { get; }

    /// <summary>
    /// Parent json function. Value can be null.
    /// </summary>
    IJsonFunction? ParentJsonFunction { get; }
   
    IParseResult<object?> EvaluateValue(
        IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData);
}

public static class JsonFunctionExtensions
{
    public static TypeCode? TryGetTypeCode(this IJsonFunction jsonFunction)
    {
        switch (jsonFunction)
        {
            case IBooleanJsonFunction:
                return TypeCode.Boolean;

            case IDoubleJsonFunction:
                return TypeCode.Double;

            case IStringJsonFunction:
                return TypeCode.String;

            case IDateTimeJsonFunction:
                return TypeCode.DateTime;
        }

        return null;
    }
}