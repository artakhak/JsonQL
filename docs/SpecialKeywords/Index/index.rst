===============
Keyword 'index'
===============

.. contents::
   :local:
   :depth: 2
   
- Special keyword **index** can be used in lambda functions used with collections and its value is a 0 based index of a collection item being iterated.
- The **index** keyword can be used in some scenarios such as selecting only range of items from a collection. 

Example
=======

.. note:: The following JSON files are referenced in JsonQL expressions in **Example.json** in example below:  :doc:`../../MutatingJsonFiles/SampleFiles/companies`.

**Example.json** below demonstrates using **index** function.


.. sourcecode:: json

    {
      "TestArray": [ 1, 3, 7, 9, 11, 13 ],

      "SelectTheFirstThreeEmployees": "$value(Companies.Select(c => c.Employees).Where(e => index <= 2))",
      "SelectAllItemsExceptFirstAndLast": "$value(TestArray.Where(x => index > 0 && index < Count(TestArray) - 1))"
    }

    
The result (i.e.,an instance of `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationResult.cs>`_) is serialized to a **Result.json** file below.

.. note::
    For brevity, the serialized result includes only serialized evaluated **Example.json** and does not include parent JSON files in **JsonQL.Compilation.ICompilationResult.CompiledJsonFiles**
 
.. sourcecode:: json

    {
      "CompiledJsonFiles":[
        {
          "TextIdentifier": "Example",
          "CompiledParsedValue":
          {
            "TestArray": [
              1,
              3,
              7,
              9,
              11,
              13
            ],
            "SelectTheFirstThreeEmployees": [
              {
                "Id":  100000001,
                "Name":  "John Smith",
                "Address": {
                  "Street":  "456 Oak Avenue",
                  "City":  "Chicago",
                  "State":  "IL",
                  "ZipCode":  "60601"
                },
                "Salary":  99500,
                "Age":  45
              },
              {
                "Id":  100000002,
                "Name":  "Alice Johnson",
                "Address": {
                  "Street":  "123 Maple Street",
                  "City":  "New York",
                  "State":  "NY",
                  "ZipCode":  "10001"
                },
                "Salary":  105000,
                "Age":  38
              },
              {
                "Id":  100000003,
                "Name":  "Michael Brown",
                "Address": {
                  "Street":  "789 Pine Lane",
                  "City":  "Los Angeles",
                  "State":  "CA",
                  "ZipCode":  "90001"
                },
                "Salary":  89000,
                "Age":  50
              }
            ],
            "SelectAllItemsExceptFirstAndLast": [
              3,
              7,
              9,
              11
            ]
          }
        }
      ],
      "CompilationErrors":
      {
        "$type": "System.Collections.Generic.List`1[[JsonQL.Compilation.ICompilationErrorItem, JsonQL]], System.Private.CoreLib",
        "$values": []
      }
    }
   
The code snippet shows how the JSON file **Example.json** was parsed using `JsonQL.Compilation.IJsonCompiler <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/IJsonCompiler.cs>`_

.. sourcecode:: csharp

    // Set the value of jsonCompiler to an instance of JsonQL.Compilation.IJsonCompiler here.
    // The value of JsonQL.Compilation.JsonCompiler is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.
    JsonQL.Compilation.IJsonCompiler jsonCompiler = null!;

    var sharedExamplesFolderPath = new []
    {
        "DocFiles", "MutatingJsonFiles", "Examples"
    };

    var companiesJsonTextData = new JsonTextData("Companies",
        LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath));

    var result = jsonCompiler.Compile(new JsonTextData("Example",
        this.LoadExampleJsonFile("Example.json"), companiesJsonTextData));