// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonFunction.JsonFunctionFactories;

/// <summary>
/// A factory for paring a constant text expression <see cref="INumericExpressionItem"/> (e.g., 'Some text', etc.) into a <see cref="IDoubleJsonFunction"/>.
/// </summary>
public interface IConstantTextJsonFunctionFactory
{
    /// <summary>
    /// Tries to create <see cref="IJsonFunction"/> from braces expression. 
    /// Example of special literal functions are: value, true, etc. 
    /// </summary>
    /// <param name="parsedSimpleValue">Parsed JSON value which contains the expression to be parsed.</param>
    /// <param name="constantTextExpressionItem">Constant text expression to convert to <see cref="IJsonFunction"/>.</param>
    /// <param name="jsonFunctionContext">If not null, parent function data.</param>
    /// <returns>
    /// Returns parse result.
    /// If the value <see cref="IParseResult{TValue}.Errors"/> is not empty, function failed to be parsed.
    /// Otherwise, the value <see cref="IParseResult{TValue}.Value"/> will be non-null, if function parsed, or null, if <see cref="IJsonFunction"/> does not
    /// know how to parse the expression into <see cref="IJsonFunction"/> (in which case the caller of this method will either try to parse the expression
    /// some other way, or will report an error).
    /// </returns>
    IParseResult<IStringJsonFunction> TryCreateConstantTextFunction(IParsedSimpleValue parsedSimpleValue, IConstantTextExpressionItem constantTextExpressionItem,
        IJsonFunctionValueEvaluationContext jsonFunctionContext);
}

/// <summary>
/// A factory for paring a constant text expression <see cref="INumericExpressionItem"/> (e.g., 'Some text', etc.) into a <see cref="IDoubleJsonFunction"/>.
/// </summary>
public class ConstantTextJsonFunctionFactory : JsonFunctionFactoryAbstr, IConstantTextJsonFunctionFactory
{
    /// <inheritdoc />
    public IParseResult<IStringJsonFunction> TryCreateConstantTextFunction(IParsedSimpleValue parsedSimpleValue, IConstantTextExpressionItem constantTextExpressionItem,
        IJsonFunctionValueEvaluationContext jsonFunctionContext)
    {
        var text = constantTextExpressionItem.TextValue.CSharpText;

        return new ParseResult<IStringJsonFunction>(
            new ConstantTextJsonFunction(GetTextFunctionName(text), text, jsonFunctionContext,
                parsedSimpleValue.LineInfo.GenerateRelativePosition(constantTextExpressionItem)));
    }

    private static string GetTextFunctionName(string text)
    {
        var jsonValuePathToString = text;

        var maxNameLength = 30;
        return $"ConstantText:{(jsonValuePathToString.Length < maxNameLength ? jsonValuePathToString : jsonValuePathToString.Substring(0, maxNameLength))}";
    }
}
