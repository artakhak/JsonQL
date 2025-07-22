namespace JsonQL.Tests.QueryManager.ResultAsObject.Models;

public interface IManager : IEmployee
{
    IReadOnlyList<IEmployee> Employees { get; }
}
