===================
Mutating JSON Files 
===================

.. contents::
   :local:
   :depth: 2

.. note::
   All JSON files used in examples in this section can be found here :doc:`./SampleFiles/index`  
   
- JsonQL expressions are used in one or many JSON files. JsonQL evaluates JsonQL expressions and loads the parsed JSON files with expressions replaced with calculated JSON objects into an instance of `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/Compilation/ICompilationResult.cs>`_.
- The property **CompiledJsonFiles** contains collection of `JsonQL.Compilation.ICompiledJsonData <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/Compilation/ICompiledJsonData.cs>`_: one per loaded file. 
- `JsonQL.Compilation.ICompiledJsonData <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/Compilation/ICompiledJsonData.cs>`_ represents mutated JSON files (i.e., mutated by using JsonQL expressions).  
- The property **CompilationErrors** contains collection of `JsonQL.Compilation.ICompilationErrorItem <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/Compilation/ICompilationErrorItem.cs>`_ with error details if any. 
- If many JSON files are specified the following rules and techniques are used:
  - Parent/child relationships between JSON files are maintained, and parent JSON files are evaluated before child JSON files are evaluated.
  - Lookup of JSON values specified in JsonQL expressions starts in JSON containing the expression first, and then in parent JSON files.

  
As an example lets consider the following JSON files. These files will be evaluated loaded by JsonQL in C# code snippet below which will evaluate the files on top of the list as parent JSON files of files that appear later. 

.. note::
  JsonQL Expressions start with '$'. Example "$value(Employees.Select(x => x.Salary >= 100000))".

- :doc:`./SampleFiles/Example1/parameters` - a JSON file with lists "FilteredCountryNames" and "FilteredCompanyNames" referenced in other JSON files below in JsonQL expressions.
- :doc:`./SampleFiles/countries` - a JSON file for data on number of countries
- :doc:`./SampleFiles/companies` - a JSON file for data on number of companies
- :doc:`./SampleFiles/Example1/filtered-companies` - a JSON file shown below with JsonQL expressions that filter companies in :doc:`./SampleFiles/companies` to include only some companies using data in :doc:`./SampleFiles//Example1/parameters`.

.. sourcecode:: json

    {
      "AdditionalCompanyNames": [ "Atlantic Transfers, Inc" ],

      "comments1": "'FilteredCompanyNames' is in parent JSON 'Parameters.json'.",
      "comments2": "We filter companies that are either in FilteredCompanyNames or in AdditionalCompanyNames in this file.",
      "FilteredCompanies": "$value(Companies.Where(c => Any(FilteredCompanyNames, x => x == c.CompanyData.Name) || Any(AdditionalCompanyNames, x => x == c.CompanyData.Name)))"
    }


- :doc:`./SampleFiles/Example1/example` - a JSON file shown below with JsonQL expressions that reference JSON objects in this file as well as in files listed above. The expressions in this file can also reference JSON objects from files above that are generated using JSON expressions (for example companies in **FilteredCompanies** in :doc:`./SampleFiles/Example1/filtered-companies`)
 
.. sourcecode:: json

    {
      "FilteredCountryNames": [ "United States", "Canada", "Australia" ],

      "FilteredCountryData": "$value(Countries.Where(c => Any(FilteredCountryNames.Where(x => x == c.Name))).Select(c => Concatenate('Name:', c.Name, ', Population:', c.Population)))",
        
      "comments": "'FilteredCompanies' array used in JsonQL expressions below is in parent JSON FilteredCompanies.json",
      "FilteredCompanyAddresses": {
        "comments": "'FilteredCompanies' array is in parent JSON FilteredCompanies.json",
        "addresses": "$value(FilteredCompanies.Where(c => c.CompanyData.CEO != 'John Malkowich').Select(x => x.Address))"
      },

      "FilteredCompanyEmployees": "$value(FilteredCompanies.Select(c => c.Employees.Where(e => e.Name !=  'John Smith')))",
      "FilteredCompanyEmployeeAddresses": "$value(FilteredCompanies.Select(c => c.Employees.Select(x => x.Address)))"
    }


