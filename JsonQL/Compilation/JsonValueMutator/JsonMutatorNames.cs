namespace JsonQL.Compilation.JsonValueMutator;

/// <summary>
/// Provides a set of predefined keywords used for JSON mutation operations within the JSONQL framework.
/// </summary>
/// <remarks>
/// The class contains constants that define special keywords recognized by the JSON value mutation system.
/// These keywords are used for operations such as merging JSON object fields, merging array items, and
/// calculating JSON object values dynamically. These constants serve to ensure uniformity and prevent
/// hardcoded strings throughout the mutation logic.
/// </remarks>
/// <example>
/// This class is used in conjunction with JSON mutation logic to interpret special directives
/// in JSON data structures.
/// </example>
public static class JsonMutatorKeywords
{
    /// <summary>
    /// Represents the keyword used to specify the copying of fields from one JSON object
    /// to another during JSON mutation. This keyword is utilized in JSON mutator functions
    /// to merge fields from a source JSON into a target JSON object efficiently.
    /// </summary>
    /// <remarks>
    /// The <c>MergedJsonObjectFields</c> keyword is implemented as a constant string with
    /// the value "$copyFields". It is intended to be used in JSON key-value pairs, typically
    /// for scenarios where partial JSON structures need to be combined or merged.
    /// </remarks>
    /// <example>
    /// When using this keyword in a JSON object, ensure that the source value being referenced
    /// is a valid JSON object (collection of key-value pairs). The mutator will attempt to
    /// inject the fields from the source object into the current object, maintaining its
    /// integrity and generating errors if unexpected conditions are encountered.
    /// </example>
    public const string MergedJsonObjectFields = "$copyFields";

    /// <summary>
    /// Represents the keyword used to specify the merging of array items from one array
    /// into another during JSON mutation. This keyword is designed to facilitate combining
    /// or appending elements of multiple JSON arrays into a single target array structure.
    /// </summary>
    /// <remarks>
    /// The <c>MergedArrayItems</c> keyword is implemented as a constant string with the value "$merge".
    /// It is primarily intended for use in JSON arrays where array elements need to be merged or
    /// extended into another array. This functionality is particularly helpful when constructing or
    /// transforming JSON data dynamically by consolidating array contents.
    /// </remarks>
    public const string MergedArrayItems = "$merge";

    /// <summary>
    /// Represents the keyword used to define a dynamically calculated value within a JSON object
    /// during JSON mutation. This keyword indicates that the value is not static but instead
    /// computed or derived based on a specific mutator function or expression.
    /// </summary>
    /// <remarks>
    /// The <c>CalculatedJsonObjectValue</c> keyword is implemented as a constant string with
    /// the value "$value". It is employed in scenarios where JSON objects require values to
    /// be evaluated or resolved at runtime, often based on additional input or computation logic.
    /// This ensures flexibility and dynamic behavior in JSON transformations.
    /// </remarks>
    public const string CalculatedJsonObjectValue = "$value";
}