namespace JsonQL.Tests.QueryManager.ResultAsObject.JsonConversionSettingsOverrides.Models;

public class EmployeeWithSsn : Employee
{
    public EmployeeWithSsn(long id) : base(id)
    {
    }

    public string Ssn { get; set; } = null!;
}