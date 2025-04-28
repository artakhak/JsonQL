namespace JsonQL.JsonObjects.JsonPath;

/// <summary>
/// Represents an interface for JSON path elements that specify array indexes.
/// Provides a contract to access the list of indexes targeting specific elements within a JSON array.
/// </summary>
public interface IJsonArrayIndexesPathElement : IJsonPathElement
{
    /// <summary>
    /// Gets the list of indexes in a JSON array path element.
    /// Provides access to the specific array positions targeted by this path element.
    /// </summary>
    IReadOnlyList<int> Indexes { get; }
}

/// <inheritdoc />
public class JsonArrayIndexesPathElement : IJsonArrayIndexesPathElement
{
    /// <summary>
    /// Represents a path element that specifies indexes in a JSON array.
    /// </summary>
    public JsonArrayIndexesPathElement(IReadOnlyList<int> indexes)
    {
        Indexes = indexes;
    }

    /// <inheritdoc />
    public IReadOnlyList<int> Indexes { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"[{string.Join(',', Indexes)}]";
    }

    /// <inheritdoc />
    public bool Equals(IJsonPathElement jsonPathElement)
    {
        if (jsonPathElement is not JsonArrayIndexesPathElement jsonArrayIndexesPathElement)
            return false;

        if (this.Indexes.Count != jsonArrayIndexesPathElement.Indexes.Count)
            return false;

        for (var i = 0; i < this.Indexes.Count; ++i)
        {
            if (Indexes[i] != jsonArrayIndexesPathElement.Indexes[i])
                return false;
        }

        return true;
    }
}