using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Query;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.NullableValueSupport.NullableSyntaxInQueryResultType.Examples.ResultAsListOfValueTypeItems;

public class Example : QueryObjectExampleManagerForSuccessAbstr<IReadOnlyList<double?>?>
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IObjectQueryResult<IReadOnlyList<double?>?> QueryObject()
    {
        var query = "ListOfListsOfDoubles.Flatten().Where(x => x is null || x > 10)";

        // The result "listOfNumbersResult" is a nullable list of nullable double values. 
        // Result of type "IReadOnlyList<double?>?" can be null, and each double value
        // in list "IReadOnlyList<double?>" can be null according to value used for parameter
        // "convertedValueNullability"
        var listOfNumbersResult =
            _queryManager.QueryObject<IReadOnlyList<double?>?>(query,
                new JsonTextData("Data",
                    this.LoadExampleJsonFile("Data.json")),
                convertedValueNullability: [
                    // The result of type "IReadOnlyList<double?>?" can be null.
                    true,
                    // "double" values in list "IReadOnlyList<double?>" can be null.
                    true]);

        return listOfNumbersResult;
    }
}
