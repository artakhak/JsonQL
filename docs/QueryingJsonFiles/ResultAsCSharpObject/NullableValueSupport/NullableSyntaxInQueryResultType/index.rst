====================================
Nullable Syntax in Query Result Type
====================================

.. contents::
   :local:
   :depth: 3

- To specify return type nullability we should pass non-null optional parameter **convertedValueNullability** in calls to overloaded method **JsonQL.Query.IQueryManager.QueryObject<TQueriedObject>(...)**.
- **convertedValueNullability** parameter type is a collection of boolean values with the following meaning: The first value indicates if return value can be null, the second value indicates if items in collection in returned value can be null, the third value indicates if items in collections of collection in returned type can be null and so forth (examples below will clarify this).
- The nullability syntax ("?" symbols) used with queried object (i.e., generic parameter type value used for **TQueriedObject** in call to **JsonQL.Query.IQueryManager.QueryObject<TQueriedObject>(...)**) should correspond to values in parameter **convertedValueNullability**, however only the values in **convertedValueNullability** will result in errors being reported if values are null.
    
    .. note::
        We normally should not have to use parameter **convertedValueNullability** and should instead just derive nullability from the return type (using "?" syntax attributes), but during development of JsonQL it was difficult to derive this information for reference return types (this is no issue for nullable value types). This information is derived currently for nullable properties, but not reference return types. In tne future we might be able to derive nullability for return reference types as well, and the parameter might be removed at that point.

- If optional parameter **convertedValueNullability** is not used or is null, return type is assumed to be nullable (both type as well as collection item types). In other words, no errors will be reported if value is null or any collection item value is null. 
- If the returned type is specified to have non-nullable, and the value is missing, there will be conversion error(s) of type `JsonQL.JsonToObjectConversion.IConversionError <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IConversionError.cs>`_ in result with value of **ErrorType** equal to **ValueNotSet**.
- If a collection item type nullability is specified as non-nullable, and collection item is not set in returned value, there will be conversion error(s) of type `JsonQL.JsonToObjectConversion.IConversionError <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IConversionError.cs>`_ in result with value of **ErrorType** equal to **NonNullableCollectionItemValueNotSet**.

Examples
--------

Result as Value Type
~~~~~~~~~~~~~~~~~~~~

- In the example below the query result is expected to be of non-nullable type **double**. 
- If the result is null, there will be conversion error(s) of type `JsonQL.JsonToObjectConversion.IConversionError <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IConversionError.cs>`_ in result with value of **ErrorType** equal to **ValueNotSet**. 

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

     // Select average salary of all employees across all companies
             var query =
                  "Average(Companies.Select(c => c.Employees.Select(e => e.Salary)))";

     // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
     // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
     // and it is normally configured as a singleton.
     JsonQL.Query.IQueryManager queryManager = null!;

     // The result "averageSalaryResult" is a non-nullable double value according to value
     // used for parameter "convertedValueNullability"
     var averageSalaryResult =
         queryManager.QueryObject<double>(query,
             new JsonTextData("Companies",
                 LoadJsonFileHelpers.LoadJsonFile("Companies.json", ["DocFiles", "QueryingJsonFiles", "JsonFiles"])),
             convertedValueNullability: [
                 // The result of type "double" cannot be null. An error will be reported if the value is null.
                 false]);

.. raw:: html

   <details>
   <summary>Click to expand the result of the query in example above (i.e., instance of <b>JsonQL.Query.IObjectQueryResult&lt;double&gt;</b>) serialized into <b>Result.json</b></summary>

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


Result as Reference Type
~~~~~~~~~~~~~~~~~~~~~~~~

- In the example below the query result is expected to be of non-nullable type **IEmployee**. 
- If the result is null, there will be conversion error(s) of type `JsonQL.JsonToObjectConversion.IConversionError <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IConversionError.cs>`_ in result with value of **ErrorType** equal to **ValueNotSet**. 

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

     // Select the first employee older than 40 in company with CompanyData.Name=='Atlantic Transfers, Inc'
     var query = 
         "Companies.Where(x => x.CompanyData.Name=='Atlantic Transfers, Inc').Select(c => c.Employees).First(e => e.Age > 40)";

     // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
     // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
     // and it is normally configured as a singleton.
     JsonQL.Query.IQueryManager queryManager = null!;

     var employeeResult =
         queryManager.QueryObject<IEmployee>(query,
             new JsonTextData("Companies",
                 LoadJsonFileHelpers.LoadJsonFile("Companies.json", ["DocFiles", "QueryingJsonFiles", "JsonFiles"])),
             convertedValueNullability: [
                 // The result of type "IEmployee" cannot be null. An error will be reported if the value is null.
                 false]);

