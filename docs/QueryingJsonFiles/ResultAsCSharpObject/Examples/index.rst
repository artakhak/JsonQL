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

Example of querying for a C# reference type object
==================================================

- The code snippet below shows how to query for list of employees of type **System.Collections.Generic.IReadOnlyList<IEmployee>** in JSON array **FilteredCompanies** in :doc:`../SampleFiles/filtered-companies` JSON file.
- :doc:`../SampleFiles/filtered-companies` itself has JsonQL expressions that reference "FilteredCompanyNames" array in :doc:`../SampleFiles/parameters` and **Companies** array in :doc:`../SampleFiles/companies`
- Parent/child relationship of queried files set up in variable **filteredCompaniesJsonTextData** in code snippet is set up as follows:
    
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
    JsonQL.Query.IQueryManager queryManager = null!;

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

The result (an instance of `JsonQL.Query.IObjectQueryResult[IReadOnlyList[IEmployee]] <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IObjectQueryResult.cs>`_) is serialized to a **Result.json** file below.

.. raw:: html

   <details>
   <summary>Click to expand the result of the query in example above (i.e., instance of <b>JsonQL.Query.IObjectQueryResult&lt;IReadOnlyList&lt;IEmployee&gt;&gt;</b>) serialized into <b>Result.json</b></summary>

.. code-block:: json

    {
      "$type": "JsonQL.Query.ObjectQueryResult`1[[System.Collections.Generic.IReadOnlyList`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib]], JsonQL",
      "Value": {
        "$type": "System.Collections.Generic.List`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib",
        "$values": [
          {
            "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
            "Id": 100000001,
            "FirstName": "John",
            "LastName": "Smith",
            "Address": {
              "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
              "Street": "456 Oak Avenue",
              "City": "Chicago",
              "State": "IL",
              "ZipCode": "60601",
              "County": null
            },
            "Salary": 99500,
            "Age": 45,
            "Manager": null,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": [
                "312-555-0134",
                "312-555-0178"
              ]
            }
          },
          {
            "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
            "Id": 100000002,
            "FirstName": "Alice",
            "LastName": "Johnson",
            "Address": {
              "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
              "Street": "123 Maple Street",
              "City": "New York",
              "State": "NY",
              "ZipCode": "10001",
              "County": null
            },
            "Salary": 105000,
            "Age": 38,
            "Manager": null,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": [
                "212-555-0199"
              ]
            }
          },
          {
            "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
            "Id": 100000003,
            "FirstName": "Michael",
            "LastName": "Brown",
            "Address": {
              "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
              "Street": "789 Pine Lane",
              "City": "Los Angeles",
              "State": "CA",
              "ZipCode": "90001",
              "County": null
            },
            "Salary": 89000,
            "Age": 50,
            "Manager": null,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": []
            }
          },
          {
            "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
            "Id": 100000004,
            "FirstName": "Emily",
            "LastName": "Davis",
            "Address": {
              "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
              "Street": "321 Elm Drive",
              "City": "Houston",
              "State": "TX",
              "ZipCode": "77001",
              "County": null
            },
            "Salary": 92000,
            "Age": 42,
            "Manager": null,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": [
                "713-555-0147",
                "713-555-0112"
              ]
            }
          },
          {
            "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
            "Id": 100000008,
            "FirstName": "Laura",
            "LastName": "Lee",
            "Address": {
              "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
              "Street": "258 Willow Lane",
              "City": "San Diego",
              "State": "CA",
              "ZipCode": "92101",
              "County": null
            },
            "Salary": 105500,
            "Age": 32,
            "Manager": null,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": [
                "619-555-0155",
                "619-555-0122"
              ]
            }
          },
          {
            "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
            "Id": 100000009,
            "FirstName": "Andrew",
            "LastName": "Harris",
            "Address": {
              "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
              "Street": "369 Spruce Drive",
              "City": "Dallas",
              "State": "TX",
              "ZipCode": "75201",
              "County": null
            },
            "Salary": 88000,
            "Age": 41,
            "Manager": null,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": [
                "214-555-0180"
              ]
            }
          },
          {
            "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
            "Id": 100000010,
            "FirstName": "Jessica",
            "LastName": "Thompson",
            "Address": {
              "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
              "Street": "159 Cherry Lane",
              "City": "Austin",
              "State": "TX",
              "ZipCode": "73301",
              "County": null
            },
            "Salary": 98700,
            "Age": 37,
            "Manager": null,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": []
            }
          },
          {
            "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
            "Id": 250150245,
            "FirstName": "Jane",
            "LastName": "Doe",
            "Address": {
              "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
              "Street": "Main St",
              "City": "San Jose",
              "State": "PA",
              "ZipCode": "95101",
              "County": null
            },
            "Salary": 144186,
            "Age": 63,
            "Manager": null,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": [
                "408-555-0133",
                "408-555-0190"
              ]
            }
          },
          {
            "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
            "Id": 783328759,
            "FirstName": "Robert",
            "LastName": "Brown",
            "Address": {
              "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
              "Street": "Pine St",
              "City": "Los Angeles",
              "State": "CA",
              "ZipCode": "90001",
              "County": null
            },
            "Salary": 122395,
            "Age": 58,
            "Manager": null,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": [
                "323-555-0177"
              ]
            }
          }
        ]
      },
      "ErrorsAndWarnings": {
        "$type": "JsonQL.Query.QueryResultErrorsAndWarnings, JsonQL",
        "CompilationErrors": {
          "$type": "JsonQL.Compilation.ICompilationErrorItem[], JsonQL",
          "$values": []
        },
        "ConversionErrors": {
          "$type": "JsonQL.JsonToObjectConversion.ConversionErrors, JsonQL",
          "Errors": {
            "$type": "System.Collections.Generic.List`1[[JsonQL.JsonToObjectConversion.IConversionError, JsonQL]], System.Private.CoreLib",
            "$values": []
          }
        },
        "ConversionWarnings": {
          "$type": "JsonQL.JsonToObjectConversion.ConversionErrors, JsonQL",
          "Errors": {
            "$type": "System.Collections.Generic.List`1[[JsonQL.JsonToObjectConversion.IConversionError, JsonQL]], System.Private.CoreLib",
            "$values": []
          }
        }
      }
    }

