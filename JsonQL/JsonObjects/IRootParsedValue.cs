using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonObjects;

/// <summary>
/// An interface for root parsed json. Can be either <see cref="IParsedJson"/> or <see cref="IRootParsedArrayValue"/>.
/// </summary>
public interface IRootParsedValue: IParsedValue
{
    /// <summary>
    /// If value with <see cref="IParsedValue.Id"/> is found in <see cref="IRootParsedValue"/>, returns true, and the value of
    /// <param name="parsedValue"></param> is set to this value.
    /// Otherwise, false is returned, and  <param name="parsedValue"></param> is set to null.
    /// </summary>
    /// <param name="valueId">Value Id.</param>
    /// <param name="parsedValue">Value being looked up by Id.</param>
    bool TryGetParsedValue(Guid valueId, [NotNullWhen(true)]out IParsedValue? parsedValue);

    /// <summary>
    /// Called by json classes, such as <see cref="IParsedJson"/> or <see cref="IParsedArrayValue"/> when values are added.
    /// The <see cref="IRootParsedValue"/> can implement this method to do some internal maintenance of json objects.
    /// </summary>
    /// <param name="parsedValue"></param>
    /// <remarks>
    /// This call should be done for every json object added. In other words, the call is made for parent json<br/>
    /// object as well as for every child object, and the implementation of <see cref="ValueAdded"/> does not have<br/>
    /// to recursively process child json values of <param name="parsedValue"></param>.<br/>
    /// The reasoning behind this is that the call to <see cref="ValueAdded"/> is normally done in json classes that have<br/>
    /// methods for adding child json values (e.g., <see cref="IParsedJson.this[string]"/>,<br/>
    /// <see cref="IParsedArrayValue.AddValueAt"/>, etc.), and the methods to add child json values will be called for every<br/>
    /// child value.
    /// </remarks>
    void ValueAdded(IParsedValue parsedValue);

    /// <summary>
    /// Called by json classes, such as <see cref="IParsedJson"/> or <see cref="IParsedArrayValue"/> when values are removed.
    /// The <see cref="IRootParsedValue"/> can implement this method to do some internal maintenance of json objects.
    /// </summary>
    /// <param name="parsedValue"></param>
    /// <remarks>
    /// This call should be done for every json object removed. In other words, the call is made for parent json<br/>
    /// object as well as for every child object, and the implementation of <see cref="ValueRemoved"/> does not have<br/>
    /// to recursively process child json values of <param name="parsedValue"></param>.<br/>
    /// </remarks>
    void ValueRemoved(IParsedValue parsedValue);
}