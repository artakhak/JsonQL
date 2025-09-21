============================================
Nullable Syntax in Queried Object Properties
============================================

.. contents::
   :local:
   :depth: 2

- Nullable syntax can be used with model class property types (with both C# reference types as well as value types such as **System.Double**) for classes used as return types in query result. 
- Nullable syntax can be used for item types as well in model class properties that are of one of the following types: 
    - System.Collections.Generic.List<T>
    - System.Collections.Generic.IReadOnlyList<T>
    - System.Collections.Generic.IEnumerable<T>
    - Arrays
- JsonQL will evaluate all non-nullable property values and will report conversion errors of type `JsonQL.JsonToObjectConversion.IConversionError <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IConversionError.cs>`_ in result with value of **ErrorType** equal to one of the following enum values: **NonNullablePropertyNotSet**, **NonNullableCollectionItemValueNotSet**.

In example below we query for a non-nullable collection of non-nullable **IEmployee** items. 

- If the query results in **IEmployee** instances with null value for **Address** property of non-nullable type **IAddress**, of if any instance of **IEmployee** in query result has null value for **IEmployee.Address.Street** (**IAddress.Street** is of non-nullable **string** type), JsonQL will report conversion error(s) of type `JsonQL.JsonToObjectConversion.IConversionError <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IConversionError.cs>`_ in result with value of **ErrorType** equal to one of the following enum values: **NonNullablePropertyNotSet**.

.. raw:: html

   <details>
   <summary>Click to expand <b>Company.json</b> used in query in example below</summary>

.. code-block:: json

    {
      "Companies": [
        {
          "CompanyData": {
            "Name": "Strange Things, Inc",
            "CEO": "John Malkowich",
            "Address": {
              "Street": "123 Maple Street",
              "City": "Springfield",
              "State": "IL",
              "ZipCode": "62701"
            },
            "CountryDetails": "$value(Countries.Where(x => x.Name == 'United States'))"
          },
          "Employees": [
            {
              "Id": 100000001,
              "FirstName": "John",
              "LastName": "Smith",
              "Address": {
                "Street": "456 Oak Avenue",
                "City": "Chicago",
                "State": "IL",
                "ZipCode": "60601"
              },
              "Salary": 99500,
              "Age": 45,
              "Phones": [
                "312-555-0134",
                "312-555-0178"
              ]
            },
            {
              "Id": 100000002,
              "FirstName": "Alice",
              "LastName": "Johnson",
              "Address": {
                "Street": "123 Maple Street",
                "City": "New York",
                "State": "NY",
                "ZipCode": "10001"
              },
              "Salary": 105000,
              "Age": 38,
              "Phones": [
                "212-555-0199"
              ]
            },
            {
              "Id": 100000003,
              "FirstName": "Michael",
              "LastName": "Brown",
              "Address": {
                "Street": "789 Pine Lane",
                "City": "Los Angeles",
                "State": "CA",
                "ZipCode": "90001"
              },
              "Salary": 89000,
              "Age": 50,
              "Phones": []
            },
            {
              "Id": 100000004,
              "FirstName": "Emily",
              "LastName": "Davis",
              "Address": {
                "Street": "321 Elm Drive",
                "City": "Houston",
                "State": "TX",
                "ZipCode": "77001"
              },
              "Salary": 92000,
              "Age": 42,
              "Phones": [
                "713-555-0147",
                "713-555-0112"
              ]
            }
          ]
        },
        {
          "CompanyData": {
            "Name": "Sherwood Forest Timber, Inc",
            "CEO": "Robin Wood",
            "Address": {
              "Street": "789 Pine Lane",
              "City": "Denver",
              "State": "CO",
              "ZipCode": "80203"
            },
            "CountryDetails": "$value(Countries.Where(x => x.Name == 'Germany'))"
          },
          "Employees": [
            {
              "Id": 100000005,
              "FirstName": "Christopher",
              "LastName": "Garcia",
              "Address": {
                "Street": "654 Cedar Road",
                "City": "Phoenix",
                "State": "AZ",
                "ZipCode": "85001"
              },
              "Salary": 111000,
              "Age": 29,
              "Logins": [
                "cgarcia@sherwood.com",
                "cgarcia@gmail.com"
              ],
              "Phones": [
                "602-555-0166",
                "602-555-0188"
              ]
            },
            {
              "Id": 100000006,
              "FirstName": "Sarah",
              "LastName": "Wilson",
              "Address": null,
              "Salary": 78000,
              "Age": 35,
              "Phones": []
            },
            {
              "Id": 100000007,
              "FirstName": "David",
              "LastName": "Martinez",
              "Address": {
                "Street": "147 Birch Street",
                "City": "San Antonio",
                "State": "TX",
                "ZipCode": "78201"
              },
              "Salary": 95000,
              "Age": 46,
              "Logins": [
                "dmartinez@sherwood.com",
                "dmartinez@gmail.com"
              ],
              "Phones": [
                "210-555-0123"
              ]
            }
          ]
        },
        {
          "CompanyData": {
            "Name": "Atlantic Transfers, Inc",
            "CEO": "Black Beard",
            "Address": {
              "Street": "101 Elm Drive",
              "City": "Dallas",
              "State": "TX",
              "ZipCode": "75201"
            },
            "CountryDetails": "$value(Countries.Where(x => x.Name == 'United States'))"
          },
          "Employees": [
            {
              "Id": 100000008,
              "FirstName": "Laura",
              "LastName": "Lee",
              "Address": {
                "Street": "258 Willow Lane",
                "City": "San Diego",
                "State": "CA",
                "ZipCode": "92101"
              },
              "Salary": 105500,
              "Age": 32,
              "Phones": [
                "619-555-0155",
                "619-555-0122"
              ]
            },
            {
              "Id": 100000009,
              "FirstName": "Andrew",
              "LastName": "Harris",
              "Address": {
                "Street": "369 Spruce Drive",
                "City": "Dallas",
                "State": "TX",
                "ZipCode": "75201"
              },
              "Salary": 88000,
              "Age": 41,
              "Phones": [
                "214-555-0180"
              ]
            },
            {
              "Id": 100000010,
              "FirstName": "Jessica",
              "LastName": "Thompson",
              "Address": {
                "Street": "159 Cherry Lane",
                "City": "Austin",
                "State": "TX",
                "ZipCode": "73301"
              },
              "Salary": 98700,
              "Age": 37,
              "Phones": []
            },
            {
              "Id": 250150245,
              "FirstName": "Jane",
              "LastName": "Doe",
              "Address": {
                "Street": "Main St",
                "City": "San Jose",
                "State": "PA",
                "ZipCode": "95101"
              },
              "Salary": 144186,
              "Age": 63,
              "Phones": [
                "408-555-0133",
                "408-555-0190"
              ]
            },
            {
              "Id": 783328759,
              "FirstName": "Robert",
              "LastName": "Brown",
              "Address": {
                "Street": "Pine St",
                "City": "Los Angeles",
                "State": "CA",
                "ZipCode": "90001"
              },
              "Salary": 122395,
              "Age": 58,
              "Phones": [
                "323-555-0177"
              ]
            }
          ]
        }
      ]
    }


.. raw:: html

   </details><br/><br/>
   
.. sourcecode:: csharp

     // Select the employees in all companies with non-null value for Address
     var query = 
         "Companies.Select(c => c.Employees).Where(e => e.Address is not null)";

     // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
     // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
     // and it is normally configured as a singleton.
     JsonQL.Query.IQueryManager queryManager = null!;

     // Employees in employeesResult.Value of type IReadOnlyList<IEmployee> can have null for 
     // property IEmployee.Manager since this property is of type "IManager?" (uses nullable syntax)
     // Also, the values of IEmployee.Address.County can be null in result too
     // since the property IAddress.County is of type "string?" (nullable string)
     var employeesResult =
         queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
             new JsonTextData("Companies",
                 LoadJsonFileHelpers.LoadJsonFile("Companies.json", ["DocFiles", "QueryingJsonFiles", "JsonFiles"])));

.. raw:: html

   <details>
   <summary>Click to expand the result of the query in example above (i.e., instance of <b>JsonQL.Query.IJsonValueQueryResult&lt;IReadOnlyList&lt;IEmployee&gt;&gt;</b>) serialized into <b>Result.json</b></summary>

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
            "Id": 100000005,
            "FirstName": "Christopher",
            "LastName": "Garcia",
            "Address": {
              "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
              "Street": "654 Cedar Road",
              "City": "Phoenix",
              "State": "AZ",
              "ZipCode": "85001",
              "County": null
            },
            "Salary": 111000,
            "Age": 29,
            "Manager": null,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": [
                "602-555-0166",
                "602-555-0188"
              ]
            }
          },
          {
            "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
            "Id": 100000007,
            "FirstName": "David",
            "LastName": "Martinez",
            "Address": {
              "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
              "Street": "147 Birch Street",
              "City": "San Antonio",
              "State": "TX",
              "ZipCode": "78201",
              "County": null
            },
            "Salary": 95000,
            "Age": 46,
            "Manager": null,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": [
                "210-555-0123"
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
