using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonValueMutator;

/// <summary>
/// Represents data for a parsed expression within a compiled JSON value mutator.
/// </summary>
public interface IParsedExpressionData
{
    /// <summary>
    /// MutatorFunctionName.Examples:<br/>
    /// -In expression "$(x+y)", function name is "$".<br/>
    /// -In expression "$value(x+y)", function name is "$value".<br/>
    /// </summary>
    string MutatorFunctionName { get; }

    /// <summary>
    /// Index of template text that starts with the '$' character. For example, if the parsed text is "Int1 is $(Example.Object1.Int1) and Int2 is $(Example.Object1.Int2)",
    /// the value of <see cref="TemplateStartIndex"/> is the index of the first '$' character for the first expression parsed from "$(Example.Object1.Int1)",
    /// and it is the index of the second '$' character for the second expression parsed from "$(Example.Object1.Int2)".
    /// </summary>
    int TemplateStartIndex { get; }

    /// <summary>
    /// Length of template text that starts with the '$' character. For example, if the parsed text is "Int1 is $(Example.Object1.Int1) and Int2 is $(Example.Object1.Int2)",
    /// the value of <see cref="TemplateLength"/> is the length of a text "$(Example.Object1.Int1)" for the first parsed expression, and
    /// it is the length of $(Example.Object1.Int2)" for the second parsed expression.
    /// </summary>
    int TemplateLength { get; }
   
    /// <summary>
    /// Parsed expression.
    /// Example of parsed text is "Value is $value(Example.Object1.Int1:10)" or
    /// "Value is $(Example.Object1.String1:'String 1')".
    /// </summary>
    IExpressionItemBase ParsedExpressionItem { get; }
}

/// <inheritdoc />
public class ParsedExpressionData : IParsedExpressionData
{
    public ParsedExpressionData(string mutatorFunctionName, int templateStartIndex, int templateLength, IExpressionItemBase parsedExpressionItem)
    {
        MutatorFunctionName = mutatorFunctionName;
        ParsedExpressionItem = parsedExpressionItem;
        TemplateStartIndex = templateStartIndex;
        TemplateLength = templateLength;
    }

    /// <inheritdoc />
    public IExpressionItemBase ParsedExpressionItem { get; }

    /// <inheritdoc />
    public string MutatorFunctionName { get; }

    /// <inheritdoc />
    public int TemplateStartIndex { get; }

    /// <inheritdoc />
    public int TemplateLength { get; }
}