The code snippet below shows how to load the the five files above in such a way that JsonQL loads :doc:`./SampleFiles/countries` first, then loads :doc:`./SampleFiles/companies` as a child of :doc:`./SampleFiles/countries` (i.e., can use JsonQL expressions that reference JSON objects in evaluated file :doc:`./SampleFiles/countries`), then
loads :doc:`./SampleFiles/Example1/parameters`, :doc:`./SampleFiles/Example1/filtered-companies` and finally loads :doc:`./SampleFiles/Example1/example` using similar parent child relationship rules.

.. sourcecode:: csharp

    

    var parametersJsonTextData = new JsonTextData("Parameters",
                this.LoadExampleJsonFile("Parameters.json"));

    // countriesJsonTextData has uses parametersJsonTextData for parameter parentJsonTextData
    var countriesJsonTextData = new JsonTextData("Countries",
        LoadJsonFileHelpers.LoadJsonFile("Countries.json", _sharedExamplesFolderPath), parametersJsonTextData);

    var companiesJsonTextData = new JsonTextData("Companies",
        LoadJsonFileHelpers.LoadJsonFile("Companies.json", _sharedExamplesFolderPath), countriesJsonTextData);

    var filteredCompaniesJsonTextData = new JsonTextData("FilteredCompanies",
        this.LoadExampleJsonFile("FilteredCompanies.json"), companiesJsonTextData);      

    // Create an instance of JsonQL.Compilation.JsonCompiler here.
    //This is normally done once on application start.
    JsonQL.Compilation.IJsonCompiler jsonCompiler = null!;

    var result = jsonCompiler.Compile(new JsonTextData("Example",
        this.LoadExampleJsonFile("Example.json"), filteredCompaniesJsonTextData));

- The value **result** in C# snippet above is of type `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/Compilation/ICompilationResult.cs>`_ and the serialized value of **result** can be found here :doc:`./SampleFiles/Example1/result`.
- To retrieve the details of loaded JSON files in `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/Compilation/ICompilationResult.cs>`_ you need to retrieve an item in **CompiledJsonFiles** property for compiled JSON file. The items in this list are ordered in such a way that parent JSON file items appear before child JSON file items.
- For more details on data structure used for loading multiple JSON files see :doc:`./ParsedResultDataStructure/index`

**TEXT TO DELETE START**

