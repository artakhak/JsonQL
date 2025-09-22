using JsonQL.Demos.Examples.DataModels;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.ConversionSettings.SummaryExample;

public class EmployeeWithoutId : Employee
{
    public EmployeeWithoutId() : base(0)
    {
    }
}