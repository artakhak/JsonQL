using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Query;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.NullableValueSupport.NullableSyntaxInQueryResultType.Examples.ResultAsListOfArraysOfValueTypeItems;

public class Example : QueryObjectExampleManagerForSuccessAbstr<IReadOnlyList<double?[]>>
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IObjectQueryResult<IReadOnlyList<double?[]>> QueryObject()
    {
        var query = "ListOfListsOfDoubles";

        // The result "listOfListsOfNumbersResult" is a list of arrays of double values. 
        // Result of type "IReadOnlyList<double?[]>" cannot be null, and each array "double?[]" in list
        // "IReadOnlyList<double?[]>" cannot be null, however numeric values in "double?[]" can be null in converted object
        // according to value used for parameter "convertedValueNullability"
        var listOfListsOfNumbersResult =
            _queryManager.QueryObject<IReadOnlyList<double?[]>>(query,
                new JsonTextData("Data",
                    this.LoadExampleJsonFile("Data.json")),
                convertedValueNullability: [
                    // The result of type "IReadOnlyList<double?[]>" cannot be null. An error will be reported if the result is null
                    false,
                    // "double?[]" items in "IReadOnlyList<double?[]>>" cannot be null
                    false,
                    // "double" values in "double?[]" array can be null.
                    true]);

        return listOfListsOfNumbersResult;
    }
}
