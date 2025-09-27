using JsonQL.Demos.Examples.DataModels;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.ConversionSettings.SummaryExample;

public class ManagerWithoutEmployees : Manager
{
    public ManagerWithoutEmployees(long id) : base(id)
    {
        Employees = [];
    }
}
