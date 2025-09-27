using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Demos.Examples.DataModels;
using JsonQL.Query;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.NullableValueSupport.NullableSyntaxInQueryResultType.Examples.ResultAsListOfListsOfReferenceTypeItems;

public class Example : QueryObjectExampleManagerForSuccessAbstr<List<IReadOnlyList<IEmployee?>?>>
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IObjectQueryResult<List<IReadOnlyList<IEmployee?>?>> QueryObject()
    {
        // Select all companies
        var query = "Companies";

        // The result "companiesResult" is a list of list. Each company is represented as list of employees
        // The result of type "List<IReadOnlyList<IEmployee?>?>" cannot be null,
        // however list of employees can be null, and each employee in list of employees can be null too,
        // according to value used for parameter "convertedValueNullability"
        var companiesResult =
            _queryManager.QueryObject<List<IReadOnlyList<IEmployee?>?>>(query,
                new JsonTextData("Companies",
                    this.LoadExampleJsonFile("CompaniesOrganizedAsArraysOfArrays.json")),
                    convertedValueNullability: [
                    // The result of type "List<IReadOnlyList<IEmployee?>?>" cannot be null.
                    // An error will be reported if the result is null
                    false,
                    // "IReadOnlyList<IEmployee?>" items in "List<IReadOnlyList<IEmployee?>?>" can be null
                    true,
                    // "IEmployee" items in "IReadOnlyList<IEmployee?>" can be null.
                    true]);

        return companiesResult;
    }
}
