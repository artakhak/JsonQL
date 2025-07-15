// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Diagnostics.CodeAnalysis;

namespace JsonQL.Compilation.JsonFunction;
// TODO: Delete in  next release if not used.
[Obsolete("TODO: DELETE IF NOT USED")]
internal interface IVariablesStore
{
    IReadOnlyList<IVariableJsonFunction> AllVariables { get; }
    bool TryGetVariable(string variableName, [NotNullWhen(true)] out IVariableJsonFunction? variableJsonFunction);
}

[Obsolete("TODO: DELETE IF NOT USED")]
internal class VariablesStore : IVariablesStore
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