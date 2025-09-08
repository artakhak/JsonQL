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
                this.LoadExampleJsonFile("Parameters.json"));

    // countriesJsonTextData uses parametersJsonTextData for parameter parentJsonTextData
    var countriesJsonTextData = new JsonTextData("Countries",
        LoadJsonFileHelpers.LoadJsonFile("Countries.json", sharedExamplesFolderPath), parametersJsonTextData);

    var companiesJsonTextData = new JsonTextData("Companies",
        LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath), countriesJsonTextData);

    var filteredCompaniesJsonTextData = new JsonTextData("FilteredCompanies",
        this.LoadExampleJsonFile("FilteredCompanies.json"), companiesJsonTextData);      

    // Create an instance of JsonQL.Compilation.JsonCompiler here.
    // This is normally done once on application start.
    JsonQL.Compilation.IJsonCompiler jsonCompiler = null!;

    var result = jsonCompiler.Compile(new JsonTextData("Example",
        this.LoadExampleJsonFile("Example.json"), filteredCompaniesJsonTextData));

Now consider scenario when we want to reuse all or some of compiled parent JSON files to parse some other JSON file.
We could use the same method as above, however doing this would result in parent JSON files being compiled again.
To re-use the compiled parent JSON files without compiling them again we can call **ICompilationResult Compile(IJsonTextData jsonTextData)** the method to compile only parent JSON files,
and then use another overloaded version **ICompilationResult Compile(string jsonText, string jsonTextIdentifier, IReadOnlyList<ICompiledJsonData> compiledParents)** of that method which takes **IReadOnlyList<ICompiledJsonData> compiledParents** instead of **IJsonTextData jsonTextData** as a parameter.

Here is a code snippet demonstrating this approach:

.. sourcecode:: csharp

    string[] sharedExamplesFolderPath = new string[] { "DocFiles", "MutatingJsonFiles", "Examples"};

    var parametersJsonTextData = new JsonTextData("Parameters",
                this.LoadExampleJsonFile("Parameters.json"));

    // countriesJsonTextData uses parametersJsonTextData for parameter parentJsonTextData
    var countriesJsonTextData = new JsonTextData("Countries",
        LoadJsonFileHelpers.LoadJsonFile("Countries.json", sharedExamplesFolderPath), parametersJsonTextData);

    var companiesJsonTextData = new JsonTextData("Companies",
        LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath), countriesJsonTextData);

    var filteredCompaniesJsonTextData = new JsonTextData("FilteredCompanies",
        this.LoadExampleJsonFile("FilteredCompanies.json"), companiesJsonTextData);      

    // Create an instance of JsonQL.Compilation.JsonCompiler here.
    // This is normally done once on application start.
    JsonQL.Compilation.IJsonCompiler jsonCompiler = null!;

    var result = jsonCompiler.Compile(new JsonTextData("Example",
        this.LoadExampleJsonFile("Example.json"), filteredCompaniesJsonTextData));

    var parametersJsonTextData = new JsonTextData("Parameters", this.LoadExampleJsonFile("Parameters.json"));

    var countriesJsonTextData = new JsonTextData("Countries",
                LoadJsonFileHelpers.LoadJsonFile("Countries.json", sharedExamplesFolderPath), parametersJsonTextData);

    var companiesJsonTextData = new JsonTextData("Companies",
        LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath), countriesJsonTextData);

    var cachedCompilationResult = _jsonCompiler.Compile(new JsonTextData("FilteredCompanies",
        this.LoadExampleJsonFile("FilteredCompanies.json"), companiesJsonTextData));

    if (cachedCompilationResult.CompilationErrors.Count > 0)
        throw new ApplicationException("Compilation failed");

    var compiledParents = new List<ICompiledJsonData>
    {
        cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "Companies")
    };

    var jsonThatDependsOnCompanies = 
        string.Concat("{\"AllCompanyNames:\": \"$value(Companies.Select(x => x.CompanyData.Name))\"," +
        "\"AllCompanyEmployees:\": \"$value(Companies.Where(x => !(x.CompanyData.Name starts with 'Strange')).Select(x => x.Employees))\"}");

    var jsonThatDependsOnCompaniesResult = _jsonCompiler.Compile(jsonThatDependsOnCompanies, "Json1", compiledParents);
    // Do something with jsonThatDependsOnCompaniesResult here.

    compiledParents = new List<ICompiledJsonData>
    {
        cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "Parameters"),
        cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "Countries"),
        cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "Companies"),
        cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "FilteredCompanies"),
    };

    var exampleJsonResult = _jsonCompiler.Compile(this.LoadExampleJsonFile("Example.json"), "Example", compiledParents);

- In this example we do the following:
    - Compile parent JSON files and store the result in **cachedCompilationResult**
    - Compile a child JSON in variable **jsonThatDependsOnCompanies** by passing collection of **JsonQL.JsonObjects.JsonQL.Compilation** retrieved from **cachedCompilationResult** for only **Companies.json** as parents.
    - Compile a child JSON in file **Example.json** by passing collection of **JsonQL.JsonObjects.JsonQL.Compilation** retrieved from **cachedCompilationResult** for **Parameters**, **Companies.json**, **Companies**, **FilteredCompanies** as parents, since JsonQL expressions in **Example.json** depend on JSON objects in all these fies.
