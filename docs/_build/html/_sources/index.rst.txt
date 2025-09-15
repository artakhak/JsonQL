======
JsonQL
======

.. contents::
   :local:
   :depth: 2
   
- **JsonQL** is a powerful JSON query language implementation that provides a flexible way to query and manipulate JSON data using a SQL/Linq-like syntax with rich function support.
- All aspects of implementations are extensible (custom operators, functions, path elements, etc. can be added). JsonQL expressions are used in JSON texts and are parsed by JsonQL library.
- Allows using JsonQL expressions in one or more JSON files to mutate JSON files. JsonQL parses the mutated JSON files with JsonQL expressions to generate a JSON structure.
- Supports executing JsonQL queries against one ore more JSON files. The query result is converted either to C# model classes (depending on API used).

  .. note::
          Model classes used for de-serialization can be either C# classes or interfaces, and the properties can be of class or interface types. JsonQL will either use default implementations of interfaces, or will use classes specified by the developer.

- Errors are reported in JsonQL error classes that have error position data as well as additional data.

Features
========

- Rich set of built-in functions for JSON manipulation
- Support for aggregate functions (COUNT, AVG, MIN, MAX, SUM)
- Collection manipulation with ANY and ALL operations
- String operations (ToUpper, ToLower, Length, Concatenate, etc)
- Type conversion functions (DateTime, Date, Double, Int, Boolean, String)
- Mathematical operations (Abs, IsEven, IsOdd)
- Object property inspection (HasField)
- Lambda expression support for complex queries
- Extensible function architecture
- Built-in conversion of query results to C# objects

  .. note:: 
    JsonQL binds interfaces to default implementations and also supports binding interfaces to any class via configuration**

- Mutating JSON files by replacing JSON field values by evaluated JSON values
- Extending the API to provide custom operators, functions, as well as customizing any part of JsonQL implementation

.. note::
  JsonQL Expressions start with '$'. Example "$value(Employees.Select(x => x.Salary >= 100000))".

Using JsonQL Expressions to Mutate JSON Files 
=============================================

.. note::
    **This section is a short summary with couple of examples. For more details reference this section:** :doc:`MutatingJsonFiles/index`

- JsonQL expressions are used in one or many JSON files. JsonQL evaluates JsonQL expressions and loads the parsed JSON files with expressions replaced with calculated JSON objects into an instance of `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/Compilation/ICompilationResult.cs>`_.
- The property **CompiledJsonFiles** contains collection of `JsonQL.Compilation.ICompiledJsonData <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompiledJsonData.cs>`_: one per loaded file. 
- `JsonQL.Compilation.ICompiledJsonData <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompiledJsonData.cs>`_ represents mutated JSON files (i.e., mutated by using JsonQL expressions).  
- The property **CompilationErrors** contains collection of `JsonQL.Compilation.ICompilationErrorItem <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationErrorItem.cs>`_ with error details if any. 
- If many JSON files are specified the following rules and techniques are used:
  - Parent/child relationships between JSON files are maintained, and parent JSON files are evaluated before child JSON files are evaluated.
  - Lookup of JSON values specified in JsonQL expressions starts in JSON containing the expression first, and then in parent JSON files.

Example: JsonQL expressions to mutate JSON files
------------------------------------------------

An overview example of mutating multiple JSON files is `here <https://github.com/artakhak/JsonQL/tree/main/JsonQL.Demos/Examples/IJsonCompilerExamples/SuccessExamples/Overview>`_.

In this example the following JSON files are processed with JSON files appearing earlier being processed as parents JSON files appearing later:

  - :doc:`./Examples/json-with-json-ql-expressions.data-1`
  - :doc:`./Examples/json-with-json-ql-expressions.data-2`
  - :doc:`./Examples/json-with-json-ql-expressions.data-3`
  - :doc:`./Examples/json-with-json-ql-expressions.data-4`

- The file :doc:`./Examples/json-with-json-ql-expressions.data-4` file with JsonQL expressions is shown below.

