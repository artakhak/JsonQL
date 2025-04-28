namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Represents metadata about the type information of items within a collection.
/// Provides details such as the type of the collection item and whether the item is nullable.
/// </summary>
public class CollectionItemTypeData
{
    /// <summary>
    /// Represents metadata for the type of items contained in a collection.
    /// </summary>
    /// <remarks>
    /// Provides details about the type of the collection items and whether they are nullable.
    /// </remarks>
    public CollectionItemTypeData(Type itemType, bool isNullable)
    {
        ItemType = itemType;
        IsNullable = isNullable;
    }

    /// <summary>
    /// Gets the type of the items contained within a collection. This property provides
    /// metadata about the type of the collection's items, which is utilized during
    /// the conversion and processing of JSON data into collection objects.
    /// </summary>
    public Type ItemType { get; }

    /// <summary>
    /// Indicates whether the items contained within a collection are nullable. This property is utilized
    /// to handle type information during the conversion of JSON data to object collections, ensuring proper
    /// validation and error handling for nullable and non-nullable collection items.
    /// </summary>
    public bool IsNullable { get; }
}