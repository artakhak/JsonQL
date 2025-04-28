namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// A JSON function used to parse lambda expressions parameters or variables that have any meaning in the context of
/// currently evaluated function.
/// </summary>
public interface IVariableJsonFunction: IJsonFunction
{
    /// <summary>
    /// Gets the name of the variable or parameter as interpreted within the context of the JSON function.
    /// </summary>
    string Name { get; }
}