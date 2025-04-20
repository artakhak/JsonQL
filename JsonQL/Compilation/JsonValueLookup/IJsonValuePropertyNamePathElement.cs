namespace JsonQL.Compilation.JsonValueLookup;


public interface IJsonValuePropertyNamePathElement : IJsonValuePathElement
{
    string Name { get; }
}