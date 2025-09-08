============================================================
Result Data Structure: JsonQL.Compilation.ICompilationResult
============================================================

.. contents::
   :local:
   :depth: 2

- The result of loading JSON files is stored in an instance of `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationResult.cs>`_.

Consider the following C# code snippet that loads :doc:`../../MutatingJsonFiles/SampleFiles/Example1/example` that depends on these JSON files (i.e., has JsonQL expressions that reference JSON objects in these files).

- :doc:`../../MutatingJsonFiles/SampleFiles/Example1/parameters` - a JSON file with lists "FilteredCountryNames" and "FilteredCompanyNames" referenced in other JSON files below in JsonQL expressions.
- :doc:`../../MutatingJsonFiles/SampleFiles/countries` - a JSON file for data on number of countries
- :doc:`../../MutatingJsonFiles/SampleFiles/companies` - a JSON file for data on number of companies
- :doc:`../../MutatingJsonFiles/SampleFiles/Example1/filtered-companies` - a JSON file shown below with JsonQL expressions that filter companies in :doc:`../../MutatingJsonFiles/SampleFiles/companies` to include only some companies using data in :doc:`../../MutatingJsonFiles/SampleFiles/Example1/parameters`.

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

- The value **result** in C# snippet above is of type `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationResult.cs>`_ and the serialized value of **result** can be found here :doc:`../../MutatingJsonFiles/SampleFiles/Example1/result`.
- To retrieve the details of loaded JSON files in `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationResult.cs>`_ you need to retrieve an item in **CompiledJsonFiles** property for compiled JSON file. The items in this list are of type `JsonQL.Compilation.ICompiledJsonData <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompiledJsonData.cs>`_ are ordered in such a way that parent JSON file items appear before child JSON file items.
- See :doc:`ICompiledJsonData/index` to learn more about this interface.
- Property **CompilationErrors** stores details of errors encountered during compilation process, if any.

.. toctree:: 
  
   ICompiledJsonData/index.rst
   IRootParsedValue/index.rst
   IParsedValue/index.rst
