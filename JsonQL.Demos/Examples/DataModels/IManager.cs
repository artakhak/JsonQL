namespace JsonQL.Demos.Examples.DataModels;

public interface IManager : IEmployee
{
    IReadOnlyList<IEmployee> Employees { get; }
}