.. raw:: html

   </details><br/><br/>
   
Example of querying for value type value
========================================

- The code snippet below shows how to query for average salary as  **System.Double** value in JSON array **Companies** in :doc:`../SampleFiles/companies` JSON file.
  
.. sourcecode:: csharp

    string[] sharedExamplesFolderPath = ["DocFiles", "QueryingJsonFiles", "JsonFiles"];

    var query =
          "Average(Companies.Select(c => c.Employees.Where(e => e.Name != 'John Smith').Select(e => e.Salary)))";

    // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
    // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.
    JsonQL.Query.IQueryManager queryManager = null!;

    var averageSalaryResult =
          queryManager.QueryObject<double>(query, new JsonTextData("Companies",
              LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath)),
              convertedValueNullability: [false]);
         
    LogHelper.Context.Log.InfoFormat("Average salary is {0}", averageSalaryResult.Value);

The result (an instance of `JsonQL.Query.IObjectQueryResult[double] <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IObjectQueryResult.cs>`_) is serialized to a **Result.json** file below.

.. raw:: html

   <details>
   <summary>Click to expand the result of the query in example above (i.e., instance of <b>JsonQL.Query.IObjectQueryResult&lt;IReadOnlyList&lt;IEmployee&gt;&gt;</b>) serialized into <b>Result.json</b></summary>

.. code-block:: json

    {
      "$type": "JsonQL.Query.ObjectQueryResult`1[[System.Double, System.Private.CoreLib]], JsonQL",
      "Value": 102356.75,
      "ErrorsAndWarnings": {
        "$type": "JsonQL.Query.QueryResultErrorsAndWarnings, JsonQL",
        "CompilationErrors": {
          "$type": "JsonQL.Compilation.ICompilationErrorItem[], JsonQL",
          "$values": []
        },
        "ConversionErrors": {
          "$type": "JsonQL.JsonToObjectConversion.ConversionErrors, JsonQL",
          "Errors": {
            "$type": "System.Collections.Generic.List`1[[JsonQL.JsonToObjectConversion.IConversionError, JsonQL]], System.Private.CoreLib",
            "$values": []
          }
        },
        "ConversionWarnings": {
          "$type": "JsonQL.JsonToObjectConversion.ConversionErrors, JsonQL",
          "Errors": {
            "$type": "System.Collections.Generic.List`1[[JsonQL.JsonToObjectConversion.IConversionError, JsonQL]], System.Private.CoreLib",
            "$values": []
          }
        }
      }
    }

.. raw:: html

   </details><br/><br/>
