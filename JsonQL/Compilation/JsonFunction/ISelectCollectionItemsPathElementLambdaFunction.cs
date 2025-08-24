// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.JsonFunctions;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Represents a specialized lambda function used with "Select" clause. Example of usage of this lambda is "e => e.Salary * 0.1" in<br/>
/// $value(parent.Object1.Companies.Select(e => e.Salary * 0.1)).
/// </summary>
/// <remarks>
/// This interface is typically used in scenarios where JSON path syntax and lambda functions
/// are combined to filter, map, or process JSON data based on dynamic criteria defined at runtime.
/// </remarks>
public interface ISelectCollectionItemsPathElementLambdaFunction: ILambdaFunction<IEvaluatesJsonValuePathLookupResult>
{
    
}

/// <inheritdoc />
public class SelectCollectionItemsPathElementLambdaFunction : ISelectCollectionItemsPathElementLambdaFunction
{
    /// <summary>
    /// Represents a lambda function that combines a parameter JSON function
    /// and a JSON value path function to evaluate JSON expressions
    /// against a specified value path in the JSON structure.
    /// </summary>
    public SelectCollectionItemsPathElementLambdaFunction(ILambdaFunctionParameterJsonFunction parameterJsonFunction, IEvaluatesJsonValuePathLookupResult jsonValuePathJsonFunction)
    {
        ParameterJsonFunction = parameterJsonFunction;
        LambdaExpressionFunction = jsonValuePathJsonFunction;
    }

    /// <inheritdoc />
    public ILambdaFunctionParameterJsonFunction ParameterJsonFunction { get; }

    /// <inheritdoc />
    public IEvaluatesJsonValuePathLookupResult LambdaExpressionFunction { get; }
}