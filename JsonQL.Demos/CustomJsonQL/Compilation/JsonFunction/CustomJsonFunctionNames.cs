namespace JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction;

public static class CustomJsonFunctionNames
{
    public const string ReverseTextAndAddMarkers = "ReverseTextAndAddMarkers";

    /// <summary>
    /// Note, we could use a better example of special function, such as Now, however
    /// it might result in conflicts with core (non-custom) functions in the future,
    /// is similar function in introduced in jsonQL library.
    /// Therefore, to avoid future conflicts, impractical examples are used. 
    /// </summary>
    public const string JsonQLReleaseDateFunction = "JsonQLReleaseDate";
}