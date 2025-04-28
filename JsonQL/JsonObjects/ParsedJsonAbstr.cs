using System.Diagnostics.CodeAnalysis;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.JsonObjects;

/// <summary>
/// Base class for <see cref="IParsedJson"/> implementations.
/// </summary>
public abstract class ParsedJsonAbstr : ParsedValueAbstr, IParsedJson
{
    private readonly IParsedJsonVisitor _parsedJsonVisitor;
    private readonly List<IJsonKeyValue> _keyValues = new();
    private readonly Dictionary<string, IJsonKeyValue> _keyToJsonKeyValueMap = new(StringComparer.Ordinal);

    /// <summary>
    /// Represents an abstract base class for JSON objects parsed from a JSON structure.
    /// Implements the <see cref="IParsedJson"/> interface and serves as a foundation
    /// for classes representing specific types of parsed JSON objects.
    /// </summary>
    /// <remarks>
    /// This class provides common properties and methods to interact with key-value pairs
    /// contained within a parsed JSON object. It also supports traversal and visiting
    /// of the parsed JSON structure using an <see cref="IParsedJsonVisitor"/>.
    /// </remarks>
    protected ParsedJsonAbstr(IParsedJsonVisitor parsedJsonVisitor, IParsedValue? parentJsonValue, IJsonKeyValue? jsonKeyValue) : base(parentJsonValue, jsonKeyValue)
    {
        _parsedJsonVisitor = parsedJsonVisitor;
    }

    /// <inheritdoc />
    public IReadOnlyList<IJsonKeyValue> KeyValues => _keyValues;

    /// <inheritdoc />
    public bool HasKey(string key)
    {
        return _keyToJsonKeyValueMap.ContainsKey(key);
    }

    /// <inheritdoc />
    public IJsonKeyValue this[string key, int? index = null]
    {
        get => _keyToJsonKeyValueMap[key];
        set
        {
            if (_keyToJsonKeyValueMap.TryGetValue(key, out var currentValue))
            {
                var currentValueIndex = _keyValues.IndexOf(currentValue);

                if (currentValueIndex >= 0)
                {
                    _keyValues[currentValueIndex] = value;

                    _parsedJsonVisitor.Visit(currentValue.Value, visitedJsonValue =>
                    {
                        this.RootParsedValue.ValueRemoved(visitedJsonValue);
                        return true;
                    });
                }
                else
                {
                    _keyValues.Add(value);
                    this.RootParsedValue.ValueAdded(value.Value);
                    ThreadStaticLogging.Log.ErrorFormat("Key [{0}] is missing in [{1}]", key, nameof(_keyValues));
                }
            }
            else
            {
                if (index != null && index >= 0 && index < _keyValues.Count)
                {
                    _keyValues.Insert(index.Value, value);
                }
                else
                {
                    _keyValues.Add(value);
                }
               
                this.RootParsedValue.ValueAdded(value.Value);
            }

            _keyToJsonKeyValueMap[key] = value;
        }
    }

    /// <inheritdoc />
    public bool TryGetJsonKeyValue(string key, [NotNullWhen(true)] out IJsonKeyValue? jsonKeyValue)
    {
        return _keyToJsonKeyValueMap.TryGetValue(key, out jsonKeyValue);
    }

    /// <inheritdoc />
    public bool TryRemoveKey(string key, [NotNullWhen(true)] out IJsonKeyValue? jsonKeyValue)
    {
        if (!_keyToJsonKeyValueMap.Remove(key, out jsonKeyValue))
        {
            return false;
        }

        var currentValueIndex = _keyValues.IndexOf(jsonKeyValue);

        if (currentValueIndex >= 0)
        {
            _keyValues.RemoveAt(currentValueIndex);
        }
        else
        {
            ThreadStaticLogging.Log.ErrorFormat("Key [{0}] is missing in [{1}]", key, nameof(_keyValues));
        }

        _keyToJsonKeyValueMap.Remove(key);
        return true;
    }
}