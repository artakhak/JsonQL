namespace JsonQL.JsonToObjectConversion.ConvertedObjectPath;

/// Factory interface for creating instances of IIndexConvertedObjectPathElement.
/// Provides functionality to generate path elements representing indexed values in a converted object path.
public interface IIndexConvertedObjectPathElementFactory
{
    /// <summary>
    /// Creates an instance of <see cref="IIndexConvertedObjectPathElement"/> that represents an indexed value
    /// in a converted object path using the specified index and type information.
    /// </summary>
    /// <param name="itemIndex">The index of the item within the collection or array being referenced.</param>
    /// <param name="itemType">The type of the item at the specified index.</param>
    /// <returns>An instance of <see cref="IIndexConvertedObjectPathElement"/> representing the indexed value.</returns>
    IIndexConvertedObjectPathElement Create(int itemIndex, Type itemType);
}

/// <inheritdoc />
public class IndexConvertedObjectPathElementFactory : IIndexConvertedObjectPathElementFactory
{
    /// <inheritdoc />
    public IIndexConvertedObjectPathElement Create(int itemIndex, Type itemType)
    {
        return new IndexConvertedObjectPathElement(itemIndex, itemType);
    }
}