.. sourcecode:: json

     {
       "CountryNamesWithPopulationOf80MlnOrMore": "$value(Countries.Where(x => x.Population >= 80000000).Select(x => x.Name))",
       "CompanyDetailsWithAnyEmployeeWithNullAddress": "$value(Companies.Where(x => Any(x.Employees, x => x.Address is null)).Select(x => x.CompanyData))",
       "EmployeesWithNonEmptyLoginsList": "$value(Companies.Select(x => x.Employees.Where(x => Count(x.Logins) > 0)))",
       "FirstEmployeeWithLoginsField": "$value(Companies.Select(x => x.Employees.Where(x => HasField(x, 'Logins'))).First(x => x.Id != 100000005))",
       "LastEmployeeWithLoginsField": "$value(Companies.Select(x => x.Employees.Where(x => HasField(x, 'Logins'))).Last())",
       "FlattenExamples": {
         // Flattening arrays can be applied multiple times to execute sophisticated queries
         "FlattenArraysAndFilterObjects": "$value(MultiDimensionalMatrix.Flatten().Where(x => x > 3 || x.Name == 'John' || x.CompanyName starts with 'Sherwood'))",
         "GetAllNumericValuesInArraysFiltered": "$value(MultiDimensionalMatrix.Flatten().Flatten(x => typeof x != 'JsonArray' || All(x, y => y != 12)).Where(x => typeof x == 'Number'))"
       },
       // If CompanyData.CEO field is missing or value is null for second company, compilation will fail.
       "SelectSecondCompanyCeoNameWithAssertionThatCeoNameFieldIsPresetAndNotNull": "$(Companies[1].CompanyData.CEO assert)",

       // Example of merging fields into a JSON object. EnhancedCompanyData has fields in CompanyData for company with Name that starts with text 'Sherwood'
       // enhanced with additonal fields
       "EnhancedCompanyData": {
         "Capitalization": 1000000,
         "CompanyDataCopied": "$copyFields(Companies.First(x => x.CompanyData.Name starts with 'Sherwood').CompanyData)",
         "NumberOfEmployees": "$(Count(Companies.First(x => x.CompanyData.Name starts with 'Sherwood').Employees))"
       },

       // Example of merging array items into an array
       // List of company data for companies with  max salary of employees at least 110000 will be merged into array ListOfCompaniesEnhancedWithNewCompanies
       "ListOfCompaniesEnhancedWithNewCompanies": [
         {
           "Name": "Some company 1",
           "CEO": "Napoleon Bonaparte"
         },
         // Merge data of companies with max salary of employees at least 110000
         "$merge(Companies.Where(x => Max(x.Employees, y => true, y => y.Salary) > 110000).Select(x => x.CompanyData))",
         {
           "Name": "Some company 2",
           "CEO": "Georges Danton"
         }
       ],
       // Example of simple value using mutator function "$". In contrast to "$value" it generates a simple value (string, numeroc, etc) and not an object.
       "AverageSalaryOfAllEmployeesInFilteredCompanies": "$(Average(Companies.Where(x => !(x.CompanyData.Name starts with 'Sherwood')).Select(x => x.Employees.Select(x => x.Salary))))",
       "AddressingObjectsInMultiDimensionalMatrix": "$value(MultiDimensionalMatrix[1, 2].Address)",

       // Examples of using contextual data such a 'index' as well as example of referencing parent lamdba function parameter
       "EmployeeNamesInSecondCompanyExcludingFirstAndLastEmployee": "$value(Companies[1].Select(x => x.Employees.Where(y => index != 0 && index != Count(x.Employees) - 1).Select(x => x.Name)))",
       // We can use named parameters to change the order of parameters, as well as not specified optional parameter that are before parameters we pass
       // In this example both criteria and value are optional, and criteria is before value parameter. By using named parameter value, we can ommit criteria parameter
       "MaxRaisedSalaryAccrossAllCompaniesUsingNamedParameter": "$value(Max(Companies.Select(x => x.Employees).Flatten(), value->x=>x.Salary*1.05))",

       // isReverseSearch named parameter does not have to be used here, and we can use true here, since this parameter comes after the predicate. 
       // However, it makes the meaning clear.
       "ThirdEmployeeFromEndWithSalaryGreaterThan_100000_AcrossAllCompanies": "$value(Companies.Select(x => x.Employees).Flatten().At(2, x => x.Salary >= 100000 && x.Id != 100000008, isReverseSearch->true))",

       "UseOf_this_ToEnsureObjectIsLookedUpInCurrentFileAndNotInParentFiles": "$(this.CountryNamesWithPopulationOf80MlnOrMore[1])",
       "UseOf_parent_ToEnsureObjectIsLookedUpInParentFilesAndSearchInCurrentFilesIsSkipped": "$value(Count(parent.Companies, x => x.CompanyData.CEO != 'John Malkowich'))",
       "EmployeeWithNonNullIdAndNullAddress": "$value(Companies.Select(x => x.Employees).Flatten().Where(x => x.Address is null && x.Id is not null))",
       "NumberOfEmployeesWithout_Logins_Field": "$(Count(Companies.Select(x => x.Employees).Flatten().Where(x => x.Logins is undefined)))",
       "NumberOfEmployeesWith_Logins_Field": "$(Count(Companies.Select(x => x.Employees).Flatten().Where(x => x.Logins is not undefined)))",
       "InvalidValuesEvaluateToUndefined": "$(Companies[0].Employees[1000].Salary is undefined) = true",
       "ConversionExample_Datetime": "'2022-05-22T18:25:43.511Z' < '2022-05-23T18:25:43.511Z'=$(ToDateTime('2022-05-22T18:25:43.511Z') < ToDateTime('2022-05-23T18:25:43.511Z'))",
       "DefaultValue": "Defaulted salary to $(Companies[0].Employees[1000].Salary:100000)=100000"
     }


