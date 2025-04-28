namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Represents an error indicating that a non-nullable property was not set during the JSON-to-object conversion.
/// </summary>
public enum ConversionErrorType
{
    /// <summary>
    /// Represents a general error encountered during the JSON-to-object conversion process.
    /// This error signifies an unspecified issue that does not fall into more specific error categories.
    /// </summary>
    Error,

    /// <summary>
    /// Represents an error that occurs when an instance of a class cannot be created during the JSON-to-object conversion process.
    /// This error might arise when the target class lacks an accessible parameterless constructor or when an exception
    /// is thrown during the instantiation process.
    /// </summary>
    CannotCreateInstanceOfClass,

    /// <summary>
    /// Indicates an error during the JSON-to-object conversion process where a required value
    /// was expected but was not provided or set in the JSON data.
    /// </summary>
    ValueNotSet,

    /// <summary>
    /// Represents an error that occurs when a non-nullable property is not set during the JSON-to-object conversion process.
    /// This error indicates that the conversion process failed to assign a value to a property that must not accept null values.
    /// </summary>
    NonNullablePropertyNotSet,

    /// <summary>
    /// Represents an error indicating that a non-nullable collection contains an item whose value was not set
    /// during the JSON-to-object conversion process.
    /// This error occurs when the conversion expects each item in the collection to have a value,
    /// but an unset or null value is encountered.
    /// </summary>
    NonNullableCollectionItemValueNotSet,

    /// <summary>
    /// Indicates an error that occurs when a JSON value cannot be converted to the expected .NET type.
    /// This error typically arises when the type of the value in the JSON does not match the expected type
    /// during the JSON-to-object conversion process.
    /// </summary>
    FailedToConvertJsonValueToExpectedType
}