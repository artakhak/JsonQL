========
Examples
========

.. contents::
   :local:
   :depth: 2

Similar to how we did this in section :doc:`../../../MutatingJsonFiles/index` we will use the following four files in our examples

    .. note::
        :doc:`../../../MutatingJsonFiles/index` also uses Example.json, which has expressions that reference JSON objects in that file as well as the four files mentioned.

Lets consider example with the following four files

- :doc:`../SampleFiles/parameters` - a JSON file with lists "FilteredCountryNames" and "FilteredCompanyNames" referenced in other JSON files below in JsonQL expressions.
- :doc:`../SampleFiles/countries` - a JSON file for data on number of countries
- :doc:`../SampleFiles/companies` - a JSON file for data on number of companies
- :doc:`../SampleFiles/filtered-companies` - a JSON file shown below with JsonQL expressions that filter companies in :doc:`../SampleFiles/companies` to include only some companies using data in :doc:`../SampleFiles/parameters`.

Example of querying for a JSON array
====================================

- The code snippet below shows how to query for list of employees (JSON array) in JSON array **FilteredCompanies** in :doc:`../SampleFiles/filtered-companies` JSON file.
- :doc:`../SampleFiles/filtered-companies` itself has JsonQL expressions that reference "FilteredCompanyNames" array in :doc:`../SampleFiles/parameters` and **Companies** array in :doc:`../SampleFiles/companies`
- Parent/child relationship of queried files set up in variable **filteredCompaniesJsonTextData** in code snippet is set up as follows:
    
    - :doc:`../SampleFiles/filtered-companies` => :doc:`../SampleFiles/companies` => :doc:`../SampleFiles/countries` => :doc:`../SampleFiles/parameters`.
  
.. sourcecode:: csharp

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

    // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
    // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.
    JsonQL.Query.IQueryManager queryManager = null!;

    var employeesResult =
        queryManager.QueryJsonValue(query, filteredCompaniesJsonTextData);

    Assert.That(employeesResult, Is.Not.Null);
    Assert.That(employeesResult.CompilationErrors.Count, Is.EqualTo(0));
    Assert.That(employeesResult.ParsedValue, Is.Not.Null);
    Assert.That(employeesResult.ParsedValue, Is.InstanceOf<IParsedArrayValue>());
    Assert.That(((IParsedArrayValue)employeesResult.ParsedValue!).Values.Count, Is.EqualTo(9));
 
The result (an instance of `JsonQL.Query.IJsonValueQueryResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IJsonValueQueryResult.cs>`_) is serialized to a **Result.json** file below.

.. raw:: html

   <details>
   <summary>Click to expand the result of the query in example above (i.e., instance of <b>JsonQL.Query.IJsonValueQueryResult&lt;IReadOnlyList&lt;IEmployee&gt;&gt;</b>) serialized into <b>Result.json</b></summary>

.. code-block:: json

    {
      "ParsedValue":
      [
        {
          "Id":  100000001,
          "FirstName":  "John",
          "LastName":  "Smith",
          "Address": {
            "Street":  "456 Oak Avenue",
            "City":  "Chicago",
            "State":  "IL",
            "ZipCode":  "60601"
          },
          "Salary":  99500,
          "Age":  45,
          "Phones": [
            "312-555-0134",
            "312-555-0178"
          ]
        },
        {
          "Id":  100000002,
          "FirstName":  "Alice",
          "LastName":  "Johnson",
          "Address": {
            "Street":  "123 Maple Street",
            "City":  "New York",
            "State":  "NY",
            "ZipCode":  "10001"
          },
          "Salary":  105000,
          "Age":  38,
          "Phones": [
            "212-555-0199"
          ]
        },
        {
          "Id":  100000003,
          "FirstName":  "Michael",
          "LastName":  "Brown",
          "Address": {
            "Street":  "789 Pine Lane",
            "City":  "Los Angeles",
            "State":  "CA",
            "ZipCode":  "90001"
          },
          "Salary":  89000,
          "Age":  50,
          "Phones": [
          ]
        },
        {
          "Id":  100000004,
          "FirstName":  "Emily",
          "LastName":  "Davis",
          "Address": {
            "Street":  "321 Elm Drive",
            "City":  "Houston",
            "State":  "TX",
            "ZipCode":  "77001"
          },
          "Salary":  92000,
          "Age":  42,
          "Phones": [
            "713-555-0147",
            "713-555-0112"
          ]
        },
        {
          "Id":  100000008,
          "FirstName":  "Laura",
          "LastName":  "Lee",
          "Address": {
            "Street":  "258 Willow Lane",
            "City":  "San Diego",
            "State":  "CA",
            "ZipCode":  "92101"
          },
          "Salary":  105500,
          "Age":  32,
          "Phones": [
            "619-555-0155",
            "619-555-0122"
          ]
        },
        {
          "Id":  100000009,
          "FirstName":  "Andrew",
          "LastName":  "Harris",
          "Address": {
            "Street":  "369 Spruce Drive",
            "City":  "Dallas",
            "State":  "TX",
            "ZipCode":  "75201"
          },
          "Salary":  88000,
          "Age":  41,
          "Phones": [
            "214-555-0180"
          ]
        },
        {
          "Id":  100000010,
          "FirstName":  "Jessica",
          "LastName":  "Thompson",
          "Address": {
            "Street":  "159 Cherry Lane",
            "City":  "Austin",
            "State":  "TX",
            "ZipCode":  "73301"
          },
          "Salary":  98700,
          "Age":  37,
          "Phones": [
          ]
        },
        {
          "Id":  250150245,
          "FirstName":  "Jane",
          "LastName":  "Doe",
          "Address": {
            "Street":  "Main St",
            "City":  "San Jose",
            "State":  "PA",
            "ZipCode":  "95101"
          },
          "Salary":  144186,
          "Age":  63,
          "Phones": [
            "408-555-0133",
            "408-555-0190"
          ]
        },
        {
          "Id":  783328759,
          "FirstName":  "Robert",
          "LastName":  "Brown",
          "Address": {
            "Street":  "Pine St",
            "City":  "Los Angeles",
            "State":  "CA",
            "ZipCode":  "90001"
          },
          "Salary":  122395,
          "Age":  58,
          "Phones": [
            "323-555-0177"
          ]
        }
      ],
      "CompilationErrors":
      {
        "$type": "JsonQL.Compilation.ICompilationErrorItem[], JsonQL",
        "$values": []
      }
    }

