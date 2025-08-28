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

Example of using JsonQL expressions in JSON file **Overview.json** to mutate the JSON being parsed by **JsonQL**
================================================================================================================

  .. note::
          JsonQL Expressions start with '$'. Example "$value(Employees.Select(x => x.Salary >= 100000))".


- Files evaluated in JsonQL expressions in “Overview.json” file below are listed here:
    .. raw:: html

        <a href="https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IJsonCompilerExamples/SuccessExamples/Overview/Companies.json"><p class="codeSnippetRefText">Companies.json</p></a>
        <a href="https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IJsonCompilerExamples/SuccessExamples/Overview/Countries.json"><p class="codeSnippetRefText">Countries.json</p></a>
        <a href="https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IJsonCompilerExamples/SuccessExamples/Overview/AdditionalTestData.json"><p class="codeSnippetRefText">AdditionalTestData.json</p></a>

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


.. raw:: html

    <a href="https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IJsonCompilerExamples/SuccessExamples/Overview/Result.json"><p class="codeSnippetRefText">Click here to see the JSON generated from the JSON above </p></a>

**Here is a C# code example that evaluates the Json in file above**

.. sourcecode:: csharp

    var additionalTestData = new JsonTextData(
        "AdditionalTestData",
        this.LoadExampleJsonFile("AdditionalTestData.json"));

    var countriesJsonTextData = new JsonTextData(
        "Countries",
        this.LoadExampleJsonFile("Countries.json"), additionalTestData);

    var companiesJsonTextData = new JsonTextData("Companies",
        this.LoadExampleJsonFile("Companies.json"), countriesJsonTextData);

    // Create an instance of JsonQL.Compilation.JsonCompiler here.
    //This is normally done once on application start.
    JsonQL.Compilation.IJsonCompiler jsonCompiler = null!;

    var result = jsonCompiler.Compile(new JsonTextData("Overview",
        this.LoadExampleJsonFile("Overview.json"), companiesJsonTextData));



Example of querying a JSON data in one or more JSON files and converting the result to C# objects
=================================================================================================

- Files evaluated in JsonQL query below are listed here:
    .. raw:: html

        <a href="https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IQueryManagerExamples/SuccessExamples/ResultAsObject/ResultAsNonNullableEmployeesList/Data.json"><p class="codeSnippetRefText">Data.json</p></a>      
        
.. sourcecode:: csharp

    // NOTE: Data.json has a root JSON with a collection of employees. 
    // If the JSON had a JSON object with the "Employees" field, the
    // query would be: "Employees.Where(...)" instead of "Where(...)"
    var query = "Where(x => x.Id==100000006 || x.Id==100000007)";

    // Create an instance of JsonQL.Query.QueryManager here.
    // This is normally setup in DI normally using a singletone binding done on application start.
    IQueryManager queryManager = null!; 
                                        
    // We can convert to the following collection types:
    // -One of the following interfaces: IReadOnlyList<T>, IEnumerable<T>, IList<T>, 
    // ICollection<T>, IReadOnlyCollection<T>
    // -Any type that implements ICollection<T>. Example: List<T>,
    // -Array T[],
    // In these examples T is either an object (value or reference type) or another collection 
    // type (one of the listed here). 
    var employeesResult =
        queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
            new JsonTextData("Data",
                this.LoadExampleJsonFile("Data.json")),
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


.. raw:: html

    <a href="https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IQueryManagerExamples/SuccessExamples/ResultAsObject/ResultAsNonNullableEmployeesList/Result.json"><p class="codeSnippetRefText">Click here to see the JSON generated from the JSON above </p></a>


Example of querying a JSON data in one or more JSON files and converting the result of collection of double values
==================================================================================================================

- Files evaluated in JsonQL query below are listed here:
    .. raw:: html

        <a href="https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IQueryManagerExamples/SuccessExamples/ResultAsObject/SalariesOfAllEmployeesInAllCompaniesAsReadOnlyListOfDoubles/Data.json"><p class="codeSnippetRefText">Data.json</p></a>      
        
.. sourcecode:: csharp

    var salariesOfAllEmployeesOlderThan35InAllCompaniesQuery = 
        "Companies.Select(x => x.Employees.Where(x => x.Age > 35).Select(x => x.Salary))";

    // Create an instance of JsonQL.Query.QueryManager here.
    // This is normally done once on application start.
    IQueryManager queryManager = null!;

    var salariesResult =
        queryManager.QueryObject<IReadOnlyList<double>>(salariesOfAllEmployeesOlderThan35InAllCompaniesQuery,
            new JsonTextData("Data",
                this.LoadExampleJsonFile("Data.json")), null);

.. raw:: html

    <a href="https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Examples/IQueryManagerExamples/SuccessExamples/ResultAsObject/SalariesOfAllEmployeesInAllCompaniesAsReadOnlyListOfDoubles/Result.json"><p class="codeSnippetRefText">Click here to see the JSON generated from the JSON above </p></a>


.. toctree::

   json-file-mutation/index.rst

Indices and tables
==================

* :ref:`genindex`
* :ref:`modindex`
* :ref:`search`