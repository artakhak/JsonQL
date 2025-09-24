============
Type Binding
============

.. contents::
   :local:
   :depth: 2

JsonQL tries converts the result of a query executed by calling method one of overloaded methods **IJsonQL.Query.QueryManager.QueryObject<TQueryObject>** to an instance of **TQueryObject** and store it in property **Value** of type **TQueryObject** in result of type `JsonQL.Query.IObjectQueryResult[TQueryObject] <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IObjectQueryResult.cs>`_

The following rules are applied when converting JSON compiled as a result of executing the query to an instance of **TQueryObject**.

- Any interface is deserialized using the default implementation model classes found in the same namespace (this includes both **TQueryObject** if it is an interface, as well as any properties used in **TQueryObject**).
- Model classes should have either properties with public setters, or property value should be injected in constructors for the values in JSON to be stored in de-serialized objects (if public setter is missing, the value will not be deserialized).

    Below is an example of model class **Employee** with a property **Employee** that has public getter/setter, and a property **Id** with only a public getter, and a constructor with injected parameter **id** which sets the value of **Id** property.

    .. sourcecode:: csharp

        namespace JsonQL.Demos.Examples.DataModels;

        public class Employee : IEmployee
        {
            // Example when property can be set in constructor.
            public Employee(long id)
            {
                Id = id;
            }

            public long Id { get; }
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;
            public IAddress Address { get; set; } = null!;
            public int Salary { get; set; }
            public int Age { get; set; }
            public IManager? Manager { get; set; }

            /// <summary>
            /// Example of <see cref="List{T}"/> property. Other examples use <see cref="IReadOnlyList{T}"/>
            /// </summary>
            public List<string> Phones { get; set; } = null!;
        }

     
- Class or interface used to deserialize a JSON value can be replaced with custom provided class or interface by either setting the value of **TryMapTypeDelegate** property in `JsonQL.JsonToObjectConversion.IJsonConversionSettings <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IJsonConversionSettings.cs>`_ (normally done once in dependency injection setup on application start) or in `JsonQL.JsonToObjectConversion.IJsonConversionSettingsOverrides <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IJsonConversionSettingsOverrides.cs>`_ (can be done with each query execution, and overrides the value set in `JsonQL.JsonToObjectConversion.IJsonConversionSettings <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IJsonConversionSettings.cs>`_).
    
    .. note::
        See :doc:`../ConversionSettings/index` for examples of configuring settings.
        See :ref:`replace-serialization-type` for example of replacing the de-serialization type per query execution. 

- Types used for serialization (both value types like 'System.Double' as well as reference types) can use use nullable syntax. See :doc:`../NullableValueSupport/index` for more details on nullable syntax in types used for de-serialization.
- If result fails to de-serialize into C# object specified, there will be conversion errors of type `JsonQL.JsonToObjectConversion.IConversionError <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IConversionError.cs>`_ in result with value of **ErrorType** equal to one of the following enum values: **CannotCreateInstanceOfClass**, **FailedToConvertJsonValueToExpectedType**, or in some cases **Error** (normally more specific error type will be reported).

Examples
--------

Convert to Classes
~~~~~~~~~~~~~~~~~~

This example queries the JSON file :doc:`../SampleFiles/companies` and converts the result to **IReadOnlyList<Employee>**.

.. note::
    The following Microsoft collection types can be used in query results, as well as for any property in model classes: **System.Collections.Generic.List<T>**, **System.Collections.Generic.IReadOnlyList<T>**, **System.Collections.Generic.IEnumerable<T>**, as well as arrays (e.g., **IEmployee[]**).

.. sourcecode:: csharp

     var query = "Companies.Select(c => c.Employees.Where(e => e.Salary >= 100000))";

     // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
     // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
     // and it is normally configured as a singleton.
     JsonQL.Query.IQueryManager queryManager = null!;

     // The result "employeesResult" is of type "JsonQL.Query.IObjectQueryResult<IReadOnlyList<Employee>>".
     var employeesResult =
         _queryManager.QueryObject<IReadOnlyList<Employee>>(query,
             new JsonTextData("Companies",
                 LoadJsonFileHelpers.LoadJsonFile("Companies.json", 
                     ["DocFiles", "QueryingJsonFiles", "JsonFiles"])));
     
The **Result.json** below stores serialized instance of `JsonQL.Query.IObjectQueryResult<IReadOnlyList<Employee> <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IObjectQueryResult.cs>`_ for the query in example above.

.. raw:: html

   <details>
   <summary>Click to expand <b>Result.json</b> </summary>