- C# code example that parses the JSON files above

.. sourcecode:: csharp

    var additionalTestData = new JsonTextData(
        "AdditionalTestData",
        this.LoadExampleJsonFile("AdditionalTestData.json"));

    var companiesJsonTextData = new JsonTextData("Companies",
        this.LoadExampleJsonFile("Companies.json"), countriesJsonTextData);

    // Set the value of queryManager to an instance of JsonQL.Compilation.JsonCompiler here.
    // The value of JsonQL.Compilation.JsonCompiler is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.
    JsonQL.Compilation.IJsonCompiler jsonCompiler = null!;

    var countriesJsonTextData = new JsonTextData(
        "Countries",
        this.LoadExampleJsonFile("Countries.json"), additionalTestData);
        
    var result = jsonCompiler.Compile(new JsonTextData("Overview",
        this.LoadExampleJsonFile("Overview.json"), companiesJsonTextData));


- Result of a query above for can be found here: :doc:`./Examples/json-with-json-ql-expressions.result`

  .. note::
    The serialized result in :doc:`./Examples/json-with-json-ql-expressions.result` was formatted to show only mutated JSON for :doc:`./Examples/json-with-json-ql-expressions.data-4` to make the file smaller.  

JsonQL queries of JSON Files with Result Converted to C# objects
================================================================

.. note::
    **This section is a short summary with couple of examples. For more details reference this section:** :doc:`QueryingJsonFiles/ResultAsCSharpObject/index`

- The overloaded methods **QueryObject** in interface `JsonQL.Query.IQueryManager <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryManager.cs>`_ and similarly named overloaded extension methods with generic parameter **QueryObject<T>** in `JsonQL.Query.QueryManagerExtensions <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/QueryManagerExtensions.cs>`_ can be used to to query one or more JSON files using a JsonQL query expressions.

.. note::
    - The extension methods with generic parameter **TQueryObject** are easier to use. The methods in `JsonQL.Query.IQueryManager <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryManager.cs>`_ might be easier to use with reflection.
    - Moving forward the extension methods will be discussed.

Example: Query and convert JSON to C# objects
---------------------------------------------

.. sourcecode:: csharp

    // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
    // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.
    IQueryManager queryManager = null!; 

    // NOTE: Data.json has a root JSON with a collection of employees. 
    // If the JSON had a JSON object with the "Employees" field, the
    // query would be: "Employees.Where(...)" instead of "Where(...)"
    var query = "Where(e => e.Id==100000006 || e.Id==100000007 || Any(EmployeeIds, p => p == e.Id))";

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
    var employeesResult =
        queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
                    new JsonTextData("Data",
                        this.LoadExampleJsonFile("Data.json"),
                        new JsonTextData("Parameters", this.LoadExampleJsonFile("Parameters.json"))),
            [false, false], new JsonConversionSettingsOverrides
            {
                TryMapJsonConversionType = (type, parsedJson) =>
                {
                    // If we always return null, or just do not set the value, of TryMapJsonConversionType
                    // IEmployee will always be bound to Employee
                    // In this example, we ensure that if parsed JSON has "Employees" field,
                    // then the default implementation of IManager (i.e., Manager) is used to
                    // deserialize the JSON.
                    // We can also specify Manager explicitly.
                    if (parsedJson.HasKey(nameof(IManager.Employees)))
                        return typeof(IManager);
                    return null;
                }
            });

    // This example is copied from https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IQueryManagerExamples/SuccessExamples/ResultAsObject/ResultAsNonNullableEmployeesList/Example.cs

