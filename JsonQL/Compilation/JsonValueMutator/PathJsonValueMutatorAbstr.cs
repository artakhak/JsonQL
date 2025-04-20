using OROptimizer.Diagnostics.Log;
using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonFunction.JsonFunctions;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueMutator;

public abstract class PathJsonValueMutatorAbstr : JsonValueMutatorAbstr
{
    protected PathJsonValueMutatorAbstr(IParsedSimpleValue parsedSimpleValue, IJsonValuePathJsonFunction jsonValuePathJsonFunction) 
        : base(jsonValuePathJsonFunction.LineInfo)
    {
        JsonValuePathJsonFunction = jsonValuePathJsonFunction;
        ParsedSimpleValue = parsedSimpleValue;
    }

    /// <inheritdoc />
    public sealed override void Mutate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, List<IJsonObjectParseError> errors)
    {
        var parsedSimpleValue = MutatorHelpers.TryGetParsedSimpleValue(this.ParsedSimpleValue);

        if (parsedSimpleValue == null)
        {
            LogHelper.Context.Log.InfoFormat("The mutator [{0}] will not execute.", GetType().FullName);
            return;
        }

        var pathLookupResult = JsonValuePathJsonFunction.Evaluate(rootParsedValue, compiledParentRootParsedValues, null);

        if (pathLookupResult.Errors.Count > 0 || pathLookupResult.Value == null)
        {
            if (pathLookupResult.Errors.Count == 0)
            {
                errors.Add(new JsonObjectParseError($"Failed to lookup a json object by path [{JsonValuePathJsonFunction.JsonValuePath}].", JsonValuePathJsonFunction.JsonValuePath.LineInfo));
            }
            else
            {
                errors.AddRange(pathLookupResult.Errors);
            }

            return;
        }

        var parsedValuesResult = pathLookupResult.Value.GetResultAsParsedValuesList(false, this.LineInfo);

        if (parsedValuesResult.Errors.Count > 0)
        {
            errors.AddRange(parsedValuesResult.Errors);
            return;
        }

        MutateValue(parsedSimpleValue, parsedValuesResult.Value?? Array.Empty<IParsedValue>(), errors);
    }

    protected IParsedSimpleValue ParsedSimpleValue { get; }

    protected IJsonValuePathJsonFunction JsonValuePathJsonFunction { get; }

    /// <summary>
    /// Mutates the json that owns <param name="currentParsedSimpleValueWithPath"></param>.
    /// </summary>
    /// <param name="currentParsedSimpleValueWithPath">Jason value that has the expression with referenced path.</param>
    /// <param name="referencedParsedValues">Json values looked up by referenced path <see cref="JsonValuePath"/>.
    /// The list will not be empty, since this class does the path validations, before calling <see cref="MutateValue"/>
    /// </param>
    /// <param name="errors">The implementation can add errors to this list.</param>
    protected abstract void MutateValue(
        IParsedSimpleValue currentParsedSimpleValueWithPath,
        IReadOnlyList<IParsedValue> referencedParsedValues,
        List<IJsonObjectParseError> errors);
}