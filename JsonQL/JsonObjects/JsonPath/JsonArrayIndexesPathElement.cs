namespace JsonQL.JsonObjects.JsonPath;

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