- Files evaluated in JsonQL query above are listed here:
   - :doc:`./Examples/query-and-convert-json-to-csharp-objects.data-1`
   
- Result of query above can be found here: :doc:`./Examples/query-and-convert-json-to-csharp-objects.result`
- Example classes and JSON files for this example can be found `here <https://github.com/artakhak/JsonQL/tree/main/JsonQL.Demos/Examples/IQueryManagerExamples/SuccessExamples/ResultAsObject/ResultAsNonNullableEmployeesList>`_

Example: Query and convert JSON to collection of double values
--------------------------------------------------------------

- Files evaluated in JsonQL query below are listed here:
   - :doc:`./Examples/query-and-convert-json-to-collection-of-doubles.data-1`
        
.. sourcecode:: csharp

    // Set the value of queryManager to an instance of JsonQL.Compilation.JsonCompiler here.
    // The value of JsonQL.Compilation.JsonCompiler is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.
    IQueryManager queryManager = null!;

    var salariesOfAllEmployeesOlderThan35InAllCompaniesQuery = 
        "Companies.Select(x => x.Employees.Where(x => x.Age > 35).Select(x => x.Salary))";
        
    var salariesResult =
        queryManager.QueryObject<IReadOnlyList<double>>(salariesOfAllEmployeesOlderThan35InAllCompaniesQuery,
            new JsonTextData("Data",
                this.LoadExampleJsonFile("Data.json")), null);

- Result of query above can be found here: :doc:`./Examples/query-and-convert-json-to-collection-of-doubles.result`
- Example classes and JSON files for this example can be found `here <https://github.com/artakhak/JsonQL/tree/main/JsonQL.Demos/Examples/IQueryManagerExamples/SuccessExamples/ResultAsObject/SalariesOfAllEmployeesInAllCompaniesAsReadOnlyListOfDoubles>`_

JsonQL queries of JSON Files with Result Converted to JSON structure
====================================================================

.. note::
    **This section is a short summary with couple of examples. For more details reference this section:** :doc:`QueryingJsonFiles/ResultAsJsonStructure/index`

- The interface `JsonQL.Query.IQueryManager  <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryManager.cs>`_ and its extensions are used to query one or more JSON files using a JsonQL query expression.
- The result is converted to `JsonQL.Query.IJsonValueQueryResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IJsonValueQueryResult.cs>`_.
- The result stores the query result as a JSON structure as well as data about errors encountered during execution of the query.
- One ore more JSON files can be specified as parameters to be used when looking up JSON values referenced by JsonQL expressions.
- If many JSON files are specified the the following rules and techniques are used:
  - Parent/child relationships between JSON files is maintained and parent JSON files are evaluated before child JSON files are evaluated.
  - Lookup of JSON values specified in JsonQL expressions starts in JSON containing the expression first, and then in parent JSON files.


Example: Query JSON files with result as JSON structure
-------------------------------------------------------

- Files evaluated in JsonQL query below are listed here:
   - :doc:`./Examples/query-with-result-as-json-object-1.data`
        
.. sourcecode:: csharp

    // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
    // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.
    IQueryManager queryManager = null!; 

    var query = "Companies.Where(x => Max(x.Employees, value-> y => y.Salary) < 106000)";

    // companiesResult is of type 'JsonQL.Query.IJsonValueQueryResult'
    // that stores information about the loaded JSON as well as errors if any.
    IJsonValueQueryResult companiesResult =
                _queryManager.QueryJsonValue(query,
                    new JsonTextData("Data",
                        this.LoadExampleJsonFile("Data.json")));

- Result of query above can be found here: :doc:`./Examples/query-with-result-as-json-object.result`
- Example classes and JSON files for this example can be found `here <https://github.com/artakhak/JsonQL/tree/main/JsonQL.Demos/Examples/IQueryManagerExamples/SuccessExamples/ResultAsParsedJsonValue/CompaniesWithLimitOnMaxSalary>`_

.. toctree::

   MutatingJsonFiles/index.rst
   QueryingJsonFiles/index.rst
   LambdaFunctions/index.rst
   SpecialKeywords/index.rst
   JsonMutatorOperators/index.rst
   JsonPathFunctions/index.rst
   Functions/index.rst
   AggregateFunctions/index.rst
   Operators/index.rst
   OptionalAndNamedParameters/index.rst 
   DependencyInjectionSetup/index.rst
   CustomJsonQL/index.rst
   FutureReleases/index.rst

Indices and tables
==================

* :ref:`genindex`
* :ref:`modindex`
* :ref:`search`
