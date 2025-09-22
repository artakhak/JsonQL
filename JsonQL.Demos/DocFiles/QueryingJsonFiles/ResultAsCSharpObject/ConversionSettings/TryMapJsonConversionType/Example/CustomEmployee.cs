using JsonQL.Demos.Examples.DataModels;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.ConversionSettings.TryMapJsonConversionType.Example;

public class CustomEmployee: Employee
{
    public CustomEmployee(long id) : base(id)
    {
        LogHelper.Context.Log.InfoFormat("Employee of type {0} with id={1} was created.",
            this.GetType().FullName, id);
    }
}