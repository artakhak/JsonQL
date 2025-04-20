namespace JsonQL.Demos.Examples.DataModels;

public interface IEmployee
{
    long Id { get; }
    string FirstName { get; }
    string LastName { get; }
    IAddress Address { get; }
    IManager? Manager { get; }

    List<string> Phones { get; }
}