=============
Error Details
=============

.. contents::
   :local:
   :depth: 2

- Data about errors resulted from executing the the method **JsonQL.Query.IQueryManager.QueryObject<QueryObject>(...)**  is stored in property **ErrorsAndWarnings** in type `JsonQL.Query.IObjectQueryResult[TQueryObject] <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IObjectQueryResult.cs>`_.
    .. note::
        - The property is actually in parent interface `JsonQL.Query.IObjectQueryResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IObjectQueryResult.cs>`_

- The property **ErrorsAndWarnings** is of type `JsonQL.Query.IQueryResultErrorsAndWarnings <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryResultErrorsAndWarnings.cs>`_ 
- Interface `JsonQL.Query.IQueryResultErrorsAndWarnings <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryResultErrorsAndWarnings.cs>`_ has the following properties for error details:
  
  - **CompilationErrors**: stores collection of `JsonQL.Compilation.ICompilationErrorItem <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationErrorItem.cs>`_ for errors occurred while evaluating queried JSON files and JsonQL expressions (both expressions in queried JSON files as well as JsonQL expressions in query itself). These errors happen before jsonQL attempts to convert resulted JSON value (e.g., simple JSON value, JSON object or JSON array) to a C# type instance.
  - **ConversionErrors**: stores a property of type `JsonQL.JsonToObjectConversion.IConversionErrors <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IConversionErrors.cs>`_ for errors occurred while converting query result to specified C# type instance.
  - **ConversionWarnings**: stores a property of type `JsonQL.JsonToObjectConversion.IConversionErrors <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IConversionErrors.cs>`_ for warnings occurred while converting query result to specified C# type instance.
  
  .. note::
    Properties **ConversionErrors** and **ConversionWarnings** are of the same type. The only difference is that errors that do not result in query failure. Classification of some error types (error or warning) can be configured using settings (see :doc:`../ConversionSettings/index` for more details).

  .. note::
    Errors are logged by `JsonQL.Compilation.ICompilationResultLogger <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationResultLogger.cs>`_ and custom implementation of this interface can be provided.

Couple of examples are demonstrated in sections that follow. For more examples demonstrating errors look at examples in this folder: `FailureExamples: <https://github.com/artakhak/JsonQL/tree/main/JsonQL.Demos/Examples/IQueryManagerExamples/FailureExamples/ResultAsObject>`_
            
Examples
========

Compilation error
-----------------

The code snippet below uses a query that contains an invalid expression (closing brace missing). The list employeesResult.ErrorsAndWarnings.CompilationErrors.Errors will not be empty and will contain one compilation error. 

.. sourcecode:: csharp

     // This query will result in compilation error since closing brace is missing for open brace.
     var query = "Employees.Where(x => x.Salary >= 100000";

     // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
     // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
     // and it is normally configured as a singleton.
     JsonQL.Query.IQueryManager queryManager = null!;

     var employeesResult =
         queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
             new JsonTextData("Employees", this.LoadExampleJsonFile("Employees.json")));

     Assert.That(employeesResult.ErrorsAndWarnings.CompilationErrors.Count, Is.GreaterThan(0));
     
The JSON below stores serialized instance of `JsonQL.Query.IQueryResultErrorsAndWarnings <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryResultErrorsAndWarnings.cs>`_ for a failed query in example above.