.. code-block:: json

    {
      "$type": "JsonQL.Query.ObjectQueryResult`1[[System.Collections.Generic.IReadOnlyList`1[[JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.ConvertToClasses.DataModels.Employee, JsonQL.Demos]], System.Private.CoreLib]], JsonQL",
      "Value": {
        "$type": "System.Collections.Generic.List`1[[JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.ConvertToClasses.DataModels.Employee, JsonQL.Demos]], System.Private.CoreLib",
        "$values": [
          {
            "$type": "JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.ConvertToClasses.DataModels.Employee, JsonQL.Demos",
            "Id": 100000002,
            "FirstName": "Alice",
            "LastName": "Johnson",
            "Address": {
              "$type": "JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.ConvertToClasses.DataModels.Address, JsonQL.Demos",
              "Street": "123 Maple Street",
              "City": "New York",
              "State": "NY",
              "ZipCode": "10001",
              "County": null
            },
            "Salary": 105000,
            "Age": 38,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": [
                "212-555-0199"
              ]
            }
          },
          {
            "$type": "JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.ConvertToClasses.DataModels.Employee, JsonQL.Demos",
            "Id": 100000005,
            "FirstName": "Christopher",
            "LastName": "Garcia",
            "Address": {
              "$type": "JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.ConvertToClasses.DataModels.Address, JsonQL.Demos",
              "Street": "654 Cedar Road",
              "City": "Phoenix",
              "State": "AZ",
              "ZipCode": "85001",
              "County": null
            },
            "Salary": 111000,
            "Age": 29,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": [
                "602-555-0166",
                "602-555-0188"
              ]
            }
          },
          {
            "$type": "JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.ConvertToClasses.DataModels.Employee, JsonQL.Demos",
            "Id": 100000008,
            "FirstName": "Laura",
            "LastName": "Lee",
            "Address": {
              "$type": "JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.ConvertToClasses.DataModels.Address, JsonQL.Demos",
              "Street": "258 Willow Lane",
              "City": "San Diego",
              "State": "CA",
              "ZipCode": "92101",
              "County": null
            },
            "Salary": 105500,
            "Age": 32,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": [
                "619-555-0155",
                "619-555-0122"
              ]
            }
          },
          {
            "$type": "JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.ConvertToClasses.DataModels.Employee, JsonQL.Demos",
            "Id": 250150245,
            "FirstName": "Jane",
            "LastName": "Doe",
            "Address": {
              "$type": "JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.ConvertToClasses.DataModels.Address, JsonQL.Demos",
              "Street": "Main St",
              "City": "San Jose",
              "State": "PA",
              "ZipCode": "95101",
              "County": null
            },
            "Salary": 144186,
            "Age": 63,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": [
                "408-555-0133",
                "408-555-0190"
              ]
            }
          },
          {
            "$type": "JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.ConvertToClasses.DataModels.Employee, JsonQL.Demos",
            "Id": 783328759,
            "FirstName": "Robert",
            "LastName": "Brown",
            "Address": {
              "$type": "JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.ConvertToClasses.DataModels.Address, JsonQL.Demos",
              "Street": "Pine St",
              "City": "Los Angeles",
              "State": "CA",
              "ZipCode": "90001",
              "County": null
            },
            "Salary": 122395,
            "Age": 58,
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
   

Convert to Classes Interfaces
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

This example queries the JSON file :doc:`../SampleFiles/companies` and converts the result to **IReadOnlyList<IEmployee>**.
-JsonQL will use default implementation **Employee** of **Employee** when creating instances in **IReadOnlyList<IEmployee>**.

.. sourcecode:: csharp

     var query = "Companies.Select(c => c.Employees.Where(e => e.Salary >= 100000))";

     // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
     // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
     // and it is normally configured as a singleton.
     JsonQL.Query.IQueryManager queryManager = null!;

     // The result "employeesResult" is of type "JsonQL.Query.IObjectQueryResult<IReadOnlyList<Employee>>".
     var employeesResult =
         _queryManager.QueryObject<IReadOnlyList<Employee>>(query,
             new JsonTextData("Companies",
                 LoadJsonFileHelpers.LoadJsonFile("Companies.json", 
                     ["DocFiles", "QueryingJsonFiles", "JsonFiles"])));
     
The **Result.json** below stores serialized instance of `JsonQL.Query.IObjectQueryResult<IReadOnlyList<IEmployee> <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IObjectQueryResult.cs>`_ for the query in example above.

.. raw:: html

   <details>
   <summary>Click to expand <b>Result.json</b> </summary>

.. code-block:: json

    {
      "$type": "JsonQL.Query.ObjectQueryResult`1[[System.Collections.Generic.IReadOnlyList`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib]], JsonQL",
      "Value": {
        "$type": "System.Collections.Generic.List`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib",
        "$values": [
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


.. _replace-serialization-type:

Replace Interface Implementation
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

This example queries the JSON file :doc:`Examples/ReplaceInterfaceImplementation/employees` and converts the result to **IReadOnlyList<IEmployee>**.

- We use **jsonConversionSettingOverrides** parameter in call to method **queryManager.QueryObject<IReadOnlyList<IEmployee>>()** to use **IManager** instead of **IEmployee** if converted JSON object in query result has **Employees** key with a value as a non-empty array.

    .. note::
        We could do the same thing by setting the value of **TryMapJsonConversionType** of property `JsonQL.JsonToObjectConversion.IJsonConversionSettings <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IJsonConversionSettings.cs>`_ to the same lambda expression we used for parameter **jsonConversionSettingOverrides** when creating an instance of `JsonQL.JsonToObjectConversion.IJsonConversionSettings <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IJsonConversionSettings.cs>`_. See :doc:`../ConversionSettings/index` for more details on configuring settings. 
    
- JsonQL will use default implementation **Employee** of **IEmployee** when creating instances in **IReadOnlyList<IEmployee>** in all other cases.

.. sourcecode:: csharp

     var query = "Employees.Where(e => e.Salary >= 100000)";

     // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
     // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
     // and it is normally configured as a singleton.
     JsonQL.Query.IQueryManager queryManager = null!;

     // The result "employeesResult" is of type "JsonQL.Query.IObjectQueryResult<IReadOnlyList<IEmployee>>".
     var employeesResult =
         queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
             new JsonTextData("Employees",
                 this.LoadExampleJsonFile("Employees.json")),
             convertedValueNullability: [false, false],
             jsonConversionSettingOverrides:
             new JsonConversionSettingsOverrides
             {
                 TryMapJsonConversionType = (type, parsedJson) =>
                 {
                     // If we always return null, or just do not set the value, of TryMapJsonConversionType
                     // IEmployee will always be bound to Employee
                     // In this example, we ensure that if parsed JSON has "Employees" as a non-empty array,
                     // then the default implementation of IManager (i.e., Manager) is used to
                     // deserialize the JSON.
                     // We can also specify Manager explicitly.
                     if (//parsedJson.HasKey(nameof(IManager.Employees))
                         parsedJson.TryGetJsonKeyValue(nameof(IManager.Employees), out var employees) &&
                         employees.Value is IParsedArrayValue employeesArray && employeesArray.Values.Count > 0)
                         return typeof(IManager);

                     return null;
                 }
             });

     Assert.That(employeesResult.Value, Is.Not.Null);
     Assert.That(employeesResult.Value!.Count, Is.EqualTo(2));
     Assert.That(employeesResult.Value[0], Is.Not.InstanceOf<IManager>());
     Assert.That(employeesResult.Value[1], Is.InstanceOf<IManager>());
     Assert.That(employeesResult.Value[1], Is.TypeOf<Manager>());
     
The **Result.json** below stores serialized instance of `JsonQL.Query.IObjectQueryResult<IReadOnlyList<IEmployee> <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IObjectQueryResult.cs>`_ for the query in example above.

.. raw:: html

   <details>
   <summary>Click to expand <b>Result.json</b> </summary>

.. code-block:: json

    {
      "$type": "JsonQL.Query.ObjectQueryResult`1[[System.Collections.Generic.IReadOnlyList`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib]], JsonQL",
      "Value": {
        "$type": "System.Collections.Generic.List`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib",
        "$values": [
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
                "525-000-0001"
              ]
            }
          },
          {
            "$type": "JsonQL.Demos.Examples.DataModels.Manager, JsonQL.Demos",
            "Employees": {
              "$type": "System.Collections.Generic.List`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib",
              "$values": [
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
                      "525-000-0001"
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
                      "515-100-0001"
                    ]
                  }
                }
              ]
            },
            "Id": 100000006,
            "FirstName": "Sarah",
            "LastName": "Wilson",
            "Address": {
              "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
              "Street": "987 Ash Boulevard",
              "City": "Philadelphia",
              "State": "PA",
              "ZipCode": "19101",
              "County": null
            },
            "Salary": 160000,
            "Age": 35,
            "Manager": null,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": [
                "632-111-1112",
                "632-111-1113"
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