// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Represents a context for evaluating JSON function values, providing access to
/// necessary functionality and state such as variable management and parent functions.
/// </summary>
public interface IJsonFunctionValueEvaluationContext
{
    /// <summary>
    /// Gets the parent JSON function used in the current context, allowing hierarchical
    /// evaluation of functions. This property is assigned later and should not be accessed
    /// in constructors where the instance of this class is injected.
    /// </summary>
    /// <remarks>
    /// The value is set later. Therefore, do not use this value in constructors of classes where
    /// this class instance is injected.
    /// The value can be used when evaluating function value. For example in <see cref="IJsonFunction.EvaluateValue"/>.
    /// </remarks>
    IJsonFunction? ParentJsonFunction { get; }

    /// <summary>
    /// Represents the manager responsible for resolving or evaluating variable values
    /// within a given context. This property is essential for handling dynamic variable
    /// values and binding them during the evaluation of JSON functions.
    /// </summary>
    /// <remarks>
    /// The functionality provided by this property is heavily utilized in the context
    /// of JSON function execution, where variable values need to be resolved or evaluated.
    /// It is typically used during function execution to fetch or calculate the runtime values
    /// of variables based on their identifiers or other context-specific logic.
    /// </remarks>
    IVariablesManager VariablesManager { get; }
}

/// <inheritdoc />
public class JsonFunctionValueEvaluationContext : IJsonFunctionValueEvaluationContext
{
    //private IJsonFunction? _parentJsonFunction;

    public JsonFunctionValueEvaluationContext(IVariablesManager variablesManager)
    {
        VariablesManager = variablesManager;
    }

    /// <inheritdoc />
    public IJsonFunction? ParentJsonFunction { get; set; }

    /// <inheritdoc />
    public IVariablesManager VariablesManager { get; }
}
