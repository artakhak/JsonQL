===========================
Reusing Compiled JSON Files
===========================

.. contents::
   :local:
   :depth: 2

Example in code snippet below is copied from section :doc:`../index` and uses one of overladed methods **ICompilationResult Compile(IJsonTextData jsonTextData)** in interface `JsonQL.Compilation.IJsonCompiler <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/IJsonCompiler.cs>`_ to parse **"Example.json**.
This example also parses parent JSON files **Parameters.json**, **Countries**, **Companies**, **FilteredCompanies**.   
        
.. sourcecode:: csharp

    string[] sharedExamplesFolderPath = new string[] { "DocFiles", "MutatingJsonFiles", "Examples"};

    var parametersJsonTextData = new JsonTextData("Parameters",
        LoadJsonFileHelpers.LoadJsonFile("Parameters.json", sharedExamplesFolderPath));

    // countriesJsonTextData uses parametersJsonTextData for parameter parentJsonTextData
    var countriesJsonTextData = new JsonTextData("Countries",
        LoadJsonFileHelpers.LoadJsonFile("Countries.json", sharedExamplesFolderPath), parametersJsonTextData);

    var companiesJsonTextData = new JsonTextData("Companies",
        LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath), countriesJsonTextData);

    var filteredCompaniesJsonTextData = new JsonTextData("FilteredCompanies",
        LoadJsonFileHelpers.LoadJsonFile("FilteredCompanies.json",  sharedExamplesFolderPath), companiesJsonTextData);

    // Set the value of jsonCompiler to an instance of JsonQL.Compilation.IJsonCompiler here.
    // The value of JsonQL.Compilation.JsonCompiler is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.
    JsonQL.Compilation.IJsonCompiler jsonCompiler = null!;

    var result = jsonCompiler.Compile(new JsonTextData("Example",
        this.LoadExampleJsonFile("Example.json"), filteredCompaniesJsonTextData));


Now consider scenario when we want to reuse all or some of compiled parent JSON files to parse some other JSON file.
We could use the same method as above, however doing this would result in parent JSON files being compiled again.
To re-use the compiled parent JSON files without compiling them again we can call the **ICompilationResult Compile(IJsonTextData jsonTextData)** method to compile only parent JSON files,
and then use another overloaded version **ICompilationResult Compile(string jsonText, string jsonTextIdentifier, IReadOnlyList<ICompiledJsonData> compiledParents)** of that method which takes **IReadOnlyList<ICompiledJsonData> compiledParents** instead of **IJsonTextData jsonTextData** as a parameter.

Here is a code snippet demonstrating this approach:

.. sourcecode:: csharp

    string[] sharedExamplesFolderPath = new string[] { "DocFiles", "MutatingJsonFiles", "Examples"};

    var parametersJsonTextData = new JsonTextData("Parameters",
                this.LoadExampleJsonFile("Parameters.json"));

    // countriesJsonTextData uses parametersJsonTextData for parameter parentJsonTextData
    var countriesJsonTextData = new JsonTextData("Countries",
                LoadJsonFileHelpers.LoadJsonFile("Countries.json", _sharedExamplesFolderPath), parametersJsonTextData);

    var companiesJsonTextData = new JsonTextData("Companies",
                LoadJsonFileHelpers.LoadJsonFile("Companies.json", _sharedExamplesFolderPath), countriesJsonTextData);

    // Set the value of jsonCompiler to an instance of JsonQL.Compilation.IJsonCompiler here.
    // The value of JsonQL.Compilation.IJsonCompiler is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.
    JsonQL.Compilation.IJsonCompiler jsonCompiler = null!;

    var cachedCompilationResult = jsonCompiler.Compile(new JsonTextData("FilteredCompanies",
                this.LoadExampleJsonFile("FilteredCompanies.json"), companiesJsonTextData));

    if (cachedCompilationResult.CompilationErrors.Count > 0)
                throw new ApplicationException("Compilation failed");
            
    var compiledParents = new List<ICompiledJsonData>
    {
        cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "Companies")
    };

    var jsonThatDependsOnCompanies = 
           string.Concat(
           "{\"AllCompanyNames:\": \"$value(Companies.Select(x => x.CompanyData.Name))\"," +
           "\"AllCompanyEmployees:\": \"$value(Companies.Where(x => !(x.CompanyData.Name starts with 'Strange')).Select(x => x.Employees))\"}");

    // Compile jsonThatDependsOnCompanies JSON using only compiled "Companies.json" as parents (in compiledParents)
    var jsonThatDependsOnCompaniesResult = jsonCompiler.Compile(jsonThatDependsOnCompanies, "Json1", compiledParents);
    // Do something with jsonThatDependsOnCompaniesResult here.

    // NOTE: The list of parents compiledParents passed to _jsonCompiler.Compile() is organized in such a way
    // that child JSON files appear earlier, and parent JSON files appear later.
    // In this example "Example.json" will be treated as a child of "FilteredCompanies", "FilteredCompanies" will be treated as a child of
    // "Companies" and so forth. 
    // This relationship will ensure that JSON objects referenced in JsonQL expressions in "Example.json" will
    // be looked up first in "Example.json", then in "FilteredCompanies", and so forth.
    compiledParents =
    [
        cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "FilteredCompanies"),
        cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "Companies"),
        cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "Countries"),
        cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "Parameters")
    ];

    // Compile jsonThatDependsOnCompanies JSON using all four compiled JSON files as parents (in compiledParents)
    var exampleJsonResult = jsonCompiler.Compile(this.LoadExampleJsonFile("Example.json"), "Example", compiledParents);
    // Do something with exampleJsonResult here.


- In this example we do the following:
    - Compile parent JSON files and store the result in **cachedCompilationResult**
    - Compile a child JSON in variable **jsonThatDependsOnCompanies** by passing collection of **JsonQL.JsonObjects.JsonQL.Compilation** retrieved from **cachedCompilationResult** for only **Companies.json** as parents.
    - Compile a child JSON in file **Example.json** by passing collection of **JsonQL.JsonObjects.JsonQL.Compilation** retrieved from **cachedCompilationResult** for **Parameters**, **Companies.json**, **Companies**, **FilteredCompanies** as parents, since JsonQL expressions in **Example.json** depend on JSON objects in all these fies.
