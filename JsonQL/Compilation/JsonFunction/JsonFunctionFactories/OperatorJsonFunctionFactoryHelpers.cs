// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonFunction.JsonFunctionFactories;

/// <summary>
/// Provides helper methods for operator-based JSON function factory implementations.
/// This static class is designed to assist in the parsing and error handling
/// of operator expressions during the creation of JSON functions in the compilation pipeline.
/// </summary>
public static class OperatorJsonFunctionFactoryHelpers
{
    /// <summary>
    /// Generates a generic error result as a parse result of a JSON function.
    /// This method is used when an operator expression cannot be parsed into a JSON function.
    /// </summary>
    /// <param name="parsedSimpleValue">The parsed simple value that contains the context of the error.</param>
    /// <param name="operatorExpressionItem">The operator expression item that could not be parsed into a JSON function.</param>
    /// <returns>An instance of <see cref="ParseResult{TValue}"/> containing a JSON function parse error.</returns>
    public static ParseResult<IJsonFunction> GetGenericErrorResult(IParsedSimpleValue parsedSimpleValue, IOperatorExpressionItem operatorExpressionItem)
    {
        return new ParseResult<IJsonFunction>(CollectionExpressionHelpers.Create(new JsonObjectParseError("Failed to parse an operator expression into json function.",
            parsedSimpleValue.LineInfo.GenerateRelativePosition(operatorExpressionItem))));
    }
}