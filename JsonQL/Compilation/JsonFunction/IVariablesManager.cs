namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Provides functionality to manage variable resolvers and resolve variable values
/// within an evaluation context.
/// </summary>
public interface IVariablesManager
{
    /// <summary>
    /// Registers a variable resolver for use in variable value resolution.
    /// </summary>
    /// <param name="resolvesVariableValue">The variable resolver to register.</param>
    void Register(IResolvesVariableValue resolvesVariableValue);

    /// <summary>
    /// Register variable value <paramref name="variableValue"/> to be resolved for variable name <paramref name="variableName"/>.
    /// </summary>
    /// <param name="resolvesVariableValue">Variable resolver.</param>
    /// <param name="variableName">Variable name.</param>
    /// <param name="variableValue">Variable value.</param>
    void RegisterVariableValue(IResolvesVariableValue resolvesVariableValue, string variableName, object? variableValue);

    /// <summary>
    /// Unregisters variable value for variable name <paramref name="variableName"/>.
    /// </summary>
    /// <param name="resolvesVariableValue">Variable resolver.</param>
    /// <param name="variableName">Variable name.</param>
    void UnregisterVariableValue(IResolvesVariableValue resolvesVariableValue, string variableName);

    /// <summary>
    /// Tries to resolve variable value for variable name <paramref name="variableName"/>.
    /// </summary>
    /// <param name="variableName">Variable name.</param>
    /// <returns>An instance of <see cref="IParseResult{TValue}"/> containing the resolved value, or null if resolution fails.</returns>
    IParseResult<object?>? TryResolveVariableValue(string variableName);

    /// <summary>
    /// Unregisters a variable resolver, removing it from the list of registered resolvers.
    /// </summary>
    /// <param name="resolvesVariableValue">The variable resolver to unregister.</param>
    void UnRegister(IResolvesVariableValue resolvesVariableValue);
}

/// <inheritdoc />
public class VariablesManager : IVariablesManager
{
    private readonly List<VariablesResolvedData> _variableResolvers = new();
    private readonly Dictionary<IResolvesVariableValue, VariablesResolvedData> _variableResolverToVariablesResolvedDataMap = new();

    /// <inheritdoc />
    public void Register(IResolvesVariableValue resolvesVariableValue)
    {
        var variablesResolvedData = new VariablesResolvedData(resolvesVariableValue);
        _variableResolvers.Add(variablesResolvedData);
        _variableResolverToVariablesResolvedDataMap[resolvesVariableValue] = variablesResolvedData;
    }

    /// <inheritdoc />
    public void UnRegister(IResolvesVariableValue resolvesVariableValue)
    {
        if (_variableResolvers.Count > 0)
        {
            _variableResolverToVariablesResolvedDataMap.Remove(resolvesVariableValue);

            if (_variableResolvers.Last().ResolvesVariableValue == resolvesVariableValue)
            {
                _variableResolvers.RemoveAt(_variableResolvers.Count - 1);
                return;
            }

            for (var i = 0; i < _variableResolvers.Count; ++i)
            {
                var variablesResolvedData = _variableResolvers[i];

                if (variablesResolvedData.ResolvesVariableValue == resolvesVariableValue)
                {
                    _variableResolvers.RemoveAt(i);
                    return;
                }
            }
        }

        ThreadStaticLoggingContext.Context.ErrorFormat("Variable resolver [{0}] of type [{1}] was not registered!",
            resolvesVariableValue, resolvesVariableValue.GetType());
    }

    /// <inheritdoc />
    public void RegisterVariableValue(IResolvesVariableValue resolvesVariableValue, string variableName, object? variableValue)
    {
        if (!_variableResolverToVariablesResolvedDataMap.TryGetValue(resolvesVariableValue, out var variablesResolvedData))
            return;

        variablesResolvedData.RegisterVariableValue(variableName, variableValue);
    }

    /// <inheritdoc />
    public void UnregisterVariableValue(IResolvesVariableValue resolvesVariableValue, string variableName)
    {
        if (!_variableResolverToVariablesResolvedDataMap.TryGetValue(resolvesVariableValue, out var variablesResolvedData))
            return;

        variablesResolvedData.UnregisterVariableValue(variableName);
    }

    /// <inheritdoc />
    public IParseResult<object?>? TryResolveVariableValue(string variableName)
    {
        for (var i = _variableResolvers.Count - 1; i >= 0; --i)
        {
            if (_variableResolvers[i].VariableNameToResolvedValueMap.TryGetValue(variableName, out var variableValue))
            {
                return new ParseResult<object?>(variableValue);
            }
        }

        return null;
    }

    private class VariablesResolvedData
    {
        private readonly Dictionary<string, object?> _variableNameToResolvedValueMap = new();
        public VariablesResolvedData(IResolvesVariableValue resolvesVariableValue)
        {
            ResolvesVariableValue = resolvesVariableValue;
        }

        public IResolvesVariableValue ResolvesVariableValue { get; }

        public void RegisterVariableValue(string variableName, object? variableValue)
        {
            _variableNameToResolvedValueMap[variableName] = variableValue;
        }

        public void UnregisterVariableValue(string variableName)
        {
            _variableNameToResolvedValueMap.Remove(variableName);
        }

        public IReadOnlyDictionary<string, object?> VariableNameToResolvedValueMap => _variableNameToResolvedValueMap;
    }
}