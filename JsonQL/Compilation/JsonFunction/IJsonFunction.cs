// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Defines the base interface for JSON functions used in compilation and evaluation contexts.
/// </summary>
public interface IJsonFunction
{
    /// <summary>
    /// The name of the function. Cannot be null.
    /// </summary>
    string FunctionName { get; }

    /// <summary>
    /// Represents the line information associated with a JSON function, including the line number and line position within a source file.
    /// Can be null if the line information is not available or applicable.
    /// </summary>
    IJsonLineInfo? LineInfo { get; }

    /// <summary>
    /// Represents the parent JSON function associated with the current function, if any.
    /// May be null if the current function has no parent.
    /// </summary>
    IJsonFunction? ParentJsonFunction { get; }
    
    /// <summary>
    /// Evaluates a specified value based on the provided root parsed value,
    /// a list of parent root parsed values, and the optional evaluation context data.
    /// </summary>
    /// <param name="rootParsedValue">The root parsed value to be evaluated.</param>
    /// <param name="compiledParentRootParsedValues">
    /// A read-only list of compiled parent root parsed values that may be used during evaluation.
    /// </param>
    /// <param name="contextData">
    /// Optional context data that provides additional information or state for the evaluation process.
    /// </param>
    /// <returns>
    /// An object implementing <see cref="IParseResult{T}"/> containing the evaluated value,
    /// or errors if the evaluation fails.
    /// </returns>
    IParseResult<object?> EvaluateValue(
        IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData);
}

/// <summary>
/// Provides extension methods for working with instances of <see cref="IJsonFunction"/>.
/// </summary>
public static class JsonFunctionExtensions
{
    /// <summary>
    /// Attempts to determine the type code of the given JSON function based on its specific implementation.
    /// </summary>
    /// <param name="jsonFunction">
    /// The JSON function whose type code is to be determined. This should be an instance of a class implementing
    /// <see cref="IJsonFunction"/>.
    /// </param>
    /// <returns>
    /// The <see cref="TypeCode"/> corresponding to the type of the JSON function if determinable;
    /// otherwise, returns <c>null</c> if the type cannot be determined.
    /// </returns>
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