.. raw:: html

   </details><br/><br/>

Example of querying for a JSON object
=====================================

- The code snippet below shows how to query for an employee (JSON object) in JSON array **FilteredCompanies** in :doc:`../SampleFiles/filtered-companies` JSON file.
- :doc:`../SampleFiles/filtered-companies` itself has JsonQL expressions that reference "FilteredCompanyNames" array in :doc:`../SampleFiles/parameters` and **Companies** array in :doc:`../SampleFiles/companies`
- Parent/child relationship of queried files set up in variable **filteredCompaniesJsonTextData** in code snippet is set up as follows:
    
    - :doc:`../SampleFiles/filtered-companies` => :doc:`../SampleFiles/companies` => :doc:`../SampleFiles/countries` => :doc:`../SampleFiles/parameters`.
  
.. sourcecode:: csharp

    string[] sharedExamplesFolderPath = ["DocFiles", "QueryingJsonFiles", "JsonFiles"];
            
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
    JsonQL.Query.IQueryManager queryManager = null!;

    var query = "FilteredCompanies.Select(c => c.Employees.Where(e => e.Name !=  'John Smith')).First(x => x.Age >= 40)";

    var employeeResult =
        queryManager.QueryJsonValue(query, filteredCompaniesJsonTextData);

    Assert.That(employeeResult, Is.Not.Null);
    Assert.That(employeeResult.CompilationErrors.Count, Is.EqualTo(0));
    Assert.That(employeeResult.ParsedValue, Is.Not.Null);

    // We don't care about the null / conversion exception in this example for brevity.
    var parsedJson = (IParsedJson)employeeResult.ParsedValue!;

    Assert.That(((IParsedSimpleValue)parsedJson["Id"].Value).Value, Is.EqualTo("100000001"));
 
The result (an instance of `JsonQL.Query.IJsonValueQueryResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IJsonValueQueryResult.cs>`_) is serialized to a **Result.json** file below.

.. raw:: html

   <details>
   <summary>Click to expand the result of the query in example above (i.e., instance of <b>JsonQL.Query.IJsonValueQueryResult&lt;IReadOnlyList&lt;IEmployee&gt;&gt;</b>) serialized into <b>Result.json</b></summary>

.. code-block:: json

    {
      "ParsedValue":
      {
        "Id":  100000001,
        "FirstName":  "John",
        "LastName":  "Smith",
        "Address": {
          "Street":  "456 Oak Avenue",
          "City":  "Chicago",
          "State":  "IL",
          "ZipCode":  "60601"
        },
        "Salary":  99500,
        "Age":  45,
        "Phones": [
          "312-555-0134",
          "312-555-0178"
        ]
      },
      "CompilationErrors":
      {
        "$type": "JsonQL.Compilation.ICompilationErrorItem[], JsonQL",
        "$values": []
      }
    }

.. raw:: html

   </details><br/><br/>
   
Example of querying for simple JSON value
=========================================

- The code snippet below shows how to query for average salary (simple JSON numeric value) in JSON array **Companies** in :doc:`../SampleFiles/companies` JSON file.
  
.. sourcecode:: csharp

    string[] sharedExamplesFolderPath = ["DocFiles", "QueryingJsonFiles", "JsonFiles"];
            
    var query =
        "Average(Companies.Select(c => c.Employees.Where(e => e.Name != 'John Smith').Select(e => e.Salary)))";

    // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
    // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.
    JsonQL.Query.IQueryManager queryManager = null!;

    var averageSalaryResult =
        queryManager.QueryJsonValue(query, new JsonTextData("Companies",
            LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath)));

    Assert.That(averageSalaryResult, Is.Not.Null);
    Assert.That(averageSalaryResult.CompilationErrors.Count, Is.EqualTo(0));
    Assert.That(averageSalaryResult.ParsedValue, Is.Not.Null);

    // We don't care about the null / conversion exception in this example for brevity.
    var parsedSimpleValue = (IParsedSimpleValue)averageSalaryResult.ParsedValue!;

    Assert.That(parsedSimpleValue.IsString, Is.False);
    Assert.That(parsedSimpleValue.Value, Is.EqualTo("102356.75"));

The result (an instance of `JsonQL.Query.IJsonValueQueryResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IJsonValueQueryResult.cs>`_) is serialized to a **Result.json** file below.

.. raw:: html

   <details>
   <summary>Click to expand the result of the query in example above (i.e., instance of <b>JsonQL.Query.IJsonValueQueryResult&lt;IReadOnlyList&lt;IEmployee&gt;&gt;</b>) serialized into <b>Result.json</b></summary>

.. code-block:: json

    {
      "ParsedValue": 102356.75,
      "CompilationErrors":
      {
        "$type": "JsonQL.Compilation.ICompilationErrorItem[], JsonQL",
        "$values": []
      }
    }

.. raw:: html

   </details><br/><br/>