.. raw:: html

   <details>
   <summary>Click to expand the result of the query in example above (i.e., instance of <b>JsonQL.Query.IObjectQueryResult&lt;IEmployee&gt;</b>) serialized into <b>Result.json</b></summary>

.. code-block:: json

    {
      "$type": "JsonQL.Query.ObjectQueryResult`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], JsonQL",
      "Value": {
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
   
Result as List of Value Type Items
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

In the example below the query result is expected to be of type **IReadOnlyList<double?>?**. The result can be  null and items in returned collection can be null as well in this example. 

- If we use **[false, true]** for **convertedValueNullability** and the query result is null, there will be conversion error(s) of type `JsonQL.JsonToObjectConversion.IConversionError <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IConversionError.cs>`_ in result with value of **ErrorType** equal to **ValueNotSet**.
- If we use **[true, false]** for **convertedValueNullability** and the query result is not null, but there are null values in collection, there will be conversion error(s) of type `JsonQL.JsonToObjectConversion.IConversionError <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IConversionError.cs>`_ in result with value of **ErrorType** equal to **NonNullableCollectionItemValueNotSet**.

.. raw:: html

   <details>
   <summary>Click to expand <b>Data.json</b> used in query in example below</summary>

.. code-block:: json

    {
      "ListOfListsOfDoubles": [
        [
          15,
          13,
          null,
          18
        ],
        [
          14,
          6,
          7
        ]
      ]
    }


.. raw:: html

   </details><br/><br/>
   
.. sourcecode:: csharp

     var query = "ListOfListsOfDoubles.Flatten().Where(x => x is null || x > 10)";

     // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
     // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
     // and it is normally configured as a singleton.
     JsonQL.Query.IQueryManager queryManager = null!;

     // The result "listOfNumbersResult" is a nullable list of nullable double values. 
     // Result of type "IReadOnlyList<double?>?" can be null, and each double value
     // in list "IReadOnlyList<double?>" can be null according to value used for parameter
     // "convertedValueNullability"
     var listOfNumbersResult =
         queryManager.QueryObject<IReadOnlyList<double?>?>(query,
             new JsonTextData("Data",
                 this.LoadExampleJsonFile("Data.json")),
             convertedValueNullability: [
                 // The result of type "IReadOnlyList<double?>?" can be null.
                 false,
                 // "double" values in list "IReadOnlyList<double?>" can be null.
                 true]);

.. raw:: html

   <details>
   <summary>Click to expand the result of the query in example above (i.e., instance of <b>JsonQL.Query.IObjectQueryResult&lt;double&gt;</b>) serialized into <b>Result.json</b></summary>

.. code-block:: json

    {
      "$type": "JsonQL.Query.ObjectQueryResult`1[[System.Collections.Generic.IReadOnlyList`1[[System.Nullable`1[[System.Double, System.Private.CoreLib]], System.Private.CoreLib]], System.Private.CoreLib]], JsonQL",
      "Value": {
        "$type": "System.Collections.Generic.List`1[[System.Nullable`1[[System.Double, System.Private.CoreLib]], System.Private.CoreLib]], System.Private.CoreLib",
        "$values": [
          15.0,
          13.0,
          null,
          18.0,
          14.0
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
   
Result as Arrays of Reference Type Items
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

- In the example below the query result is expected to be of type **IEmployee?[]**. The result cannot be null and **IEmployee** items in returned collection can be null in this example. 
- If the returned array is null, there will be conversion error(s) of type `JsonQL.JsonToObjectConversion.IConversionError <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IConversionError.cs>`_ in result with value of **ErrorType** equal to **ValueNotSet**.

.. raw:: html

   <details>
   <summary>Click to expand <b>Employees.json</b> used in query in example below</summary>

.. code-block:: json

    {
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
        null,
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
        }
      ]
    }


.. raw:: html

   </details><br/><br/>
   
.. sourcecode:: csharp

     var query = "Employees.Where(e => e.Id != 250150245)";

     // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
     // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
     // and it is normally configured as a singleton.
     JsonQL.Query.IQueryManager queryManager = null!;

     // The result "employeesResult" is a nullable list of nullable IEmployee values. 
     // Result of type "IEmployee?[]" cannot be null, and each IEmployee
     // array "IEmployee?[]" can be null according to value used for parameter
     // "convertedValueNullability"
     var employeesResult =
         queryManager.QueryObject<IEmployee?[]>(query,
             new JsonTextData("Employees",
                 this.LoadExampleJsonFile("Employees.json")),
             convertedValueNullability: [
                 // The result of type "IEmployee?[]" cannot be null. Ann error will be reported if the value is null
                 false,
                 // "IEmployee" items in list "IEmployee?[]" can be null.
                 true]);

.. raw:: html

   <details>
   <summary>Click to expand the result of the query in example above (i.e., instance of <b>IEmployee?[]</b>) serialized into <b>Result.json</b></summary>

.. code-block:: json

    {
      "$type": "JsonQL.Query.ObjectQueryResult`1[[JsonQL.Demos.Examples.DataModels.IEmployee[], JsonQL.Demos]], JsonQL",
      "Value": {
        "$type": "JsonQL.Demos.Examples.DataModels.IEmployee[], JsonQL.Demos",
        "$values": [
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
          null,
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

Result as List of Arrays of Value Type Items
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

In the example below the query result is expected to be of type **IReadOnlyList<double?[]>**.

.. raw:: html

   <details>
   <summary>Click to expand <b>Data.json</b> used in query in example below</summary>

.. code-block:: json

    {
      "ListOfListsOfDoubles": [
        [
          15,
          13,
          null,
          18
        ],
        [
          15,
          6,
          7
        ]
      ]
    }


.. raw:: html

   </details><br/><br/>
   
.. sourcecode:: csharp

     var query = "ListOfListsOfDoubles";

     // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
     // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
     // and it is normally configured as a singleton.
     JsonQL.Query.IQueryManager queryManager = null!;

     // The result "listOfListsOfNumbersResult" is a list of arrays of double values. 
     // Result of type "IReadOnlyList<double?[]>" cannot be null, and each array "double?[]" in list
     // "IReadOnlyList<double?[]>" cannot be null, however numeric values in "double?[]" can be null in converted object
     // according to value used for parameter "convertedValueNullability"
     var listOfListsOfNumbersResult =
         queryManager.QueryObject<IReadOnlyList<double?[]>>(query,
             new JsonTextData("Data",
                 this.LoadExampleJsonFile("Data.json")),
             convertedValueNullability: [
                 // The result of type "IReadOnlyList<double?[]>" cannot be null. An error will be reported if the result is null
                 false,
                 // "double?[]" items in "IReadOnlyList<double?[]>>" cannot be null
                 false,
                 // "double" values in "double?[]" array can be null.
                 true]);

.. raw:: html

   <details>
   <summary>Click to expand the result of the query in example above (i.e., instance of <b>JsonQL.Query.IObjectQueryResult&lt;IReadOnlyList&lt;double?[]&gt;&gt;</b>) serialized into <b>Result.json</b></summary>

.. code-block:: json

    {
      "$type": "JsonQL.Query.ObjectQueryResult`1[[System.Collections.Generic.IReadOnlyList`1[[System.Nullable`1[[System.Double, System.Private.CoreLib]][], System.Private.CoreLib]], System.Private.CoreLib]], JsonQL",
      "Value": {
        "$type": "System.Collections.Generic.List`1[[System.Nullable`1[[System.Double, System.Private.CoreLib]][], System.Private.CoreLib]], System.Private.CoreLib",
        "$values": [
          {
            "$type": "System.Nullable`1[[System.Double, System.Private.CoreLib]][], System.Private.CoreLib",
            "$values": [
              15.0,
              13.0,
              null,
              18.0
            ]
          },
          {
            "$type": "System.Nullable`1[[System.Double, System.Private.CoreLib]][], System.Private.CoreLib",
            "$values": [
              15.0,
              6.0,
              7.0
            ]
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
   
Result as List of Lists of Reference Type Items
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

In the example below the query result is expected to be of type **List<IReadOnlyList<IEmployee?>?>**.

.. raw:: html

   <details>
   <summary>Click to expand <b>CompaniesOrganizedAsArraysOfArrays.json</b> used in query in example below</summary>

.. code-block:: json

    {
      "Companies": [
        [
          {
            "Id": 100000026,
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
            "Id": 100000027,
            "FirstName": "Megan",
            "LastName": "Reed",
            "Address": {
              "Street": "12 Lakeview Dr",
              "City": "Chicago",
              "State": "IL",
              "ZipCode": "60602"
            },
            "Salary": 86000,
            "Age": 33,
            "Phones": [
              "312-555-0311"
            ]
          },
          {
            "Id": 100000028,
            "FirstName": "Carlos",
            "LastName": "Diaz",
            "Address": {
              "Street": "98 Wacker Pl",
              "City": "Chicago",
              "State": "IL",
              "ZipCode": "60603"
            },
            "Salary": 72000,
            "Age": 29,
            "Phones": [
              "312-555-0322",
              "312-555-0333"
            ]
          }

        ],
        [
          null
        ],
        [
          {
            "Id": 100000037,
            "FirstName": "Ava",
            "LastName": "Mitchell",
            "Address": {
              "Street": "142 Willow Lane",
              "City": "Portland",
              "State": "OR",
              "ZipCode": "97205"
            },
            "Salary": 81500,
            "Age": 33,
            "Phones": [
              "503-555-0616",
              "503-555-0627"
            ]
          },
          null,
          {
            "Id": 100000038,
            "FirstName": "Lucas",
            "LastName": "Freeman",
            "Address": {
              "Street": "88 Cedar Court",
              "City": "Portland",
              "State": "OR",
              "ZipCode": "97206"
            },
            "Salary": 74250,
            "Age": 29,
            "Phones": [
              "503-555-0738"
            ]
          }

        ],
        [

          {
            "Id": 100000029,
            "FirstName": "Liam",
            "LastName": "Grant",
            "Address": {
              "Street": "210 Oak Blvd",
              "City": "Portland",
              "State": "OR",
              "ZipCode": "97201"
            },
            "Salary": 78000,
            "Age": 30,
            "Phones": [
              "503-555-0414"
            ]
          },
          {
            "Id": 100000030,
            "FirstName": "Nora",
            "LastName": "Singh",
            "Address": {
              "Street": "7 Pine Street",
              "City": "Portland",
              "State": "OR",
              "ZipCode": "97202"
            },
            "Salary": 69500,
            "Age": 27,
            "Phones": [
              "503-555-0425",
              "503-555-0436"
            ]
          },
          {
            "Id": 100000031,
            "FirstName": "Owen",
            "LastName": "Park",
            "Address": {
              "Street": "55 River Rd",
              "City": "Portland",
              "State": "OR",
              "ZipCode": "97203"
            },
            "Salary": 84500,
            "Age": 38,
            "Phones": [
              "503-555-0447"
            ]
          },
          {
            "Id": 100000032,
            "FirstName": "Zara",
            "LastName": "Khan",
            "Address": {
              "Street": "300 Harbor Ln",
              "City": "Portland",
              "State": "OR",
              "ZipCode": "97204"
            },
            "Salary": 71000,
            "Age": 28,
            "Phones": [
              "503-555-0458",
              "503-555-0469"
            ]
          }

        ],
        [

          {
            "Id": 100000033,
            "FirstName": "Hannah",
            "LastName": "Brooks",
            "Address": {
              "Street": "12 Beacon St",
              "City": "Boston",
              "State": "MA",
              "ZipCode": "02108"
            },
            "Salary": 91000,
            "Age": 41,
            "Phones": [
              "617-555-0510",
              "617-555-0521"
            ]
          },
          {
            "Id": 100000034,
            "FirstName": "Ethan",
            "LastName": "Cole",
            "Address": {
              "Street": "77 Harborview Rd",
              "City": "Boston",
              "State": "MA",
              "ZipCode": "02110"
            },
            "Salary": 76000,
            "Age": 32,
            "Phones": [
              "617-555-0532"
            ]
          },
          {
            "Id": 100000035,
            "FirstName": "Maya",
            "LastName": "Patel",
            "Address": {
              "Street": "233 Commonwealth Ave",
              "City": "Boston",
              "State": "MA",
              "ZipCode": "02115"
            },
            "Salary": 68500,
            "Age": 29,
            "Phones": [
              "617-555-0543",
              "617-555-0554"
            ]
          },
          {
            "Id": 100000036,
            "FirstName": "Noah",
            "LastName": "Ortiz",
            "Address": {
              "Street": "14 Beacon St",
              "City": "Boston",
              "State": "MA",
              "ZipCode": "02116"
            },
            "Salary": 73000,
            "Age": 35,
            "Phones": [
              "617-555-0565"
            ]
          }

        ]
      ]
    }


.. raw:: html

   </details><br/><br/>
   
.. sourcecode:: csharp

     // Select all companies
     var query = "Companies";

     // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
     // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
     // and it is normally configured as a singleton.
     JsonQL.Query.IQueryManager queryManager = null!;

     // The result "companiesResult" is a list of list. Each company is represented as list of employees
     // The result of type "List<IReadOnlyList<IEmployee?>?>" cannot be null,
     // however list of employees can be null, and each employee in list of employees can be null too,
     // according to value used for parameter "convertedValueNullability"
     var companiesResult =
         queryManager.QueryObject<List<IReadOnlyList<IEmployee?>?>>(query,
             new JsonTextData("Companies",
                 this.LoadExampleJsonFile("CompaniesOrganizedAsArraysOfArrays.json")),
                 convertedValueNullability: [
                 // The result of type "List<IReadOnlyList<IEmployee?>?>" cannot be null.
                 // An error will be reported if the result is null
                 false,
                 // "IReadOnlyList<IEmployee?>" items in "List<IReadOnlyList<IEmployee?>?>" can be null
                 true,
                 // "IEmployee" items in "IReadOnlyList<IEmployee?>" can be null.
                 true]);

.. raw:: html

   <details>
   <summary>Click to expand the result of the query in example above (i.e., instance of <b>List&lt;IReadOnlyList&lt;IEmployee?&gt;?&gt;</b>) serialized into <b>Result.json</b></summary>

.. code-block:: json

    {
      "$type": "JsonQL.Query.ObjectQueryResult`1[[System.Collections.Generic.List`1[[System.Collections.Generic.IReadOnlyList`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib]], System.Private.CoreLib]], JsonQL",
      "Value": {
        "$type": "System.Collections.Generic.List`1[[System.Collections.Generic.IReadOnlyList`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib]], System.Private.CoreLib",
        "$values": [
          {
            "$type": "System.Collections.Generic.List`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib",
            "$values": [
              {
                "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
                "Id": 100000026,
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
                "Id": 100000027,
                "FirstName": "Megan",
                "LastName": "Reed",
                "Address": {
                  "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
                  "Street": "12 Lakeview Dr",
                  "City": "Chicago",
                  "State": "IL",
                  "ZipCode": "60602",
                  "County": null
                },
                "Salary": 86000,
                "Age": 33,
                "Manager": null,
                "Phones": {
                  "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
                  "$values": [
                    "312-555-0311"
                  ]
                }
              },
              {
                "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
                "Id": 100000028,
                "FirstName": "Carlos",
                "LastName": "Diaz",
                "Address": {
                  "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
                  "Street": "98 Wacker Pl",
                  "City": "Chicago",
                  "State": "IL",
                  "ZipCode": "60603",
                  "County": null
                },
                "Salary": 72000,
                "Age": 29,
                "Manager": null,
                "Phones": {
                  "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
                  "$values": [
                    "312-555-0322",
                    "312-555-0333"
                  ]
                }
              }
            ]
          },
          {
            "$type": "System.Collections.Generic.List`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib",
            "$values": [
              null
            ]
          },
          {
            "$type": "System.Collections.Generic.List`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib",
            "$values": [
              {
                "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
                "Id": 100000037,
                "FirstName": "Ava",
                "LastName": "Mitchell",
                "Address": {
                  "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
                  "Street": "142 Willow Lane",
                  "City": "Portland",
                  "State": "OR",
                  "ZipCode": "97205",
                  "County": null
                },
                "Salary": 81500,
                "Age": 33,
                "Manager": null,
                "Phones": {
                  "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
                  "$values": [
                    "503-555-0616",
                    "503-555-0627"
                  ]
                }
              },
              null,
              {
                "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
                "Id": 100000038,
                "FirstName": "Lucas",
                "LastName": "Freeman",
                "Address": {
                  "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
                  "Street": "88 Cedar Court",
                  "City": "Portland",
                  "State": "OR",
                  "ZipCode": "97206",
                  "County": null
                },
                "Salary": 74250,
                "Age": 29,
                "Manager": null,
                "Phones": {
                  "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
                  "$values": [
                    "503-555-0738"
                  ]
                }
              }
            ]
          },
          {
            "$type": "System.Collections.Generic.List`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib",
            "$values": [
              {
                "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
                "Id": 100000029,
                "FirstName": "Liam",
                "LastName": "Grant",
                "Address": {
                  "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
                  "Street": "210 Oak Blvd",
                  "City": "Portland",
                  "State": "OR",
                  "ZipCode": "97201",
                  "County": null
                },
                "Salary": 78000,
                "Age": 30,
                "Manager": null,
                "Phones": {
                  "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
                  "$values": [
                    "503-555-0414"
                  ]
                }
              },
              {
                "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
                "Id": 100000030,
                "FirstName": "Nora",
                "LastName": "Singh",
                "Address": {
                  "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
                  "Street": "7 Pine Street",
                  "City": "Portland",
                  "State": "OR",
                  "ZipCode": "97202",
                  "County": null
                },
                "Salary": 69500,
                "Age": 27,
                "Manager": null,
                "Phones": {
                  "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
                  "$values": [
                    "503-555-0425",
                    "503-555-0436"
                  ]
                }
              },
              {
                "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
                "Id": 100000031,
                "FirstName": "Owen",
                "LastName": "Park",
                "Address": {
                  "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
                  "Street": "55 River Rd",
                  "City": "Portland",
                  "State": "OR",
                  "ZipCode": "97203",
                  "County": null
                },
                "Salary": 84500,
                "Age": 38,
                "Manager": null,
                "Phones": {
                  "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
                  "$values": [
                    "503-555-0447"
                  ]
                }
              },
              {
                "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
                "Id": 100000032,
                "FirstName": "Zara",
                "LastName": "Khan",
                "Address": {
                  "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
                  "Street": "300 Harbor Ln",
                  "City": "Portland",
                  "State": "OR",
                  "ZipCode": "97204",
                  "County": null
                },
                "Salary": 71000,
                "Age": 28,
                "Manager": null,
                "Phones": {
                  "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
                  "$values": [
                    "503-555-0458",
                    "503-555-0469"
                  ]
                }
              }
            ]
          },
          {
            "$type": "System.Collections.Generic.List`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib",
            "$values": [
              {
                "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
                "Id": 100000033,
                "FirstName": "Hannah",
                "LastName": "Brooks",
                "Address": {
                  "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
                  "Street": "12 Beacon St",
                  "City": "Boston",
                  "State": "MA",
                  "ZipCode": "02108",
                  "County": null
                },
                "Salary": 91000,
                "Age": 41,
                "Manager": null,
                "Phones": {
                  "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
                  "$values": [
                    "617-555-0510",
                    "617-555-0521"
                  ]
                }
              },
              {
                "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
                "Id": 100000034,
                "FirstName": "Ethan",
                "LastName": "Cole",
                "Address": {
                  "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
                  "Street": "77 Harborview Rd",
                  "City": "Boston",
                  "State": "MA",
                  "ZipCode": "02110",
                  "County": null
                },
                "Salary": 76000,
                "Age": 32,
                "Manager": null,
                "Phones": {
                  "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
                  "$values": [
                    "617-555-0532"
                  ]
                }
              },
              {
                "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
                "Id": 100000035,
                "FirstName": "Maya",
                "LastName": "Patel",
                "Address": {
                  "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
                  "Street": "233 Commonwealth Ave",
                  "City": "Boston",
                  "State": "MA",
                  "ZipCode": "02115",
                  "County": null
                },
                "Salary": 68500,
                "Age": 29,
                "Manager": null,
                "Phones": {
                  "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
                  "$values": [
                    "617-555-0543",
                    "617-555-0554"
                  ]
                }
              },
              {
                "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
                "Id": 100000036,
                "FirstName": "Noah",
                "LastName": "Ortiz",
                "Address": {
                  "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
                  "Street": "14 Beacon St",
                  "City": "Boston",
                  "State": "MA",
                  "ZipCode": "02116",
                  "County": null
                },
                "Salary": 73000,
                "Age": 35,
                "Manager": null,
                "Phones": {
                  "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
                  "$values": [
                    "617-555-0565"
                  ]
                }
              }
            ]
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