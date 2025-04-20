using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonFunction;

[Obsolete("TODO: DELETE IF NOT USED")]
public interface IVariablesStore
{
    IReadOnlyList<IVariableJsonFunction> AllVariables { get; }
    bool TryGetVariable(string variableName, [NotNullWhen(true)] out IVariableJsonFunction? variableJsonFunction);
}

[Obsolete("TODO: DELETE IF NOT USED")]
public class VariablesStore : IVariablesStore
{
    private List<IVariableJsonFunction> _allVariables = new();
    private readonly Dictionary<string, IVariableJsonFunction> _variableNameToVariableMap = new(StringComparer.Ordinal);

    public VariablesStore(IReadOnlyList<IVariableJsonFunction> variableJsonFunctions)
    {
        foreach (var variableJsonFunction in variableJsonFunctions)
        {
            _variableNameToVariableMap[variableJsonFunction.Name] = variableJsonFunction;
            _allVariables.Add(variableJsonFunction);
        }
    }

    /// <inheritdoc />
    public IReadOnlyList<IVariableJsonFunction> AllVariables => _allVariables;

    /// <inheritdoc />
    public bool TryGetVariable(string variableName, [NotNullWhen(true)] out IVariableJsonFunction? variableJsonFunction)
    {
        return _variableNameToVariableMap.TryGetValue(variableName, out variableJsonFunction);
    }
}