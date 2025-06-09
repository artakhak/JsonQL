namespace JsonQL.JsonToObjectConversion.ConvertedObjectPath;

/// <summary>
/// An abstract base class representing a value selector element in a converted object path.
/// This class is used as part of the JSON to object conversion process, where such elements
/// play a role in identifying and mapping specific values within the object path.
/// </summary>
public abstract class ConvertedObjectPathValueSelectorElementAbstr : ConvertedObjectPathElementAbstr, IConvertedObjectPathValueSelectorElement
{
    /// <summary>
    /// Defines the abstract base class for elements that select specific values
    /// in a converted object path during the JSON-to-object conversion process.
    /// Implements <see cref="IConvertedObjectPathValueSelectorElement"/> to signify
    /// it can function as a value selector in the object path structure.
    /// </summary>
    protected ConvertedObjectPathValueSelectorElementAbstr(string name, Type objectType) : base(name, objectType)
    {
    }

    /// <inheritdoc />
    public abstract IConvertedObjectPathValueSelectorElement Clone();
}