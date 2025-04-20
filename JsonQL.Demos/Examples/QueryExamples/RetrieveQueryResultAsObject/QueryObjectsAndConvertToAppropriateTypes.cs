using JsonQL.Compilation;
using JsonQL.Demos.Examples.DataModels;
using JsonQL.Extensions.JsonToObjectConversion;
using JsonQL.Extensions.Query;

namespace JsonQL.Demos.Examples.QueryExamples.RetrieveQueryResultAsObject;

public class QueryObjectsAndConvertToAppropriateTypes : QueryObjectExampleManagerAbstr<IReadOnlyList<IEmployee>>
{
    private readonly IQueryManager _queryManager;

    public QueryObjectsAndConvertToAppropriateTypes(IQueryManager queryManager)
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
        var employees =
            _queryManager.Query<IReadOnlyList<IEmployee>>(query,
                new JsonTextData("Employees",
                    LoadJsonFileHelpers.LoadJsonFile("QueryObjectsAndConvertToAppropriateTypes.json",
                        ["Examples", "QueryExamples", "RetrieveQueryResultAsObject"])),
                [false, false], new JsonConversionSettingsOverrides
                {
                    TryMapJsonConversionType = (type, parsedJson) =>
                    {
                        if (parsedJson.HasKey(nameof(IManager.Employees)))
                            return typeof(IManager);

                        return null;
                    }
                });

        return employees;
    }
}