namespace JsonQL.JsonToObjectConversion.ConvertedObjectPath;

/// <summary>
/// Defines an element in the converted object path that references
/// an indexed value. This interface is used in JSON-to-object
/// conversion processes to represent and identify elements by their
/// index within a collection or array structure.
/// </summary>
public interface IIndexConvertedObjectPathElement : IConvertedObjectPathValueSelectorElement
{
    
}

/// <summary>
/// Represents an element in the converted object path specifically identified
/// by an index within a collection or array structure. This class facilitates
/// the identification and mapping of values associated with the specified index
/// during JSON-to-object conversion processes.
/// </summary>
public class IndexConvertedObjectPathElement : ConvertedObjectPathValueSelectorElementAbstr, IIndexConvertedObjectPathElement
{
    private readonly int _itemIndex;

    /// <summary>
    /// Represents an element in the converted object path identified by a specific index
    /// within a collection or array structure. Used in scenarios involving JSON-to-object
    /// conversion to target values with a particular index.
    /// </summary>
    public IndexConvertedObjectPathElement(int itemIndex, Type itemType) : base(itemIndex.ToString(), itemType)
    {
        _itemIndex = itemIndex;
    }

    /// <inheritdoc />
    public override IConvertedObjectPathValueSelectorElement Clone()
    {
        return new IndexConvertedObjectPathElement(_itemIndex, this.ObjectType);
    }
}