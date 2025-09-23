====================================
Setting **TryMapJsonConversionType**
====================================

.. contents::
   :local:
   :depth: 2
   
Setting **TryMapJsonConversionType** allows customizing the types used for deserialization of JSON query result when setting the value of property **Value** in `JsonQL.Query.IObjectQueryResult<TQueryObject> <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IObjectQueryResult.cs>`_. 
By default JsonQL uses the model class types to de-serialize query result JSON to specified C# model class (i.e., uses the model class type, its property types, continuing this way recursively until the object is constructed and values are set). 

- JsonQL tries to resolve the type to use for conversion by trying number of resolutions below. The first resolution that resolves a type wins:
   - If custom settings are setup, resolve type via a call to delegate in property **TryMapJsonConversionType** in `JsonQL.JsonToObjectConversion.IJsonConversionSettingsOverrides <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IJsonConversionSettingsOverrides.cs>`_.
       
      .. note::
           To resolve a type using `JsonQL.JsonToObjectConversion.IJsonConversionSettingsOverrides <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IJsonConversionSettingsOverrides.cs>`_, use optional parameter **jsonConversionSettingOverrides** of type `JsonQL.JsonToObjectConversion.IJsonConversionSettingsOverrides <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IJsonConversionSettingsOverrides.cs>`_ when calling one of the overloaded methods **JsonQL.Query.IQueryManager.QueryObject<TQueryObject>(...)**
   
   - If settings are setup, resolve the type via a call to delegate in property **TryMapJsonConversionType** in `JsonQL.JsonToObjectConversion.IJsonConversionSettings <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IJsonConversionSettings.cs>`_.
   - If the type currently being de-serialized to is a concrete model class, then JsonQL uses the class (simple C# class. See :doc:`../../TypeBinding/index` for more details).
   - If a type a JSON object should de de-serialized is an interface, then JsonQL tries to find concrete implementation of the interface in the same namespace.
   - Otherwise, if object of a type cannot be created using any rules above, an error is reported.

Example
-------

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
         JsonPropertyFormat = JsonQL.JsonToObjectConversion.JsonPropertyFormat.PascalCase,
         FailOnFirstError = true,
         
         // conversionErrorTypeConfigurations was setup above to report all error types as errors.
         ConversionErrorTypeConfigurations = conversionErrorTypeConfigurations,
         
         // Set custom interface to implementation mappings here.
         // Default mapping mechanism (i.e., IModelClassMapper) will 
         // try to find an implementation that has the same name space and class
         // name that matches interface name without "I" prefix (if the mapped
         // type defaultTypeToConvertParsedJsonTo is an interface).
         // For example for interface JsonQL.Demos.Examples.DataModels.IEmployee class
         // JsonQL.Demos.Examples.DataModels.Employee will be used if it exists and it
         // implements JsonQL.Demos.Examples.DataModels.IEmployee.
         // If defaultTypeToConvertParsedJsonTo is a class, the default mapping mechanism will
         // use the class itself.
         TryMapJsonConversionType =
             (defaultTypeToConvertParsedJsonTo, convertedParsedJson) =>
             {
                 if (defaultTypeToConvertParsedJsonTo.FullName == "JsonQL.Demos.Examples.DataModels.IEmployee")
                 {
                     if (convertedParsedJson.HasKey("Employees"))
                         return Type.GetType("JsonQL.Demos.Examples.DataModels.IManager, JsonQL.Demos");
                 
                     if (convertedParsedJson.TryGetJsonKeyValue("$type", out var employeeType) &&
                         employeeType.Value is IParsedSimpleValue parsedSimpleValue &&
                         parsedSimpleValue.IsString && parsedSimpleValue.Value != null)
                     {
                         var convertedType = Type.GetType(parsedSimpleValue.Value);
                 
                         if (convertedType != null)
                             return convertedType;
                     }
                 }
                 
                 // Returning null will result default mapping mechanism picking a type to use.
                 return null;
             }
     };

Also, lets assume the queries in examples below query the JSON file **Employees.json** shown below:

.. raw:: html

   <details>
   <summary>Click to expand the <b>Employees.json</b> file being queried</summary>

