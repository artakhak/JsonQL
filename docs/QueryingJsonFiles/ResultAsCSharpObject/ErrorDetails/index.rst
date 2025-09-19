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

For example the following JSON file stores serialized instance of `JsonQL.Query.IQueryResultErrorsAndWarnings <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryResultErrorsAndWarnings.cs>`_ for a failed query due to non-nullable property value missing in query result.

  .. note::
    This example stores number of details about an error that help identify the source of the error, but a summary error message is **"Failed to retrieve and set the value of non-nullable property [Address] in type [JsonQL.Demos.Examples.IQueryManagerExamples.FailureExamples.ResultAsObject.NonNullablePropertyValueMissing.DataModels.Employee]."**.

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
                             1
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
                         "Name": "1",
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
    For more examples demonstrating errors look at examples in this folder: `FailureExamples: <https://github.com/artakhak/JsonQL/tree/main/JsonQL.Demos/Examples/IQueryManagerExamples/FailureExamples/ResultAsObject>`_
 