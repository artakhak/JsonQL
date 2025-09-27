===========================
Reusing Compiled JSON Files
===========================

.. contents::
   :local:
   :depth: 2

Similar to how we can reuse compiled JSON files in calls to method **Compile(string jsonText, string jsonTextIdentifier, IReadOnlyList<ICompiledJsonData> compiledParents)** in `JsonQL.Compilation.IJsonCompiler <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/IJsonCompiler.cs>`_ (demonstrated in :doc:`../../MutatingJsonFiles/index`) we can also pre-compile JSON files and re-use them when calling the overloaded methods **QueryObject(..., IReadOnlyList<ICompiledJsonData> compiledJsonDataToQuery,...)** and **QueryJsonValue(..., IReadOnlyList<ICompiledJsonData> compiledJsonDataToQuery,...)** in `JsonQL.Query.IQueryManager <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryManager.cs>`_, as well when calling extension methods for interface **JsonQL.Query.IQueryManager** that use the parameter **IReadOnlyList<ICompiledJsonData> compiledJsonDataToQuery**.

Pre-compiling JSON files and using them in calls to the overloaded methods for queries (**QueryObject<TQueriedObject>(...)** and **QueryJsonValue**) makes more sense then using overloaded methods that use parameter of type `JsonQL.Compilation.IJsonTextData.cs <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/IJsonTextData.cs>`_ (**IJsonTextData** stores JSON files as text strings) is more efficient when we need to execute multiple queries against the same JSON files.

The example in code snipped below compiles the following JSON files first, and then uses these compiled files in queries:

- :doc:`../SampleFiles/parameters`
- :doc:`../SampleFiles/countries`
- :doc:`../SampleFiles/companies`
- :doc:`../SampleFiles/filtered-companies`

To re-use the compiled parent JSON files in JsonQL queries without compiling them again do the following:

  - Call **ICompilationResult Compile(IJsonTextData jsonTextData)** the method to compile only parent JSON files.
  - Execute queries by calling the overloaded methods **QueryObject(..., IReadOnlyList<ICompiledJsonData> compiledJsonDataToQuery,...)** (or similar extension methods) that take **IReadOnlyList<ICompiledJsonData> compiledParents** as a parameter.
      - Generate the list used for parameter **compiledParents** from compiled JSON objects generated in first step above.

Here is a code snippet demonstrating this approach:

.. sourcecode:: csharp

    // Set the value of jsonCompiler to an instance of JsonQL.Compilation.IJsonCompiler here.
    // The value of JsonQL.Compilation.JsonCompiler is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.
    JsonQL.Compilation.IJsonCompiler jsonCompiler = null!;

    // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
    // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.
    JsonQL.Query.IQueryManager queryManager = null!;

    var sharedExamplesFolderPath = new []
    {
        "DocFiles", "QueryingJsonFiles", "JsonFiles"
    };

    var parametersJsonTextData = new JsonTextData("Parameters",
        LoadJsonFileHelpers.LoadJsonFile("Parameters.json", sharedExamplesFolderPath));

    var countriesJsonTextData = new JsonTextData("Countries",
        LoadJsonFileHelpers.LoadJsonFile("Countries.json", sharedExamplesFolderPath), parametersJsonTextData);

    var companiesJsonTextData = new JsonTextData("Companies",
        LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath), countriesJsonTextData);

    var cachedCompilationResult = jsonCompiler.Compile(new JsonTextData("FilteredCompanies",
        LoadJsonFileHelpers.LoadJsonFile("FilteredCompanies.json", sharedExamplesFolderPath), companiesJsonTextData));

    if (cachedCompilationResult.CompilationErrors.Count > 0)
        throw new ApplicationException("Compilation failed");

    // The first query (see employeesResult below) needs only compiled and cached "FilteredCompanies" file.
    // So lets get this compiled file from cachedCompilationResult and store it in "compiledParents" list
    // that we will pass to JsonQL.Query.IQueryManager.QueryObject<IReadOnlyList<IEmployee>>(query, compiledParents) below
    var compiledParents = new List<ICompiledJsonData>
    {
        cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "FilteredCompanies")
    };

    var employeesResult = queryManager.QueryObject<IReadOnlyList<IEmployee>>(
        "FilteredCompanies.Select(c => c.Employees).Where(e => e.Age >= 40)",
        compiledParents);

    Assert.That(employeesResult.Value is not null && employeesResult.Value.Count == 6);

    // The second query below is executed using JsonQL.Query.IQueryManager.QueryJsonValue(query, parents).
    // The query executes against the files "Countries" and "Parameters". 
    // So lets get these compiled files from cachedCompilationResult and store them in "compiledParents" list
    // that we will pass to JsonQL.Query.IQueryManager.QueryJsonValue(query, parents).
    // NOTE: The list of parents compiledParents passed to JsonQL.Query.IQueryManager.QueryJsonValue(...)
    // is organized in such a way that child JSON files appear earlier, and parent JSON files appear later.
    // In this example "Countries.json" will be treated as a child of "Parameters".
    // This relationship will ensure that JSON objects referenced in JsonQL expressions will be looked up
    // first in "Countries.json" and then in "Countries".
    compiledParents =
    [
        cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "Countries"),
        cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "Parameters")
    ];
            
    var countriesResult = queryManager.QueryJsonValue("Countries.Where(c => Any(FilteredCountryNames.Where(fc => fc == c.Name)))", compiledParents);

    Assert.That(countriesResult.ParsedValue is not null && 
                countriesResult.ParsedValue is IParsedArrayValue countriesJsonArray &&
                countriesJsonArray.Values.Count == 2);


- In this example we do the following:
    - Compile parent JSON files and store the result in **cachedCompilationResult**
    - Execute the first query "FilteredCompanies.Select(c => c.Employees).Where(e => e.Age >= 40)" by passing a list generated from compiled JSON "FilteredCompanies.json" as a parameter in method call **queryManager.QueryObject<IReadOnlyList<IEmployee>>**.
    - Execute the second query "Countries.Where(c => Any(FilteredCountryNames.Where(fc => fc == c.Name)))" by passing a list generated from compiled JSON files "Parameters.json" and "Countries.json" (with "Countries.json" being treated as a child of "Parameters.json") as a parameter in method call **queryManager.QueryJsonValue**.
