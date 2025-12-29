=======================================================
Empty Query to Convert Json to Object Without Filtering
=======================================================

.. contents::
   :local:
   :depth: 2

There might be scenarios when we want to convert the root JSON that uses JsonQL expression (or even if it does not) to a C# object without applying any filter to the JSON file.
In such scenarios we can use an empty string as a query.

.. note:: Alternativelly we can use a query string **"parent"**.


In example below we query for a Json file **Employee.json** and convert the JSON to an instance of **IEmployee** without applying any filtering to JSON.
In other words, the root JSON is converted to a non-nullable **IEmployee** instance. 

- If the query results in **IEmployee** instances with null value for **Address** property of non-nullable type **IAddress**, of if any instance of **IEmployee** in query result has null value for **IEmployee.Address.Street** (**IAddress.Street** is of non-nullable **string** type), JsonQL will report conversion error(s) of type `JsonQL.JsonToObjectConversion.IConversionError <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IConversionError.cs>`_ in result with value of **ErrorType** equal to one of the following enum values: **NonNullablePropertyNotSet**.

.. raw:: html

   <details>
   <summary>Click to expand <b>Employee.json</b> used in query in example below</summary>

.. code-block:: json

    {
      "Id": 100000008,
      "FirstName": "Laura",
      "LastName": "Lee",
      "Address": "$value(Companies[0].Employees[0].Address)",
      "Salary": 105500,
      "Age": 32,
      "Phones": [
        "619-555-0155",
        "619-555-0122"
      ]
    }

.. raw:: html

   </details><br/><br/>
   
.. sourcecode:: csharp

     // Set the value of queryManager to an instance of JsonQL.Query.IQueryManager here.
     // The value of JsonQL.Query.IQueryManager is normally created by Dependency Injection container 
     // and it is normally configured as a singleton.
     JsonQL.Query.IQueryManager queryManager = null!;

     var query = string.Empty;

     // The result "employeesResult" is a non-nullable IEmployee value.
     var employeeResult =
         queryManager.QueryObject<IEmployee>(query,
         new JsonTextData("Employee",
         this.LoadExampleJsonFile("Employee.json")),
         convertedValueNullability: [
         // The result of type "IEmployee" cannot be null. An error will be reported if the value is null
         false]);


.. raw:: html

   <details>
   <summary>Click to expand the result of the query in example above (i.e., instance of <b>JsonQL.Query.IObjectQueryResult&lt;IEmployee&gt;</b>) serialized into <b>Result.json</b></summary>

.. code-block:: json

    {
      "$type": "JsonQL.Query.ObjectQueryResult`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], JsonQL",
      "Value": {
        "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
        "Id": 100000008,
        "FirstName": "Laura",
        "LastName": "Lee",
        "Address": {
          "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
          "Street": "456 Oak Avenue",
          "City": "Chicago",
          "State": "IL",
          "ZipCode": "60601",
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
