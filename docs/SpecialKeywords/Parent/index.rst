================
Keyword 'parent'
================

.. contents::
   :local:
   :depth: 2

When JsonQL looks up JSON objects in referenced in JsonQL (for example when looking up array Companies in expression "Companies[0]"), it uses the following approach:

- Starts the search in JSON file where the expression is used, then looks up the JSON object in immediate parent JSON file in list of parent JSON files passed to methods in interfaces `JsonQL.Compilation.IJsonCompiler <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/IJsonCompiler.cs>`_ and `JsonQL.Query.IQueryManager <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryManager.cs>`_ (or in extension methods for these interfaces). Then looks in second parent JSON, and so forth.
- The first JSON object it finds in current JSON file or in one of parent JSON files is used as a result.

In situations when the looked up JSON object (an object with the same JSON key) is in JSON file where the expression is, as well as in one of parent JSON files, we might want to resolve the JSON object from parent file, rather than in current file. This is when 'parent' keyword is used.

Example
=======

.. note:: The following JSON files are referenced in JsonQL expressions in **Example.json** in example below:  :doc:`../../MutatingJsonFiles/SampleFiles/companies`.

**Example.json** below demonstrates using **index** parent.

.. sourcecode:: json

    {
      "Companies": [
        {
          "Employees": [
            {
              "Id": 500000001,
              "Name": "John Smith",
              "Address": {
                "Street": "456 Oak Avenue",
                "City": "Chicago",
                "State": "IL",
                "ZipCode": "60601"
              },
              "Salary": 99500,
              "Age": 45
            },
            {
              "Id": 500000002,
              "Name": "Aisha Khan",
              "Address": {
                "Street": "129 Pine Street",
                "City": "Boston",
                "State": "MA",
                "ZipCode": "02108"
              },
              "Salary": 88000,
              "Age": 32
            }
          ]
        }
      ],

      "EmployeeInParentFiles": "$value(parent.Companies.Select(c => c.Employees).First())",
      "EmployeeInThisFileFiles": "$value(Companies.Select(c => c.Employees).First())"
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
            "Companies": [
              {
                "Employees": [
                  {
                    "Id":  500000001,
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
                    "Id":  500000002,
                    "Name":  "Aisha Khan",
                    "Address": {
                      "Street":  "129 Pine Street",
                      "City":  "Boston",
                      "State":  "MA",
                      "ZipCode":  "02108"
                    },
                    "Salary":  88000,
                    "Age":  32
                  }
                ]
              }
            ],
            "EmployeeInParentFiles": {
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
            "EmployeeInThisFileFiles": {
              "Id":  500000001,
              "Name":  "John Smith",
              "Address": {
                "Street":  "456 Oak Avenue",
                "City":  "Chicago",
                "State":  "IL",
                "ZipCode":  "60601"
              },
              "Salary":  99500,
              "Age":  45
            }
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