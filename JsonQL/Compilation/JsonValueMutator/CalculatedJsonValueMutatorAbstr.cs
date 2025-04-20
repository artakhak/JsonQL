using JsonQL.Compilation.JsonValueLookup;
using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonFunction;
using JsonQL.JsonFunction.JsonFunctions;
using JsonQL.JsonObjects;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Compilation.JsonValueMutator;

public abstract class CalculatedJsonValueMutatorAbstr : JsonValueMutatorAbstr
{
    private readonly IParsedJsonVisitor _parsedJsonVisitor;
    private readonly IStringFormatter _stringFormatter;

    protected CalculatedJsonValueMutatorAbstr(IParsedSimpleValue parsedSimpleValue,
        IJsonFunction jsonFunction,
        IParsedJsonVisitor parsedJsonVisitor,
        IStringFormatter stringFormatter)
        : base(jsonFunction.LineInfo)
    {
        _parsedJsonVisitor = parsedJsonVisitor;
        _stringFormatter = stringFormatter;
        JsonFunction = jsonFunction;
        ParsedSimpleValue = parsedSimpleValue;
    }

    /// <inheritdoc />
    public sealed override void Mutate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, List<IJsonObjectParseError> errors)
    {
        var parsedSimpleValue = MutatorHelpers.TryGetParsedSimpleValue(this.ParsedSimpleValue);

        if (parsedSimpleValue == null)
        {
            LogHelper.Context.Log.InfoFormat("The mutator [{0}] will not execute.", GetType().FullName??"null");
            return;
        }

        var jsonFunctionResult = JsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, null);

        if (jsonFunctionResult.Errors.Count > 0)
        {
            errors.AddRange(jsonFunctionResult.Errors);
            return;
        }


        //IReadOnlyList<IParsedValue> parsedValues = Array.Empty<IParsedValue>();
        IParsedValue? parsedEvaluatedValue = null;

        IParsedArrayValue? CreateParsedArrayValue(IReadOnlyList<IParsedValue> parsedValues)
        {
            if (parsedSimpleValue.ParentJsonValue == null)
            {
                LogHelper.Context.Log.ErrorFormat("The value of [{0}.{1}] is null.",
                    typeof(IParsedSimpleValue).FullName!, nameof(IParsedSimpleValue.ParentJsonValue));
                return null;
            }

            var parsedArrayValue = new ParsedArrayValue(_parsedJsonVisitor, parsedSimpleValue.RootParsedValue, parsedSimpleValue.ParentJsonValue,
                null);

            foreach (var parsedValue in parsedValues)
                parsedArrayValue.AddValue(parsedValue);

            return parsedArrayValue;
        }

        if (jsonFunctionResult.Value is IJsonValuePathLookupResult jsonValuePathLookupResult)
        {
            if (jsonValuePathLookupResult is ICollectionJsonValuePathLookupResult collectionJsonValuePathLookupResult)
            {
                parsedEvaluatedValue = CreateParsedArrayValue(collectionJsonValuePathLookupResult.ParsedValues);
            }
            else if (jsonValuePathLookupResult is ISingleItemJsonValuePathLookupResult singleItemJsonValuePathLookupResult)
            {
                parsedEvaluatedValue = singleItemJsonValuePathLookupResult.ParsedValue;
            }
            else
            {
                var jsonValuePathJsonFunction = this.JsonFunction as IJsonValuePathJsonFunction;

                LogHelper.Context.Log.ErrorFormat("Invalid implementation [{0}] in path [{1}]. Expected either [{2}] or [{3}].",
                    jsonValuePathLookupResult.GetType().FullName??"null",
                    jsonValuePathJsonFunction?.JsonValuePath.ToString()??"null",
                    typeof(ICollectionJsonValuePathLookupResult),
                    typeof(ISingleItemJsonValuePathLookupResult));
            }
        }
        else
        {
            if (jsonFunctionResult.Value == null)
            {
                parsedEvaluatedValue = new ParsedSimpleValue(parsedSimpleValue.RootParsedValue, null, null,
                    null, false);
            }
            else
            {
                var isStringJsonValue = true;

                switch (jsonFunctionResult.Value)
                {
                    case double:
                    case bool:
                        isStringJsonValue = false;
                        break;
                }

                switch (jsonFunctionResult.Value)
                {
                    case double:
                    case bool:
                    case string:
                    case DateTime:

                        if (!_stringFormatter.TryFormat(jsonFunctionResult.Value, out var formattedValue))
                        {
                            formattedValue = jsonFunctionResult.Value.ToString()!;
                            LogHelper.Context.Log.ErrorFormat("Failed to format value [{0}] of type [{1}].",
                                formattedValue, jsonFunctionResult.Value.GetType());
                        }

                        parsedEvaluatedValue = new ParsedSimpleValue(parsedSimpleValue.RootParsedValue, null, null,
                            formattedValue, isStringJsonValue);
                        break;

                    case IParsedValue evaluatedParsedValue:
                        parsedEvaluatedValue = evaluatedParsedValue;
                        break;

                    case IReadOnlyList<IParsedValue> evaluatedParsedValues:
                        parsedEvaluatedValue = CreateParsedArrayValue(evaluatedParsedValues);
                        break;
                }
            }
        }

        if (parsedEvaluatedValue == null)
        {
            parsedEvaluatedValue = new ParsedSimpleValue(parsedSimpleValue.RootParsedValue, parsedSimpleValue.ParentJsonValue, null,
                null, false);
        }

        MutateValue(parsedSimpleValue, parsedEvaluatedValue, errors);
    }

    protected IParsedSimpleValue ParsedSimpleValue { get; }

    protected IJsonFunction JsonFunction { get; }

    /// <summary>
    /// Mutates the json that owns <param name="currentParsedSimpleValueWithPath"></param>.
    /// </summary>
    /// <param name="currentParsedSimpleValueWithPath">Jason value that has the expression with referenced path.</param>
    /// <param name="referencedParsedValue">Evaluated json value.
    /// </param>
    /// <param name="errors">The implementation can add errors to this list.</param>
    protected abstract void MutateValue(
        IParsedSimpleValue currentParsedSimpleValueWithPath,
        IParsedValue referencedParsedValue,
        List<IJsonObjectParseError> errors);
}