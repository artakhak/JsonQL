using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonObjects;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Compilation.JsonValueMutator;

public abstract class JsonSimpleValueMutatorAbstr : JsonValueMutatorAbstr
{
    protected JsonSimpleValueMutatorAbstr(IParsedSimpleValue parsedSimpleValue, IJsonLineInfo? lineInfo) : base(lineInfo)
    {
        ParsedSimpleValue = parsedSimpleValue;
    }

    public override void Mutate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, List<IJsonObjectParseError> errors)
    {
        var parsedSimpleValue = MutatorHelpers.TryGetParsedSimpleValue(this.ParsedSimpleValue);

        if (parsedSimpleValue == null)
        {
            LogHelper.Context.Log.InfoFormat("The mutator [{0}] will not execute.", GetType().FullName!);
            return;
        }

        var textGeneratorExpressionResult = GenerateStringValue(rootParsedValue, compiledParentRootParsedValues);

        if (textGeneratorExpressionResult.Errors.Count > 0 || textGeneratorExpressionResult.Value == null)
        {
            if (textGeneratorExpressionResult.Errors.Count == 0)
            {
                errors.Add(new JsonObjectParseError("Failed to parse the expression.", LineInfo));
            }
            else
            {
                errors.AddRange(textGeneratorExpressionResult.Errors);
            }

            return;
        }

        parsedSimpleValue.Value = textGeneratorExpressionResult.Value;
    }

    /// <summary>
    /// Parsed simple value with <see cref="IParsedSimpleValue.Value"/> containing the text parsed to <see cref="IJsonSimpleValueExpressionToStringConverter"/>.
    /// </summary>
    protected IParsedSimpleValue ParsedSimpleValue { get; }

    protected abstract IParseResult<string> GenerateStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues);
}