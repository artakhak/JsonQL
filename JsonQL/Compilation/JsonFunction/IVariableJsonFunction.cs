namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// A json function used to parse lambda expressions parameters or variables that have any meaning in the context of
/// currently evaluated function.
/// </summary>
public interface IVariableJsonFunction: IJsonFunction
{
    string Name { get; }
}