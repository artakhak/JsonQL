namespace JsonQL.JsonFunction.JsonFunctions;

public enum JsonFunctionResultType
{
    /// <summary>
    /// Numeric value. 
    /// </summary>
    Number,

    /// <summary>
    /// DateTime value. 
    /// </summary>
    DateTime,

    /// <summary>
    /// Boolean value
    /// </summary>
    Boolean,

    /// <summary>
    /// String value
    /// </summary>
    String,

    /// <summary>
    /// Json object
    /// </summary>
    JsonObject,

    /// <summary>
    /// Json array.
    /// </summary>
    JsonArray,

    /// <summary>
    /// Collection. This is normally result of json path function, such as x.Where(y => y > 10).
    /// </summary>
    Collection,

    /// <summary>
    /// Json null value
    /// </summary>
    JsonNull,

    /// <summary>
    /// Value is missing
    /// </summary>
    Undefined
}