.. code-block:: json

    {
      "Employees": [
        {
          "Id": 100000001,
          "FirstName": "John",
          "LastName": "Smith",
          "Employees": [],
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
          "FirstName": "Aisha",
          "LastName": "Khan",

          "Employees": [
            {
              "Id": 100000004,
              "FirstName": "Sara",
              "LastName": "Lee",
              "Address": {
                "Street": "33 Willow Road",
                "City": "San Francisco",
                "State": "CA",
                "ZipCode": "94102"
              },
              "Salary": 125000,
              "Age": 38,
              "Phones": [
                "415-555-0111"
              ]
            }
          ],
          "Address": {
            "Street": "129 Pine Street",
            "City": "Boston",
            "State": "MA",
            "ZipCode": "02108"
          },
          "Salary": 88000,
          "Age": 32,
          "Phones": [
            "617-555-0199"
          ]
        },
        {
          "$type": "JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.ConversionSettings.TryMapJsonConversionType.Example.CustomEmployee, JsonQL.Demos",
          "Id": 100000003,
          "FirstName": "Miguel",
          "LastName": "Santos",
          "Address": {
            "Street": "902 Palm Drive",
            "City": "Miami",
            "State": "FL",
            "ZipCode": "33101"
          },
          "Salary": 72000,
          "Age": 28,
          "Phones": [
            "305-555-0147",
            "305-555-0160"
          ]
        },
        {
          "Id": 100000004,
          "FirstName": "Sara",
          "LastName": "Lee",
          "Address": {
            "Street": "33 Willow Road",
            "City": "San Francisco",
            "State": "CA",
            "ZipCode": "94102"
          },
          "Salary": 125000,
          "Age": 38,
          "Phones": [
            "415-555-0111"
          ]
        }
      ]
    }

.. raw:: html

   </details><br/><br/>
  

The example below executes a query and converts the result to **IReadOnlyList<IEmployee>**.

In this example the type to use to de-serialize JSON array items in query result to employee instances in result of type **IReadOnlyList<IEmployee>** is determined as follows:

- JSON object with "Id"=100000001
    - Custom version of **TryMapJsonConversionType** returns **IEmployee** type, since the **JSON** object for this employee has **Employees** field, but it is empty.
    - If the custom version of **TryMapJsonConversionType** returned null for this JSON object, the delegate **TryMapJsonConversionType** configured in setup of `JsonQL.JsonToObjectConversion.IJsonConversionSettings <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IJsonConversionSettings.cs>`_ would return **IManager** as a type for this JSON object.
    - JsonQL maps the **IEmployee** type to default implementation **Employee**
    
- JSON object with "Id"=100000002
    - Custom version returns **IManager** type since the JSON object has **Employees** key with a value as a non-empty JSON array.
    - The delegate **TryMapJsonConversionType** configured in setup of `JsonQL.JsonToObjectConversion.IJsonConversionSettings <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IJsonConversionSettings.cs>`_ does not execute, since the custom version in code snippet below does not return null. Otherwise, if null was returned, JsonQL would execute the non-custom version of the delegate. 

- JSON object with "Id"=100000003
    - Custom version of **TryMapJsonConversionType** returns null, since the JSON being converted to **IEmployee** has no **Employees** key.
    - The call to delegate **TryMapJsonConversionType** configured in setup of `JsonQL.JsonToObjectConversion.IJsonConversionSettings <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IJsonConversionSettings.cs>`_ returns type **CustomEmployee** since the JSON object has a key **$type** with a value equal to the full name of this type. 

- JSON object with "Id"=100000004
    - Both custom version of **TryMapJsonConversionType** and the non-custom version return null. JsonQL uses the default implementation **Employee** of **IEmployee**.

