=============
Error Details
=============

.. contents::
   :local:
   :depth: 2

- If evaluated query or referenced JSON files have errors, the error details are stored in property **CompilationErrors** of interface `JsonQL.Query.IJsonValueQueryResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IJsonValueQueryResult.cs>`_.
    .. note::
        See :doc:`../../../ImportantInterfaces/IJsonValueQueryResult/index` for more details on data structure used for the result.

- Each item in property **CompilationErrors** in `JsonQL.Query.IJsonValueQueryResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IJsonValueQueryResult.cs>`_ is of type `JsonQL.Compilation.ICompilationErrorItem <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationErrorItem.cs>`_ which has details on an error such as:
    - JSON file identifier that resulted in error.
    - Position in JSON file or query for the error.
    - Error message.
    
- Errors can happen for number of reasons, some of which are listed here:
    - Missing or extra closing braces for functions or arrays.
    - Unknown symbols used in JsonQL expressions.
    - :doc:`../../../Operators/Assert/index` functions failing the evaluated expressions.
    
- JsonQL uses `JsonQL.Compilation.ICompilationResultLogger <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationResultLogger.cs>`_ to log the error details. The implementation of this interface can be replaced if necessary.

Examples
========

Below are couple of examples showing errors in queried files as well as in query itself.

.. note::
    For more examples demonstrating errors look at examples in this folder: `FailureExamples <https://github.com/artakhak/JsonQL/tree/main/JsonQL.Demos/Examples/IJsonCompilerExamples/FailureExamples>`_
 
Missing closing brace in queried JSON file:
-------------------------------------------

The C# code snippet below queries JSON file **Data.json** below. The query results in errors since the referenced file has a JsonQL function with missing closing braces.

Queried **Data.json** file:

.. sourcecode:: json

    {  
      "TestData": [ 1, 3, 4, "5", "TEST", 7 ],
      "JsonQlExpressionWithMissinClosingBraces": "$value(ToInt(TestData[3]) + 1"
    }


.. sourcecode:: csharp

    // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
    // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.
    JsonQL.Query.IQueryManager queryManager = null!;

    // This query will fail since the value of JSON key "JsonQlExpressionWithMissingClosingBraces"
    // is a JsonQL "$value(ToInt(TestData[3]) + 1" that has no closing brace for
    // "$value(" function.
    var query = "JsonQlExpressionWithMissingClosingBraces";

    var queryResult =
        queryManager.QueryJsonValue(query,
            new JsonTextData("Data",
                this.LoadExampleJsonFile("Data.json")));


The result (an instance of `JsonQL.Query.IJsonValueQueryResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IJsonValueQueryResult.cs>`_) is serialized to a **Result.json** file below.
    
.. raw:: html

   <details>
   <summary>Click to expand the result of the query in example above (i.e., instance of <b>JsonQL.Query.IJsonValueQueryResult&lt;IReadOnlyList&lt;IEmployee&gt;&gt;</b>) serialized into <b>Result.json</b></summary>

.. code-block:: json

    {
      "ParsedValue": null,
      "CompilationErrors":
      {
        "$type": "System.Collections.Generic.List`1[[JsonQL.Compilation.ICompilationErrorItem, JsonQL]], System.Private.CoreLib",
        "$values": [
          {
            "$type": "JsonQL.Compilation.CompilationErrorItem, JsonQL",
            "JsonTextIdentifier": "Data",
            "LineInfo": {
              "$type": "JsonQL.JsonObjects.JsonLineInfo, JsonQL",
              "LineNumber": 3,
              "LinePosition": 53
            },
            "ErrorMessage": "Closing brace ')' is missing."
          }
        ]
      }
    }


.. raw:: html

   </details><br/><br/>
   
The screenshot below shows the error details logged using the data in property **CompilationErrors** in `JsonQL.Query.IJsonValueQueryResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IJsonValueQueryResult.cs>`_.

 .. image:: Examples/MissingBraceInQueriedFileExample/missing-closing-brace-error.jpg

Invalid parameters in query
---------------------------

The C# code snippet below queries JSON file **Data.json**. The query results in errors since the query uses invalid number of parameter in a call to a JsonQL function.

Queried **Data.json** file:

.. sourcecode:: json

    {
      "TestData": [ 1, 3, 4, "TEST", 7 ]
    }

    
.. sourcecode:: csharp

    var query = "Lower(TestData[3])";

    // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
    // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.
    JsonQL.Query.IQueryManager queryManager = null!;

    // This query will succeed.
    var queryResult =
        queryManager.QueryJsonValue(query,
            new JsonTextData("Data",
                this.LoadExampleJsonFile("Data.json")));

    Assert.That(queryResult.ParsedValue is IParsedSimpleValue {Value: "test"});

    // This query will fail as it has extra parameter.
    query = "Lower(TestData[3], 7)";

    queryResult =
        _queryManager.QueryJsonValue(query,
            new JsonTextData("Data",
                this.LoadExampleJsonFile("Data.json")));


The result (an instance of `JsonQL.Query.IJsonValueQueryResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IJsonValueQueryResult.cs>`_) is serialized to a **Result.json** file below.
    
.. raw:: html

   <details>
   <summary>Click to expand the result of the query in example above (i.e., instance of <b>JsonQL.Query.IJsonValueQueryResult&lt;IReadOnlyList&lt;IEmployee&gt;&gt;</b>) serialized into <b>Result.json</b></summary>

.. code-block:: json

    {
      "ParsedValue": null,
      "CompilationErrors":
      {
        "$type": "System.Collections.Generic.List`1[[JsonQL.Compilation.ICompilationErrorItem, JsonQL]], System.Private.CoreLib",
        "$values": [
          {
            "$type": "JsonQL.Compilation.CompilationErrorItem, JsonQL",
            "JsonTextIdentifier": "Query_849E0817-3256-483D-8E97-01744EBC3F76",
            "LineInfo": {
              "$type": "JsonQL.JsonObjects.JsonLineInfo, JsonQL",
              "LineNumber": 2,
              "LinePosition": 18
            },
            "ErrorMessage": "Too many parameters provided for function [Lower]. Expected at most 1 parameters."
          }
        ]
      }
    }


.. raw:: html

   </details><br/><br/>
   
The screenshot below shows the error details logged using the data in property **CompilationErrors** in `JsonQL.Query.IJsonValueQueryResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IJsonValueQueryResult.cs>`_.

 .. image:: Examples/InvalidParameterInQueryExample/invalid-parameter-error.jpg
