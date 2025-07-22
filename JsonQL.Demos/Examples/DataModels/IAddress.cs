namespace JsonQL.Demos.Examples.DataModels;

public interface IAddress
{
    string Street { get; }
    string City { get; }
    string State { get; }
    string ZipCode { get; }

    // Example of nullable value that does not have to be set.
    string? County { get; }
}