.. sourcecode:: csharp

      // Select all employees
     var query = "Employees";

     // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
     // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
     // and it is normally configured as a singleton.
     JsonQL.Query.IQueryManager queryManager = null!;

     var employeesResult =
            queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
                new JsonTextData("EmployeesWithMissingData",
                    LoadJsonFileHelpers.LoadJsonFile("Employees.json", 
                        ["DocFiles", "QueryingJsonFiles", "ResultAsCSharpObject", 
                            "ConversionSettings", "TryMapJsonConversionType", "Example"])),
                jsonConversionSettingOverrides: new JsonConversionSettingsOverrides
                {
                    TryMapJsonConversionType =
                        (defaultTypeToConvertParsedJsonTo, convertedParsedJson) =>
                        {
                            if (defaultTypeToConvertParsedJsonTo == typeof(IEmployee))
                            {
                                // The delegate JsonQL.JsonToObjectConversion.IJsonConversionSettings.TryMapJsonConversionType
                                // will use IManager if "Employees" is present. Lets return IEmployees if JSON
                                // array Employees is either null or is empty.
                                if (convertedParsedJson.HasKey(nameof(IManager.Employees)) &&
                                    !(convertedParsedJson.TryGetJsonKeyValue(nameof(IManager.Employees), out var employeesJson) &&
                                      employeesJson.Value is IParsedArrayValue parsedArrayValue && parsedArrayValue.Values.Count > 0))
                                    return typeof(IEmployee);
                            }
                
                            // Returning null will result in either delegate used for
                            // JsonQL.JsonToObjectConversion.IJsonConversionSettings.TryMapJsonConversionType being used to map the type,
                            // or if the call to JsonQL.JsonToObjectConversion.IJsonConversionSettings.TryMapJsonConversionType returns 
                            // null, the default mapping mechanism picking a type to use.
                            return null;
                        }
                });
             
     Assert.That(employeesResult.ErrorsAndWarnings.ConversionErrors.Errors.Count, Is.EqualTo(0));
     Assert.That(employeesResult.ErrorsAndWarnings.ConversionWarnings.Errors.Count, Is.EqualTo(0));
     Assert.That(employeesResult.Value, Is.Not.Null);
     Assert.That(employeesResult.Value!.Count, Is.EqualTo(4));

     Assert.That(employeesResult.Value[0].GetType() == typeof(Employee));
     Assert.That(employeesResult.Value[1].GetType() == typeof(Manager));
     Assert.That(employeesResult.Value[2].GetType() == typeof(CustomEmployee));
     Assert.That(employeesResult.Value[3].GetType() == typeof(Employee));
  
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
            "$type": "JsonQL.Demos.Examples.DataModels.Manager, JsonQL.Demos",
            "Employees": {
              "$type": "System.Collections.Generic.List`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib",
              "$values": [
                {
                  "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
                  "Id": 100000004,
                  "FirstName": "Sara",
                  "LastName": "Lee",
                  "Address": {
                    "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
                    "Street": "33 Willow Road",
                    "City": "San Francisco",
                    "State": "CA",
                    "ZipCode": "94102",
                    "County": null
                  },
                  "Salary": 125000,
                  "Age": 38,
                  "Manager": null,
                  "Phones": {
                    "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
                    "$values": [
                      "415-555-0111"
                    ]
                  }
                }
              ]
            },
            "Id": 100000002,
            "FirstName": "Aisha",
            "LastName": "Khan",
            "Address": {
              "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
              "Street": "129 Pine Street",
              "City": "Boston",
              "State": "MA",
              "ZipCode": "02108",
              "County": null
            },
            "Salary": 88000,
            "Age": 32,
            "Manager": null,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": [
                "617-555-0199"
              ]
            }
          },
          {
            "$type": "JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.ConversionSettings.TryMapJsonConversionType.Example.CustomEmployee, JsonQL.Demos",
            "Id": 100000003,
            "FirstName": "Miguel",
            "LastName": "Santos",
            "Address": {
              "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
              "Street": "902 Palm Drive",
              "City": "Miami",
              "State": "FL",
              "ZipCode": "33101",
              "County": null
            },
            "Salary": 72000,
            "Age": 28,
            "Manager": null,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": [
                "305-555-0147",
                "305-555-0160"
              ]
            }
          },
          {
            "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
            "Id": 100000004,
            "FirstName": "Sara",
            "LastName": "Lee",
            "Address": {
              "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
              "Street": "33 Willow Road",
              "City": "San Francisco",
              "State": "CA",
              "ZipCode": "94102",
              "County": null
            },
            "Salary": 125000,
            "Age": 38,
            "Manager": null,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": [
                "415-555-0111"
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
