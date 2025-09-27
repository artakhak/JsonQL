using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Demos.Examples.DataModels;
using JsonQL.Query;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.Examples.QueryReferenceTypeValue;


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
        string[] sharedExamplesFolderPath = ["DocFiles", "QueryingJsonFiles", "JsonFiles"];

        var parametersJsonTextData = new JsonTextData("Parameters",
            LoadJsonFileHelpers.LoadJsonFile("Parameters.json", sharedExamplesFolderPath));

        var countriesJsonTextData = new JsonTextData("Countries",
            LoadJsonFileHelpers.LoadJsonFile("Countries.json", sharedExamplesFolderPath), parametersJsonTextData);

        var companiesJsonTextData = new JsonTextData("Companies",
            LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath), countriesJsonTextData);

        var filteredCompaniesJsonTextData = new JsonTextData("FilteredCompanies",
            LoadJsonFileHelpers.LoadJsonFile("FilteredCompanies.json", sharedExamplesFolderPath), companiesJsonTextData);
       
        var query = "FilteredCompanies.Select(c => c.Employees.Where(e => e.Name !=  'John Smith'))";

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
        // The value employeesResult.Value contains the result of the query and is of type IReadOnlyList<IEmployee>.
        var employeesResult =
            _queryManager.QueryObject<IReadOnlyList<IEmployee>>(query, filteredCompaniesJsonTextData);

        //LogHelper.Context.Log.InfoFormat("Number of employees is {0}", employeesResult.Value?.Count ?? 
        //                             throw new ApplicationException(
        //                                 $"Query failed. The serialized [{nameof(employeesResult)}] has the error details."));
        return employeesResult;
    }
}
