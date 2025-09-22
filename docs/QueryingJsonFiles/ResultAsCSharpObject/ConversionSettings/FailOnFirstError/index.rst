============================
Setting **FailOnFirstError**
============================

.. contents::
   :local:
   :depth: 2
   
The **FailOnFirstError** setting can be used to specify whether the query should fail with the first conversion error encountered, or if execution should continue even with conversion errors.

.. note::
    Errors reported as warning (see :doc:`../ConversionErrorTypeConfigurations/index`) do not stop query result generation regardless. Therefore, this setting affects the query generation only for errors reported as errors.
    
Lets assume an instance of `JsonQL.JsonToObjectConversion.IJsonConversionSettings <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IJsonConversionSettings.cs>`_ injected into `JsonQL.Query.QueryManager <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/QueryManager.cs>`_ was created using a code like the one shown below.

.. sourcecode:: csharp

     var conversionErrorTypeConfigurations = new List<ConversionErrorTypeConfiguration>();
             
     foreach (var conversionErrorType in Enum.GetValues<ConversionErrorType>())
     {
         // Set custom ErrorReportingType for ConversionErrorType here.
         // We report all errors as ErrorReportingType.ReportAsError by default.
         conversionErrorTypeConfigurations.Add(new ConversionErrorTypeConfiguration(conversionErrorType, ErrorReportingType.ReportAsError));
     }

     var jsonConversionSettings = new JsonConversionSettings
     {
         JsonPropertyFormat = JsonPropertyFormat.PascalCase,
         FailOnFirstError = true,

         // conversionErrorTypeConfigurations was setup above to report all error types as errors.
         ConversionErrorTypeConfigurations = conversionErrorTypeConfigurations,

         TryMapJsonConversionType = null
     };

Also, lets assume the queries in examples below query the JSON file **EmployeesWithMissingData.json** shown below with some null or missing values for employee addresses or phone numbers list:

.. sourcecode:: json

     {
       "Employees": [
         {
           "Id": 100000001,
           "FirstName": "John",
           "LastName": "Smith",
           "Address": null,
           "Salary": 99500,
           "Age": 45,
           "Phones": null
         },
         {
           "Id": 100000003,
           "FirstName": "Michael",
           "LastName": "Brown",     
           "Salary": 89000,
           "Age": 50
         },    
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
         }
       ]
     }
     
The example below executes a query and converts the result to **IReadOnlyList<IEmployee>**. The query execution result includes errors since some non-nullable properties values are not set in **IEmployee** items in query result collection. However, regardless of this fact, the value of **employeesResult.Value** is not null and is not empty, since JsonQL continues conversion, even if errors are encountered during the process.

.. sourcecode:: csharp

     // Select the employees with null or missing values for non-null properties
     var query =
         "Employees.Where(e => e.Address is null || e.Address is undefined || e.Phones is null || e.Phones is undefined)";

     // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
     // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
     // and it is normally configured as a singleton.
     JsonQL.Query.IQueryManager queryManager = null!;

     var employeesResult =
          queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
              new JsonTextData("EmployeesWithMissingData",
                  LoadJsonFileHelpers.LoadJsonFile("EmployeesWithMissingData.json", 
                      ["DocFiles", "QueryingJsonFiles", "ResultAsCSharpObject", 
                          "ConversionSettings", "FailOnFirstError", "Example"])),
              jsonConversionSettingOverrides: new JsonConversionSettingsOverrides
              {
                  FailOnFirstError = false
              });

     Assert.That(employeesResult.ErrorsAndWarnings.ConversionErrors.Errors.Count, Is.EqualTo(4));
     Assert.That(employeesResult.ErrorsAndWarnings.ConversionWarnings.Errors.Count, Is.EqualTo(0));
     Assert.That(employeesResult.Value, Is.Not.Null);
     Assert.That(employeesResult.Value!.Count, Is.EqualTo(2));
     
