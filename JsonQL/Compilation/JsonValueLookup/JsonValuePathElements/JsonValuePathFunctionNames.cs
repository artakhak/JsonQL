namespace JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;

/// <summary>
/// Provides a set of constant names used to represent various JSON value path functions.
/// </summary>
/// <remarks>
/// These functions are utilized in JSON query language (JsonQL) to interact with and manipulate JSON structures.
/// The constants defined in this class are referenced by other components in the system to ensure standardization
/// and avoid hardcoding function names throughout the codebase.
/// </remarks>
public static class JsonValuePathFunctionNames
{
    /// <summary>
    /// Represents the function name used to identify the "Flatten" operation
    /// in JSON value path selection. This selector function is utilized to
    /// flatten nested collections into a single, flat collection during JSON
    /// value path resolution.
    /// </summary>
    public const string FlattenCollectionItemsSelectorFunction = "Flatten";

    /// <summary>
    /// Represents the function name used to identify the "Reverse" operation
    /// in JSON value path selection. This selector function facilitates
    /// reversing the order of items in a collection during JSON
    /// value path resolution.
    /// </summary>
    public const string ReverseCollectionItemsSelectorFunction = "Reverse";

    /// <summary>
    /// Represents the function name used to identify the "Where" operation
    /// in JSON value path selection. This function applies a provided predicate
    /// to filter collection items, selecting only those that satisfy the predicate
    /// during JSON value path resolution.
    /// </summary>
    public const string WhereCollectionItemsFunction = "Where";

    /// <summary>
    /// Represents the function name used to identify the "Select" operation
    /// in JSON value path selection. This selector function is used to project
    /// or map elements within a collection based on a specified JSON path lambda function.
    /// </summary>
    public const string SelectCollectionItemsFunction = "Select";

    /// <summary>
    /// Represents the function name used to identify the selection of the first item
    /// in a collection during JSON value path resolution. This selector function
    /// is utilized to extract the first item from a collection while navigating
    /// JSON structures.
    /// </summary>
    public const string FirstCollectionItemSelectorFunction = "First";

    /// <summary>
    /// Represents the function name used to identify the "Last" operation
    /// in JSON value path selection. This function is utilized to resolve and
    /// select the last item within a collection during JSON value path execution.
    /// </summary>
    public const string LastCollectionItemSelectorFunction = "Last";

    /// <summary>
    /// Represents the function name used to select a single item from a collection
    /// during JSON value path evaluation. This selector function identifies an
    /// operation that targets and retrieves a specific item within a collection
    /// based on its position or identifier.
    /// </summary>
    public const string CollectionItemSelectorFunction = "At";

    /// <summary>
    /// Represents the function name used to reference a parent file
    /// in JSON value path resolution. This function allows navigation
    /// to the parent JSON object or context relative to the current one.
    /// </summary>
    public const string ParentFile = "parent";

    /// <summary>
    /// Represents a predefined function name used to reference the current
    /// JSON context or object within a JSON value path. This allows users to
    /// access the current scope's data when resolving paths in JSON value
    /// lookups.
    /// </summary>
    public const string ThisJson = "this";
}