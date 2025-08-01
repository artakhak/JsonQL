namespace JsonQL.Demos.Examples.DataModels;

public interface IEmployee
{
    long Id { get; }
    string FirstName { get; }
    string LastName { get; }
    IAddress Address { get; }
    int Salary { get; }
    int Age { get; }
    IManager? Manager { get; }

    List<string> Phones { get; }
}