The result (an instance of `JsonQL.Query.IObjectQueryResult[IReadOnlyList[IEmployee]] <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IObjectQueryResult.cs>`_) is serialized to a **Result.json** file below.

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
            "Address": null,
            "Salary": 99500,
            "Age": 45,
            "Manager": null,
            "Phones": null
          },
          {
            "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
            "Id": 100000003,
            "FirstName": "Michael",
            "LastName": "Brown",
            "Address": null,
            "Salary": 89000,
            "Age": 50,
            "Manager": null,
            "Phones": null
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
            "$values": [
              {
                "$type": "JsonQL.JsonToObjectConversion.ConversionError, JsonQL",
                "ErrorType": "NonNullablePropertyNotSet",
                "JsonPath": {
                  "$type": "JsonQL.JsonObjects.JsonPath.JsonPath, JsonQL",
                  "JsonTextIdentifier": "Query_849E0817-3256-483D-8E97-01744EBC3F76",
                  "Path": {
                    "$type": "System.Collections.Generic.List`1[[JsonQL.JsonObjects.JsonPath.IJsonPathElement, JsonQL]], System.Private.CoreLib",
                    "$values": [
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonPropertyNamePathElement, JsonQL",
                        "Name": "Root"
                      },
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonPropertyNamePathElement, JsonQL",
                        "Name": "query"
                      },
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonArrayIndexesPathElement, JsonQL",
                        "Indexes": {
                          "$type": "System.Collections.Generic.List`1[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib",
                          "$values": [
                            0
                          ]
                        }
                      }
                    ]
                  }
                },
                "PathInReferencedJson": {
                  "$type": "JsonQL.JsonObjects.JsonPath.JsonPath, JsonQL",
                  "JsonTextIdentifier": "EmployeesWithMissingData",
                  "Path": {
                    "$type": "System.Collections.Generic.List`1[[JsonQL.JsonObjects.JsonPath.IJsonPathElement, JsonQL]], System.Private.CoreLib",
                    "$values": [
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonPropertyNamePathElement, JsonQL",
                        "Name": "Root"
                      },
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonPropertyNamePathElement, JsonQL",
                        "Name": "Employees"
                      },
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonArrayIndexesPathElement, JsonQL",
                        "Indexes": {
                          "$type": "System.Collections.Generic.List`1[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib",
                          "$values": [
                            0
                          ]
                        }
                      }
                    ]
                  }
                },
                "Error": "Failed to retrieve and set the value of non-nullable property [Address] in type [JsonQL.Demos.Examples.DataModels.Employee].",
                "ConvertedObjectPath": {
                  "$type": "JsonQL.JsonToObjectConversion.ConvertedObjectPath.ConvertedObjectPath, JsonQL",
                  "RootConvertedObjectPathElement": {
                    "$type": "JsonQL.JsonToObjectConversion.ConvertedObjectPath.RootConvertedObjectPathElement, JsonQL",
                    "Name": "Root",
                    "ObjectType": "System.Collections.Generic.IReadOnlyList`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"
                  },
                  "Path": {
                    "$type": "System.Collections.Generic.List`1[[JsonQL.JsonToObjectConversion.ConvertedObjectPath.IConvertedObjectPathValueSelectorElement, JsonQL]], System.Private.CoreLib",
                    "$values": [
                      {
                        "$type": "JsonQL.JsonToObjectConversion.ConvertedObjectPath.IndexConvertedObjectPathElement, JsonQL",
                        "Name": "0",
                        "ObjectType": "JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                      },
                      {
                        "$type": "JsonQL.JsonToObjectConversion.ConvertedObjectPath.PropertyNameConvertedObjectPathElement, JsonQL",
                        "Name": "Address",
                        "ObjectType": "JsonQL.Demos.Examples.DataModels.IAddress, JsonQL.Demos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                      }
                    ]
                  }
                }
              },
              {
                "$type": "JsonQL.JsonToObjectConversion.ConversionError, JsonQL",
                "ErrorType": "NonNullablePropertyNotSet",
                "JsonPath": {
                  "$type": "JsonQL.JsonObjects.JsonPath.JsonPath, JsonQL",
                  "JsonTextIdentifier": "Query_849E0817-3256-483D-8E97-01744EBC3F76",
                  "Path": {
                    "$type": "System.Collections.Generic.List`1[[JsonQL.JsonObjects.JsonPath.IJsonPathElement, JsonQL]], System.Private.CoreLib",
                    "$values": [
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonPropertyNamePathElement, JsonQL",
                        "Name": "Root"
                      },
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonPropertyNamePathElement, JsonQL",
                        "Name": "query"
                      },
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonArrayIndexesPathElement, JsonQL",
                        "Indexes": {
                          "$type": "System.Collections.Generic.List`1[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib",
                          "$values": [
                            0
                          ]
                        }
                      }
                    ]
                  }
                },
                "PathInReferencedJson": {
                  "$type": "JsonQL.JsonObjects.JsonPath.JsonPath, JsonQL",
                  "JsonTextIdentifier": "EmployeesWithMissingData",
                  "Path": {
                    "$type": "System.Collections.Generic.List`1[[JsonQL.JsonObjects.JsonPath.IJsonPathElement, JsonQL]], System.Private.CoreLib",
                    "$values": [
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonPropertyNamePathElement, JsonQL",
                        "Name": "Root"
                      },
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonPropertyNamePathElement, JsonQL",
                        "Name": "Employees"
                      },
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonArrayIndexesPathElement, JsonQL",
                        "Indexes": {
                          "$type": "System.Collections.Generic.List`1[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib",
                          "$values": [
                            0
                          ]
                        }
                      }
                    ]
                  }
                },
                "Error": "Failed to retrieve and set the value of non-nullable property [Phones] in type [JsonQL.Demos.Examples.DataModels.Employee].",
                "ConvertedObjectPath": {
                  "$type": "JsonQL.JsonToObjectConversion.ConvertedObjectPath.ConvertedObjectPath, JsonQL",
                  "RootConvertedObjectPathElement": {
                    "$type": "JsonQL.JsonToObjectConversion.ConvertedObjectPath.RootConvertedObjectPathElement, JsonQL",
                    "Name": "Root",
                    "ObjectType": "System.Collections.Generic.IReadOnlyList`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"
                  },
                  "Path": {
                    "$type": "System.Collections.Generic.List`1[[JsonQL.JsonToObjectConversion.ConvertedObjectPath.IConvertedObjectPathValueSelectorElement, JsonQL]], System.Private.CoreLib",
                    "$values": [
                      {
                        "$type": "JsonQL.JsonToObjectConversion.ConvertedObjectPath.IndexConvertedObjectPathElement, JsonQL",
                        "Name": "0",
                        "ObjectType": "JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                      },
                      {
                        "$type": "JsonQL.JsonToObjectConversion.ConvertedObjectPath.PropertyNameConvertedObjectPathElement, JsonQL",
                        "Name": "Phones",
                        "ObjectType": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"
                      }
                    ]
                  }
                }
              },
              {
                "$type": "JsonQL.JsonToObjectConversion.ConversionError, JsonQL",
                "ErrorType": "NonNullablePropertyNotSet",
                "JsonPath": {
                  "$type": "JsonQL.JsonObjects.JsonPath.JsonPath, JsonQL",
                  "JsonTextIdentifier": "Query_849E0817-3256-483D-8E97-01744EBC3F76",
                  "Path": {
                    "$type": "System.Collections.Generic.List`1[[JsonQL.JsonObjects.JsonPath.IJsonPathElement, JsonQL]], System.Private.CoreLib",
                    "$values": [
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonPropertyNamePathElement, JsonQL",
                        "Name": "Root"
                      },
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonPropertyNamePathElement, JsonQL",
                        "Name": "query"
                      },
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonArrayIndexesPathElement, JsonQL",
                        "Indexes": {
                          "$type": "System.Collections.Generic.List`1[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib",
                          "$values": [
                            1
                          ]
                        }
                      }
                    ]
                  }
                },
                "PathInReferencedJson": {
                  "$type": "JsonQL.JsonObjects.JsonPath.JsonPath, JsonQL",
                  "JsonTextIdentifier": "EmployeesWithMissingData",
                  "Path": {
                    "$type": "System.Collections.Generic.List`1[[JsonQL.JsonObjects.JsonPath.IJsonPathElement, JsonQL]], System.Private.CoreLib",
                    "$values": [
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonPropertyNamePathElement, JsonQL",
                        "Name": "Root"
                      },
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonPropertyNamePathElement, JsonQL",
                        "Name": "Employees"
                      },
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonArrayIndexesPathElement, JsonQL",
                        "Indexes": {
                          "$type": "System.Collections.Generic.List`1[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib",
                          "$values": [
                            1
                          ]
                        }
                      }
                    ]
                  }
                },
                "Error": "Failed to retrieve and set the value of non-nullable property [Address] in type [JsonQL.Demos.Examples.DataModels.Employee].",
                "ConvertedObjectPath": {
                  "$type": "JsonQL.JsonToObjectConversion.ConvertedObjectPath.ConvertedObjectPath, JsonQL",
                  "RootConvertedObjectPathElement": {
                    "$type": "JsonQL.JsonToObjectConversion.ConvertedObjectPath.RootConvertedObjectPathElement, JsonQL",
                    "Name": "Root",
                    "ObjectType": "System.Collections.Generic.IReadOnlyList`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"
                  },
                  "Path": {
                    "$type": "System.Collections.Generic.List`1[[JsonQL.JsonToObjectConversion.ConvertedObjectPath.IConvertedObjectPathValueSelectorElement, JsonQL]], System.Private.CoreLib",
                    "$values": [
                      {
                        "$type": "JsonQL.JsonToObjectConversion.ConvertedObjectPath.IndexConvertedObjectPathElement, JsonQL",
                        "Name": "1",
                        "ObjectType": "JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                      },
                      {
                        "$type": "JsonQL.JsonToObjectConversion.ConvertedObjectPath.PropertyNameConvertedObjectPathElement, JsonQL",
                        "Name": "Address",
                        "ObjectType": "JsonQL.Demos.Examples.DataModels.IAddress, JsonQL.Demos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                      }
                    ]
                  }
                }
              },
              {
                "$type": "JsonQL.JsonToObjectConversion.ConversionError, JsonQL",
                "ErrorType": "NonNullablePropertyNotSet",
                "JsonPath": {
                  "$type": "JsonQL.JsonObjects.JsonPath.JsonPath, JsonQL",
                  "JsonTextIdentifier": "Query_849E0817-3256-483D-8E97-01744EBC3F76",
                  "Path": {
                    "$type": "System.Collections.Generic.List`1[[JsonQL.JsonObjects.JsonPath.IJsonPathElement, JsonQL]], System.Private.CoreLib",
                    "$values": [
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonPropertyNamePathElement, JsonQL",
                        "Name": "Root"
                      },
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonPropertyNamePathElement, JsonQL",
                        "Name": "query"
                      },
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonArrayIndexesPathElement, JsonQL",
                        "Indexes": {
                          "$type": "System.Collections.Generic.List`1[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib",
                          "$values": [
                            1
                          ]
                        }
                      }
                    ]
                  }
                },
                "PathInReferencedJson": {
                  "$type": "JsonQL.JsonObjects.JsonPath.JsonPath, JsonQL",
                  "JsonTextIdentifier": "EmployeesWithMissingData",
                  "Path": {
                    "$type": "System.Collections.Generic.List`1[[JsonQL.JsonObjects.JsonPath.IJsonPathElement, JsonQL]], System.Private.CoreLib",
                    "$values": [
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonPropertyNamePathElement, JsonQL",
                        "Name": "Root"
                      },
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonPropertyNamePathElement, JsonQL",
                        "Name": "Employees"
                      },
                      {
                        "$type": "JsonQL.JsonObjects.JsonPath.JsonArrayIndexesPathElement, JsonQL",
                        "Indexes": {
                          "$type": "System.Collections.Generic.List`1[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib",
                          "$values": [
                            1
                          ]
                        }
                      }
                    ]
                  }
                },
                "Error": "Failed to retrieve and set the value of non-nullable property [Phones] in type [JsonQL.Demos.Examples.DataModels.Employee].",
                "ConvertedObjectPath": {
                  "$type": "JsonQL.JsonToObjectConversion.ConvertedObjectPath.ConvertedObjectPath, JsonQL",
                  "RootConvertedObjectPathElement": {
                    "$type": "JsonQL.JsonToObjectConversion.ConvertedObjectPath.RootConvertedObjectPathElement, JsonQL",
                    "Name": "Root",
                    "ObjectType": "System.Collections.Generic.IReadOnlyList`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"
                  },
                  "Path": {
                    "$type": "System.Collections.Generic.List`1[[JsonQL.JsonToObjectConversion.ConvertedObjectPath.IConvertedObjectPathValueSelectorElement, JsonQL]], System.Private.CoreLib",
                    "$values": [
                      {
                        "$type": "JsonQL.JsonToObjectConversion.ConvertedObjectPath.IndexConvertedObjectPathElement, JsonQL",
                        "Name": "1",
                        "ObjectType": "JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                      },
                      {
                        "$type": "JsonQL.JsonToObjectConversion.ConvertedObjectPath.PropertyNameConvertedObjectPathElement, JsonQL",
                        "Name": "Phones",
                        "ObjectType": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"
                      }
                    ]
                  }
                }
              }
            ]
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

