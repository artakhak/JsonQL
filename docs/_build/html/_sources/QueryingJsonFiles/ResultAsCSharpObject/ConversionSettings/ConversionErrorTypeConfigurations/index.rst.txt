=============================================
Setting **ConversionErrorTypeConfigurations**
=============================================

.. contents::
   :local:
   :depth: 2

Setting **ConversionErrorTypeConfigurations** can be used to configure specific conversion error type as error, warning, or to configure the error to be ignored (not reported at all).

The configured error types are in enum `JsonQL.JsonToObjectConversion.ConversionErrorType <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/ConversionErrorType.cs>`_:

.. note::
    **ConversionErrorTypeConfigurations** affects the query execution only for conversion errors, after the JSON files (that might include JsonQL expressions) as well as the JsonQL query itself have been parsed. Therefore if errors happen during parsing of a file, the query might fail before conversion is done.

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


The examples below execute the same query with different configurations for error types **JsonQL.JsonToObjectConversion.ConversionErrorType.NonNullablePropertyNotSet** and **NonNullableCollectionItemValueNotSet** to demonstrate the usage of **ConversionErrorTypeConfigurations** setting.

Report Errors as Warnings Example
---------------------------------

The example below executes a query to get collection of **IEmployee** with reporting errors of types  **JsonQL.JsonToObjectConversion.ConversionErrorType.NonNullablePropertyNotSet** and **JsonQL.JsonToObjectConversion.ConversionErrorType.NonNullableCollectionItemValueNotSet** as warnings to be able to generate query result even if some non-nullable property values are missing, or some collection items are null in resulted JSON.

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
                        "ConversionSettings", "ConversionErrorTypeConfigurations"])),
            jsonConversionSettingOverrides: new JsonConversionSettingsOverrides
            {
                ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                {
                    new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullablePropertyNotSet, ErrorReportingType.ReportAsWarning),
                    new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullableCollectionItemValueNotSet, ErrorReportingType.ReportAsWarning)
                }
            });
            
     Assert.That(employeesResult.ErrorsAndWarnings.ConversionErrors.Errors.Count, Is.EqualTo(0));
     Assert.That(employeesResult.ErrorsAndWarnings.ConversionWarnings.Errors.Count, Is.EqualTo(4));
     Assert.That(employeesResult.Value, Is.Not.Null);
     Assert.That(employeesResult.Value!.Count, Is.EqualTo(2));


The result serialized below to **Result.json** shows that the errors were reported as warnings and not errors, and the query result is not null.

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
            "$values": []
          }
        },
        "ConversionWarnings": {
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
        }
      }
    }


.. raw:: html

   </details><br/><br/>
   
Ignore Errors Example
---------------------

The example below executes a query to get collection of **IEmployee** with ignoring errors of types  **JsonQL.JsonToObjectConversion.ConversionErrorType.NonNullablePropertyNotSet** and **JsonQL.JsonToObjectConversion.ConversionErrorType.NonNullableCollectionItemValueNotSet** to be able to generate query result even if some non-nullable property values are missing, or some collection items are null in resulted JSON.

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
                            "ConversionSettings", "ConversionErrorTypeConfigurations"])),
                jsonConversionSettingOverrides: new JsonConversionSettingsOverrides
                {
                    ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                    {
                        new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullablePropertyNotSet, ErrorReportingType.Ignore),
                        new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullableCollectionItemValueNotSet, ErrorReportingType.Ignore)
                    }
                });

     Assert.That(employeesResult.ErrorsAndWarnings.ConversionErrors.Errors.Count, Is.EqualTo(0));
     Assert.That(employeesResult.ErrorsAndWarnings.ConversionWarnings.Errors.Count, Is.EqualTo(0));
     Assert.That(employeesResult.Value, Is.Not.Null);
     Assert.That(employeesResult.Value!.Count, Is.EqualTo(2));


The result serialized below to **Result.json** shows that the errors were not reported at all, and the query result is not null.

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

Report Errors Example
---------------------

The example below executes a query to get collection of **IEmployee** without no overriding of setting **ConversionErrorTypeConfigurations**. This results in query execution failing with an error of type **JsonQL.JsonToObjectConversion.ConversionErrorType.NonNullablePropertyNotSet**. Query evaluation stops after the first error.

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
                         "ConversionSettings", "ConversionErrorTypeConfigurations"])),
             jsonConversionSettingOverrides: null);

     Assert.That(employeesResult.ErrorsAndWarnings.ConversionErrors.Errors.Count, Is.EqualTo(1));
     Assert.That(employeesResult.ErrorsAndWarnings.ConversionWarnings.Errors.Count, Is.EqualTo(0));
     Assert.That(employeesResult.Value, Is.Null);


The result serialized below to **Result.json** shows that an error of type  **JsonQL.JsonToObjectConversion.ConversionErrorType.NonNullablePropertyNotSet** is reported and the query result is null.

.. raw:: html

   <details>
   <summary>Click to expand the result of the query in example above (i.e., instance of <b>JsonQL.Query.IObjectQueryResult&lt;IReadOnlyList&lt;IEmployee&gt;&gt;</b>) serialized into <b>Result.json</b></summary>

.. code-block:: json

    {
      "$type": "JsonQL.Query.ObjectQueryResult`1[[System.Collections.Generic.IReadOnlyList`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib]], JsonQL",
      "Value": null,
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

