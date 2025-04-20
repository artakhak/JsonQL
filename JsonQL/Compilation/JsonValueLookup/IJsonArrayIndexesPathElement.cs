namespace JsonQL.Compilation.JsonValueLookup;

public interface IJsonArrayIndexesPathElement: IJsonValuePathElement
{
    /// <summary>
    /// Indexes.<br/>
    /// The items are currently of type <see cref="IJsonArrayIndexInfo"/> for numeric index value.
    /// </summary>
    IReadOnlyList<IJsonArrayIndexInfo> ArrayIndexes { get; }
}
