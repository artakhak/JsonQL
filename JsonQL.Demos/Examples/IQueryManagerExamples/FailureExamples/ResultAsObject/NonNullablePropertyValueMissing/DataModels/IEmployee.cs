using JsonQL.Demos.Examples.DataModels;

namespace JsonQL.Demos.Examples.IQueryManagerExamples.FailureExamples.ResultAsObject.NonNullablePropertyValueMissing.DataModels;

public interface IEmployee
{
    long Id { get; }
    string FirstName { get; }
    string LastName { get; }
    IAddress Address { get; }
    int Salary { get; }
    int Age { get; }
    List<string>? Phones { get; }
}
