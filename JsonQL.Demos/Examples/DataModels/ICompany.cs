namespace JsonQL.Demos.Examples.DataModels;

public interface ICompany
{
    string Name { get; }
    IReadOnlyList<IEmployee> Employees { get; }
}
