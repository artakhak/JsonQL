using JsonQL.Compilation;
using JsonQL.Demos.Examples.DataModels;
using JsonQL.JsonToObjectConversion;
using JsonQL.Query;

namespace JsonQL.Demos.Examples.IQueryManagerExamples.SuccessExamples.ResultAsObject.ResultAsNonNullableEmployeesList;

public class Example : QueryObjectExampleManagerForSuccessAbstr<IReadOnlyList<IEmployee>>
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IObjectQueryResult<IReadOnlyList<IEmployee>> QueryObject()
    {
        // NOTE: Data.json has a root JSON with a collection of employees. 
        // If the JSON had a JSON object with the "Employees" field, the
        // query would be: "Employees.Where(...)" instead of "Where(...)"
        var query = "Where(e => e.Id==100000006 || e.Id==100000007 || Any(EmployeeIds, p => p == e.Id))";

        // We can call _queryManager.QueryObject<T> with the following values for "T" generic parameter
        // -Class (value or reference type). We can use '?' for nullable values. Examples:
        //      "_queryManager.QueryObject<Manager?>(...)",
        //      "_queryManager.QueryObject<Manager>(...)"
        // -Interface. We can use '?' for nullable values. Examples:
        //      "_queryManager.QueryObject<IManager?>(...)",
        //      "_queryManager.QueryObject<IManager>(...)"
        // The following collection types:
        //          IReadOnlyList<T>, IEnumerable<T>, IList<T>, 
        //          ICollection<T>, IReadOnlyCollection<T>
        // -Any type that implements ICollection<T>. Example: List<T>, Array T[]
        // If collection type is used for "T", "T" can be either an object (value or reference type)
        // or another collection listed above. Also, nullability keyword "?" can be used for
        // collection items as well as for collection type itself.
        // The result "employeesResult" is of type "JsonQL.Query.IObjectQueryResult<IReadOnlyList<IEmployee>>".
        var employeesResult =
            _queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
                new JsonTextData("Data",
                    this.LoadExampleJsonFile("Data.json"),
                    new JsonTextData("Parameters", this.LoadExampleJsonFile("Parameters.json"))),
                [false, false], new JsonConversionSettingsOverrides
                {
                    TryMapJsonConversionType = (type, parsedJson) =>
                    {
                        // If we always return null, or just do not set the value, of TryMapJsonConversionType
                        // IEmployee will always be bound to Employee
                        // In this example, we ensure that if parsed JSON has "Employees" field,
                        // then the default implementation of IManager (i.e., Manager) is used to
                        // deserialize the JSON.
                        // We can also specify Manager explicitly.
                        if (parsedJson.HasKey(nameof(IManager.Employees)))
                            return typeof(IManager);

                        return null;
                    }
                });

        return employeesResult;
    }
}
