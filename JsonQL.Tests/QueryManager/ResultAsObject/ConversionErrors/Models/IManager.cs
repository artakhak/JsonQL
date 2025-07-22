namespace JsonQL.Tests.QueryManager.ResultAsObject.ConversionErrors.Models;
public interface IManager : IEmployee
{
    IReadOnlyList<IEmployee> Employees { get; }
}
