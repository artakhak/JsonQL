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
        var query = "Where(x => x.Id==100000006 || x.Id==100000007)";

        // We can convert to the following collection types:
        // -One of the following interfaces: IReadOnlyList<T>, IEnumerable<T>, IList<T>, ICollection<T>, IReadOnlyCollection<T>
        // -Any type that implements ICollection<T>. Example: List<T>,
        // -Array T[],
        // In these examples T is either an object (value or reference type), or another collection type (one of the listed here). 
        var employeesResult =
            _queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
                new JsonTextData("Data",
                    this.LoadExampleJsonFile("Data.json")),
                [false, false], new JsonConversionSettingsOverrides
                {
                    TryMapJsonConversionType = (type, parsedJson) =>
                    {
                        if (parsedJson.HasKey(nameof(IManager.Employees)))
                            return typeof(IManager);

                        return null;
                    }
                });

        return employeesResult;
    }
}
