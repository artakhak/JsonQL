using OROptimizer.Diagnostics.Log;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Provides functionality to manage variable resolvers and resolve variable values
/// within an evaluation context.
/// </summary>
public interface IVariablesManager: IResolvesVariableValue
{
    /// <summary>
    /// Registers a variable resolver for use in variable value resolution.
    /// </summary>
    /// <param name="resolvesVariableValue">The variable resolver to register.</param>
    void Register(IResolvesVariableValue resolvesVariableValue);

    /// <summary>
    /// Unregisters a variable resolver, removing it from the list of registered resolvers.
    /// </summary>
    /// <param name="resolvesVariableValue">The variable resolver to unregister.</param>
    void UnRegister(IResolvesVariableValue resolvesVariableValue);
}

/// <inheritdoc />
public class VariablesManager : IVariablesManager
{
    private readonly List<IResolvesVariableValue> _variableResolvers = new();

    /// <inheritdoc />
    public IParseResult<object?>? TryEvaluateVariableValue(string variableName, IJsonFunctionEvaluationContextData? contextData)
    {
        for (var i = _variableResolvers.Count - 1; i >= 0; --i)
        {
            var variableResolver = _variableResolvers[i];

            var result = variableResolver.TryEvaluateVariableValue(variableName, contextData);
            if (result == null)
                continue;

            return result;
        }

        return null;
    }

    /// <inheritdoc />
    public void Register(IResolvesVariableValue resolvesVariableValue)
    {
        _variableResolvers.Add(resolvesVariableValue);
    }

    /// <inheritdoc />
    public void UnRegister(IResolvesVariableValue resolvesVariableValue)
    {
        if (_variableResolvers.Last() == resolvesVariableValue)
        {
            _variableResolvers.RemoveAt(_variableResolvers.Count - 1);
            return;
        }

        var indexOfResolver = _variableResolvers.IndexOf(resolvesVariableValue);

        if (indexOfResolver >= 0)
        {
            ThreadStaticLogging.Log.ErrorFormat("Variable resolver [{0}] of type [{1}] is not at the top of stack when unregistered!",
                resolvesVariableValue, resolvesVariableValue.GetType());

            _variableResolvers.RemoveAt(indexOfResolver);
            return;
        }

        ThreadStaticLogging.Log.ErrorFormat("Variable resolver [{0}] of type [{1}] was not registered!", 
            resolvesVariableValue, resolvesVariableValue.GetType());
    }
}