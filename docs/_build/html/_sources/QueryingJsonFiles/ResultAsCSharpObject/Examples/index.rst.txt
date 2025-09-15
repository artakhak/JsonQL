========
Examples
========

.. contents::
   :local:
   :depth: 2

Similar to how we did this in section :doc:`../../../MutatingJsonFiles/index` we will use the following four files in our examples

    .. note::
        :doc:`../../../MutatingJsonFiles/index` also uses Example.json, which has expressions that reference JSON objects in that file as well as the four files mentioned.

lets consider example with the following four files

- :doc:`../SampleFiles/parameters` - a JSON file with lists "FilteredCountryNames" and "FilteredCompanyNames" referenced in other JSON files below in JsonQL expressions.
- :doc:`../SampleFiles/countries` - a JSON file for data on number of countries
- :doc:`../SampleFiles/companies` - a JSON file for data on number of companies
- :doc:`../SampleFiles/filtered-companies` - a JSON file shown below with JsonQL expressions that filter companies in :doc:`../SampleFiles/companies` to include only some companies using data in :doc:`../SampleFiles/parameters`.

Example of querying for a C# reference type object
==================================================

- The code snippet below shows how to query for list of employees of type **System.Collections.Generic.IReadOnlyList<IEmployee>** in JSON array **FilteredCompanies** in :doc:`../SampleFiles/filtered-companies` JSON file.
- :doc:`../SampleFiles/filtered-companies` itself has JsonQL expressions that reference "FilteredCompanyNames" array in :doc:`../SampleFiles/parameters` and **Companies** array in :doc:`../SampleFiles/companies`
- Parent/child relationship of queried files set up in **filteredCompaniesJsonTextData** in code snippet is set up as follows:
    
    - :doc:`../SampleFiles/filtered-companies` => :doc:`../SampleFiles/companies` => :doc:`../SampleFiles/countries` => :doc:`../SampleFiles/parameters`.
  
.. sourcecode:: csharp

    var sharedExamplesFolderPath = new string[]
    {
        "DocFiles", "MutatingJsonFiles", "Examples"
    };

    var parametersJsonTextData = new JsonTextData("Parameters",
        LoadJsonFileHelpers.LoadJsonFile("Parameters.json", sharedExamplesFolderPath));

    var countriesJsonTextData = new JsonTextData("Countries",
        LoadJsonFileHelpers.LoadJsonFile("Countries.json", sharedExamplesFolderPath), parametersJsonTextData);

    var companiesJsonTextData = new JsonTextData("Companies",
        LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath), countriesJsonTextData);

    var filteredCompaniesJsonTextData = new JsonTextData("FilteredCompanies",
        LoadJsonFileHelpers.LoadJsonFile("FilteredCompanies.json", sharedExamplesFolderPath), companiesJsonTextData);       

    // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
    // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.

    // We can call queryManager.QueryObject<T> with the following values for "T" generic parameter
    // -Class (value or reference type). We can use '?' for nullable values. Examples:
    //      "queryManager.QueryObject<Manager?>(...)",
    //      "queryManager.QueryObject<Manager>(...)"
    // -Interface. We can use '?' for nullable values. Examples:
    //      "queryManager.QueryObject<IManager?>(...)",
    //      "queryManager.QueryObject<IManager>(...)"
    // The following collection types:
    //          IReadOnlyList<T>, IEnumerable<T>, IList<T>, 
    //          ICollection<T>, IReadOnlyCollection<T>
    // -Any type that implements ICollection<T>. Example: List<T>, Array T[]
    // If collection type is used for "T", "T" can be either an object (value or reference type)
    // or another collection listed above. Also, nullability keyword "?" can be used for
    // collection items as well as for collection type itself.

    var query = "FilteredCompanies.Select(c => c.Employees.Where(e => e.Name !=  'John Smith'))";

    var employeesResult =
        queryManager.QueryObject<IReadOnlyList<IEmployee>>(query, filteredCompaniesJsonTextData);

    // The result "employeesResult" is of type "JsonQL.Query.IObjectQueryResult<IReadOnlyList<IEmployee>>".
    // The value employeesResult.Value contains the result of the query and is of type IReadOnlyList<IEmployee>.

    LogHelper.Context.Log.InfoFormat("Number of employees is {0}", employeesResult.Value?.Count ?? 
                                         throw new ApplicationException(
                                             $"Query failed. The serialized [{nameof(employeesResult)}] has the error details."));
 
- The serialized value of **employeesResult** can be found here :doc:`../SampleFiles/QueryReferenceTypeValue/result`

   
Example of querying for value type value
========================================

- The code snippet below shows how to query for average salary as  **System.Double** value in JSON array **Companies** in :doc:`../SampleFiles/companies` JSON file.
  
.. sourcecode:: csharp

    string[] sharedExamplesFolderPath = ["DocFiles", "QueryingJsonFiles", "JsonFiles"];

    // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
    // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.

    var query =
          "Average(Companies.Select(c => c.Employees.Where(e => e.Name != 'John Smith').Select(e => e.Salary)))";

    var averageSalaryResult =
        _queryManager.QueryObject<IReadOnlyList<IEmployee>>(query, new JsonTextData("Companies",
            LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath)));

    LogHelper.Context.Log.InfoFormat("Average salary is {0}", averageSalaryResult.Value);

- The serialized value of **employeesResult** can be found here :doc:`../SampleFiles/QueryValueTypeValue/result`
