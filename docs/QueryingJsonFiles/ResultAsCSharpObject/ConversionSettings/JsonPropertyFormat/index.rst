==============================
Setting **JsonPropertyFormat**
==============================

.. contents::
   :local:
   :depth: 2

Setting **JsonPropertyFormat** determines the format of JSON keys for the deserialization of query result to C# object to succeed.

The value of setting **JsonPropertyFormat** (a property in `JsonQL.JsonToObjectConversion.IJsonConversionSettings <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IJsonConversionSettings.cs>`_ or in `JsonQL./JsonToObjectConversion.IJsonConversionSettingsOverrides <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IJsonConversionSettingsOverrides.cs>`_) is of enum type `JsonQL.JsonToObjectConversion.JsonPropertyFormat <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/JsonPropertyFormat.cs>`_ which has the following values:
  - CamelCase. Specifies a JSON property naming convention where words are concatenated without spaces, and each word following the first starts with an uppercase letter.
  - PascalCase. Specifies a JSON property naming convention where the first letter of each word, including the first word, is capitalized.

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

Also, lets assume the queries in examples below query the JSON file **Employees.json** shown below, which follows  camel case format:

.. sourcecode:: json

     {
       "Employees": [
         {
           "id": 100000005,
           "firstName": "Christopher",
           "lastName": "Garcia",
           "address": {
             "street": "654 Cedar Road",
             "city": "Phoenix",
             "state": "AZ",
             "zipCode": "85001"
           },
           "salary": 111000,
           "age": 29,
           "logins": [
             "cgarcia@sherwood.com",
             "cgarcia@gmail.com"
           ],
           "phones": [
             "602-555-0166",
             "602-555-0188"
           ]
         },
         {
           "id": 100000006,
           "firstName": "Emily",
           "lastName": "Nguyen",
           "address": {
             "street": "241 Maple Avenue",
             "city": "Seattle",
             "state": "WA",
             "zipCode": "98101"
           },
           "salary": 95000,
           "age": 50,
           "logins": [
             "enguyen@pacifictech.com",
             "emily.nguyen@gmail.com"
           ],
           "phones": [
             "206-555-0123",
             "206-555-0456"
           ]
         },
         {
           "id": 100000007,
           "firstName": "Daniel",
           "lastName": "O'Connor",
           "address": {
             "street": "18 Birch Lane",
             "city": "Denver",
             "state": "CO",
             "zipCode": "80202"
           },
           "salary": 78000,
           "age": 41,
           "logins": [
             "dan.oconnor@mountainco.com",
             "dan.oconnor@yahoo.com"
           ],
           "phones": [
             "303-555-0789"
           ]
         }
       ]
     }
     
The example below executes a query and converts the result to **IReadOnlyList<IEmployee>**. As it can be seen, the setting **JsonPropertyFormat** was overridden to use **JsonToObjectConversion.JsonPropertyFormat.CamelCase** value. 

.. sourcecode:: csharp

     // Select the employees older than 40
     var query = "Employees.Where(e => e.age > 40)";

     // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
     // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
     // and it is normally configured as a singleton.
     JsonQL.Query.IQueryManager queryManager = null!;

     var employeesResult =
          queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
              new JsonTextData("Employees",
                  this.LoadExampleJsonFile("Employees.json")),
              jsonConversionSettingOverrides: new JsonConversionSettingsOverrides
              {
                  JsonPropertyFormat = JsonToObjectConversion.JsonPropertyFormat.CamelCase
              });

     Assert.That(employeesResult.ErrorsAndWarnings.ConversionErrors.Errors.Count, Is.EqualTo(0));
     Assert.That(employeesResult.ErrorsAndWarnings.ConversionWarnings.Errors.Count, Is.EqualTo(0));
     Assert.That(employeesResult.Value, Is.Not.Null);
     Assert.That(employeesResult.Value!.Count, Is.EqualTo(2));
     
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
            "Id": 100000006,
            "FirstName": "Emily",
            "LastName": "Nguyen",
            "Address": {
              "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
              "Street": "241 Maple Avenue",
              "City": "Seattle",
              "State": "WA",
              "ZipCode": "98101",
              "County": null
            },
            "Salary": 95000,
            "Age": 50,
            "Manager": null,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": [
                "206-555-0123",
                "206-555-0456"
              ]
            }
          },
          {
            "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
            "Id": 100000007,
            "FirstName": "Daniel",
            "LastName": "O'Connor",
            "Address": {
              "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
              "Street": "18 Birch Lane",
              "City": "Denver",
              "State": "CO",
              "ZipCode": "80202",
              "County": null
            },
            "Salary": 78000,
            "Age": 41,
            "Manager": null,
            "Phones": {
              "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
              "$values": [
                "303-555-0789"
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