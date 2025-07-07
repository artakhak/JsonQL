// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.JsonObjects.JsonPath;
using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonObjects;

/// <summary>
/// Base class for implementations of <see cref="IParsedArrayValue"/>.
/// </summary>
public abstract class ParsedArrayValueAbstr : ParsedValueAbstr, IParsedArrayValue
{
    private readonly IParsedJsonVisitor _parsedJsonVisitor;
    private readonly Dictionary<Guid, int> _valueIdToParsedValueIndexMap = new();
    private readonly List<IParsedValue> _values = new();

    /// <summary>
    /// Serves as an abstract base class for parsed JSON array values, providing a framework for implementing
    /// functionality to handle and manipulate multiple parsed values within a JSON array structure.
    /// </summary>
    protected ParsedArrayValueAbstr(IParsedJsonVisitor parsedJsonVisitor, IParsedValue? parentJsonValue, IJsonKeyValue? jsonKeyValue,
        IJsonPath? pathInReferencedJson) : base(parentJsonValue, jsonKeyValue, pathInReferencedJson)
    {
        _parsedJsonVisitor = parsedJsonVisitor;
    }

    /// <inheritdoc />
    public IReadOnlyList<IParsedValue> Values => _values;

    /// <inheritdoc />
    public bool TryGetValueIndex(Guid valueId, [NotNullWhen(true)] out int? index)
    {
        index = null;
        if (!_valueIdToParsedValueIndexMap.TryGetValue(valueId, out var index2))
            return false;

        index = index2;
        return true;
    }

    /// <inheritdoc />
    public void AddValueAt(int index, IParsedValue parsedValue)
    {
        if (index < 0 || index > _values.Count)
            throw new IndexOutOfRangeException($"Invalid index [{index}]. Expected value between 0 and [{_values.Count}] ");

        _valueIdToParsedValueIndexMap[parsedValue.Id] = index;

        for (var i = index; i < _values.Count; ++i)
        {
            // Lets modify indexes of all values right to inserted position.
            var value = _values[i];
            _valueIdToParsedValueIndexMap[value.Id] = i + 1;
        }

        _values.Insert(index, parsedValue);
        this.RootParsedValue.ValueAdded(parsedValue);
    }

    /// <inheritdoc />
    public void RemoveValueAt(int index)
    {
        if (index < 0 || index >= _values.Count)
            throw new IndexOutOfRangeException($"Invalid index [{index}]. Expected value between 0 and [{_values.Count - 1}] ");

        var removedValue = _values[index];

        for (var i = index + 1; i < _values.Count; ++i)
        {
            // Lets modify indexes of all values right to removed position.
            var value = _values[i];
            _valueIdToParsedValueIndexMap[value.Id] = i - 1;
        }

        _valueIdToParsedValueIndexMap.Remove(removedValue.Id);
        _values.RemoveAt(index);

        _parsedJsonVisitor.Visit(removedValue, visitedJsonValue =>
        {
            this.RootParsedValue.ValueRemoved(visitedJsonValue);
            return true;
        });
    }
}