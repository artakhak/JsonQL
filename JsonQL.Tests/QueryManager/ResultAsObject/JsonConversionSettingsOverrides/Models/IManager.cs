namespace JsonQL.Tests.QueryManager.ResultAsObject.JsonConversionSettingsOverrides.Models;
public interface IManager : IEmployee
{
    IReadOnlyList<IEmployee> Employees { get; }
}