.. sourcecode:: json

     {
       "$type": "JsonQL.Query.ObjectQueryResult`1[[System.Collections.Generic.IReadOnlyList`1[[JsonQL.Demos.Examples.IQueryManagerExamples.FailureExamples.ResultAsObject.NonNullablePropertyValueMissing.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib]], JsonQL",
       "Value": null,
       "ErrorsAndWarnings": {
         "$type": "JsonQL.Query.QueryResultErrorsAndWarnings, JsonQL",
         "CompilationErrors": {
           "$type": "System.Collections.Generic.List`1[[JsonQL.Compilation.ICompilationErrorItem, JsonQL]], System.Private.CoreLib",
           "$values": [
             {
               "$type": "JsonQL.Compilation.CompilationErrorItem, JsonQL",
               "JsonTextIdentifier": "Query_849E0817-3256-483D-8E97-01744EBC3F76",
               "LineInfo": {
                 "$type": "JsonQL.JsonObjects.JsonLineInfo, JsonQL",
                 "LineNumber": 2,
                 "LinePosition": 33
               },
               "ErrorMessage": "Closing brace ')' is missing."
             }
           ]
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


Running the query above results in the following error log generated by `JsonQL.Compilation.ICompilationResultLogger <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationResultLogger.cs>`_
 
.. image:: logged-errors.jpg

Conversion error
----------------

The code snippet below uses a query that tries to convert the result of a query to list of **IEmployee** instances, and some JSON objects in query JSON file Employees.json miss **Address** value for non-nullable property **IEmployee.Address**. The list employeesResult.ErrorsAndWarnings.ConversionErrors.Errors will not be empty and will contain one compilation error.

.. sourcecode:: csharp

     // This query will fail since not all values of IEmployee.Age are non-null in a result set.
     var query = "Employees.Where(x => x.Salary >= 100000)";

     var employeesResult =
         _queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
             new JsonTextData("Employees", this.LoadExampleJsonFile("Employees.json")), 
             convertedValueNullability:null,
             jsonConversionSettingOverrides:
             // NOTE: jsonConversionSettingOverrides parameter of type IJsonConversionSettingsOverrides
             // is an optional, and we do not have to provide this parameter of default settings work for us (which is most of the cases).
             // However, the parameter is specified here as an example
             new JsonConversionSettingsOverrides
             {
                 ConversionErrorTypeConfigurations = [
                     // Note, we only need to provide configurations that we want to override.
                     // Default configurations will be used for any error type that is not specified in 
                     // ConversionErrorTypeConfigurations collection
                     new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullablePropertyNotSet, 
                         // The default error reporting type of ConversionErrorType.NonNullablePropertyNotSet is
                         // ErrorReportingType.ReportAsError.
                         // This is just a demo how the default configuration can be overridden
                         ErrorReportingType.ReportAsError)] 
             });
     Assert.That(employeesResult.ErrorsAndWarnings.ConversionErrors.Errors.Count, Is.GreaterThan(0));
     
The JSON below stores serialized instance of `JsonQL.Query.IQueryResultErrorsAndWarnings <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryResultErrorsAndWarnings.cs>`_ for a failed query in example above.

.. sourcecode:: json

     {
       "$type": "JsonQL.Query.ObjectQueryResult`1[[System.Collections.Generic.IReadOnlyList`1[[JsonQL.Demos.Examples.IQueryManagerExamples.FailureExamples.ResultAsObject.NonNullablePropertyValueMissing.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib]], JsonQL",
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
                   "JsonTextIdentifier": "Employees",
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
                 "Error": "Failed to retrieve and set the value of non-nullable property [Address] in type [JsonQL.Demos.Examples.IQueryManagerExamples.FailureExamples.ResultAsObject.NonNullablePropertyValueMissing.DataModels.Employee].",
                 "ConvertedObjectPath": {
                   "$type": "JsonQL.JsonToObjectConversion.ConvertedObjectPath.ConvertedObjectPath, JsonQL",
                   "RootConvertedObjectPathElement": {
                     "$type": "JsonQL.JsonToObjectConversion.ConvertedObjectPath.RootConvertedObjectPathElement, JsonQL",
                     "Name": "Root",
                     "ObjectType": "System.Collections.Generic.IReadOnlyList`1[[JsonQL.Demos.Examples.IQueryManagerExamples.FailureExamples.ResultAsObject.NonNullablePropertyValueMissing.DataModels.IEmployee, JsonQL.Demos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"
                   },
                   "Path": {
                     "$type": "System.Collections.Generic.List`1[[JsonQL.JsonToObjectConversion.ConvertedObjectPath.IConvertedObjectPathValueSelectorElement, JsonQL]], System.Private.CoreLib",
                     "$values": [
                       {
                         "$type": "JsonQL.JsonToObjectConversion.ConvertedObjectPath.IndexConvertedObjectPathElement, JsonQL",
                         "Name": "0",
                         "ObjectType": "JsonQL.Demos.Examples.IQueryManagerExamples.FailureExamples.ResultAsObject.NonNullablePropertyValueMissing.DataModels.IEmployee, JsonQL.Demos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
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


.. note::
  This example stores number of details about an error that help identify the source of the error, but a summary error message is **"Failed to retrieve and set the value of non-nullable property [Address] in type [JsonQL.Demos.Examples.IQueryManagerExamples.FailureExamples.ResultAsObject.NonNullablePropertyValueMissing.DataModels.Employee]."**.

**Here is the JSON file queried by this example:**

.. sourcecode:: json

     {
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
           "Phones": null
         },
         {
           "Id": 100000002,
           "FirstName": "Alice",
           "LastName": "Johnson",
           "Address": null,
           "Salary": 105000,
           "Age": 37,
           "Phones": [ "111-222-3333", "111-222-4444" ]
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
           "Age": 50
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
           "Age": 42
         }
       ]
     }