- The value **result** in C# snippet above is of type `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/Compilation/ICompilationResult.cs>`_ and the serialized value of **result** can be found here :doc:`./SampleFiles/Example1/result`.
- To retrieve the details of loaded JSON files in `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/Compilation/ICompilationResult.cs>`_ you need to retrieve an item in **CompiledJsonFiles** property for compiled JSON file. The items in this list are ordered in such a way that parent JSON file items appear before child JSON file items.
- Each item in **CompiledJsonFiles** list in `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/Compilation/ICompilationResult.cs>`_ is of type `JsonQL.Compilation.ICompiledJsonData <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/Compilation/ICompiledJsonData.cs>`_.
- `JsonQL.Compilation.ICompiledJsonData <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/Compilation/ICompiledJsonData.cs>`_ has properties **TextIdentifier** and **CompiledParsedValue** (among other properties).
- Property **TextIdentifier** in `JsonQL.Compilation.ICompiledJsonData <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/Compilation/ICompiledJsonData.cs>`_ stores parsed file identifier.
- Property **CompiledParsedValue** in `JsonQL.Compilation.ICompiledJsonData <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/Compilation/ICompiledJsonData.cs>`_ stores the compiled JSON structure after applying all JsonQL expressions.
- The type of **CompiledParsedValue** property is `JsonQL.JsonObjects.IRootParsedValue <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/JsonObjects/IRootParsedValue.cs>`_.
- The interface `JsonQL.JsonObjects.IRootParsedValue <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/JsonObjects/IRootParsedValue.cs>`_ that stores loaded parsed JSON extends interface `JsonQL.JsonObjects.IParsedValue <https://github.com/artakhak/JsonQL/blob/1c115784fd0795c891a26ece647d92ee5125fad4/JsonQL/JsonObjects/IParsedValue.cs>`_.
- The interface `JsonQL.JsonObjects.IParsedValue <https://github.com/artakhak/JsonQL/blob/1c115784fd0795c891a26ece647d92ee5125fad4/JsonQL/JsonObjects/IParsedValue.cs>`_ is a base interface for all interfaces used to store parsed JSON values. Here is a list of all interfaces that extend `JsonQL.JsonObjects.IParsedValue <https://github.com/artakhak/JsonQL/blob/1c115784fd0795c891a26ece647d92ee5125fad4/JsonQL/JsonObjects/IParsedValue.cs>`_:
    - `JsonQL.JsonObjects.IRootParsedValue <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/JsonObjects/IRootParsedValue.cs>`_ - Stores entire parsed JSON file data.
    - `JsonQL.JsonObjects.IRootParsedJson <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/JsonObjects/IRootParsedJson.cs>`_ - A subclass of `JsonQL.JsonObjects.IRootParsedValue <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/JsonObjects/IRootParsedValue.cs>`_. Stores entire parsed JSON file data if root object in JSON file is a JSON object enclosed that starts and ends with '{' and '}' braces.
    - `JsonQL.JsonObjects.IRootParsedArrayValue <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/JsonObjects/IRootParsedArrayValue.cs>`_ - A subclass of `JsonQL.JsonObjects.IRootParsedValue <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/JsonObjects/IRootParsedValue.cs>`_. Stores entire parsed JSON file data if root object in JSON file is a JSON array object that starts and ends with '[' and ']' braces.
    - `JsonQL.JsonObjects.IParsedJson <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/JsonObjects/IParsedJson.cs>`_ - Stores parsed JSON object data (i.e., JSON object that starts and ends with '{' and '}' braces).
    - `JsonQL.JsonObjects.IParsedArrayValue <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/JsonObjects/IParsedArrayValue.cs>`_ - Stores parsed JSON array data (i.e., JSON object that starts and ends with '[' and ']' braces).
    - `JsonQL.JsonObjects.IParsedSimpleValue <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/JsonObjects/IParsedSimpleValue.cs>`_ - Stores parsed JSON simple value, such a string value, numeric value, boolean value.
    - Test `JsonQL.JsonObjects.IParsedCalculatedValue <https://github.com/artakhak/JsonQL/blob/1c115784fd0795c891a26ece647d92ee5125fad4/JsonQL/JsonObjects/IParsedCalculatedValue.cs>`_ - Stores calculated value for which a JSON object is created during JsonQL expression evaluation, but for which there is no corresponding JSON object in JSON files loaded. 
        - Any instance of `JsonQL.JsonObjects.IParsedCalculatedValue <https://github.com/artakhak/JsonQL/blob/1c115784fd0795c891a26ece647d92ee5125fad4/JsonQL/JsonObjects/IParsedCalculatedValue.cs>`_ also implements one of the following interfaces:
            - `JsonQL.JsonObjects.IParsedSimpleValue <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/JsonObjects/IParsedSimpleValue.cs>`_
            - `JsonQL.JsonObjects.IParsedJson <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/JsonObjects/IParsedJson.cs>`_
            - `JsonQL.JsonObjects.IParsedArrayValue <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/JsonObjects/IParsedArrayValue.cs>`_
                            
        - For example consider the following two examples demonstrating `JsonQL.JsonObjects.IParsedCalculatedValue <https://github.com/artakhak/JsonQL/blob/1c115784fd0795c891a26ece647d92ee5125fad4/JsonQL/JsonObjects/IParsedCalculatedValue.cs>`_:
            - Example with **JsonQL.JsonObjects.IParsedCalculatedValue**: {"MyCalculatedValue": "$value(Employees.Select(e => 2 * e.Salary))"}. In this case an instance of **JsonQL.JsonObjects.IParsedCalculatedValue** is created to store the numeric value **2 * e.Salary**. 
            - Example with **JsonQL.JsonObjects.IParsedSimpleValue**: {"MySimpleJsonValue": "$value(Employees.Select(e => e.Salary))"}. In this case an instance of **JsonQL.JsonObjects.IParsedSimpleValue** is created to store the numeric value **e.Salary** which represents a JSON value in one of the loaded JSON files. 

**TEXT TO DELETE END**

.. toctree::
   
   ParsedResultDataStructure/index.rst
   ErrorDetails/index.rst
   ReusingCompiledJsonFiles/index.rst   
   SampleFiles/index.rst