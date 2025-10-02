======
Select
======

.. contents::
   :local:
   :depth: 2
   
- Path function **Select** is used to project each item in a collection to another item in a collection.
- Currently we can use project items to calculated values that uses path functions as well as JsonQL expressions (e.g., "e => e.Salary * 2").

    .. note::
        Next release will support also more complex projections to create new JSON objects and arrays (e.g., "e => {"Name" : Concatenate(e.FirstName, ' ', e.LastName)}", "e => [e.Addresses.Where(a => a.State != 'CO')]", etc.).
        This improvement will be added based on demand for jsonQL as well as on this feature, since the author might be involved with other things in near future.

Function Parameters
===================

- **path**:
    - Required: Yes
    - Type: lambda function of type "x => any"
    - Description: A lambda function for projecting a collection item to a valid JsonQL expression.

Example
=======

.. note:: The following JSON files are referenced in JsonQL expressions in **Example.json** in example below:  :doc:`../../MutatingJsonFiles/SampleFiles/companies`.

**Example.json** below demonstrates using **Select** path function.

.. sourcecode:: json

    {
      "SelectFirstThreeEmployeesAcrossAllCompanies": 
        "$value(Companies.Select(c => c.Employees).Where(x => index <= 2))",

      "SelectRaisedSalariesOfFirstThreeEmployeesAcrossAllCompanies": 
        "$value(Companies.Select(c => c.Employees).Where(x => index <= 2).Select(e => 2 * e.Salary))",

      "SelectUsingNamedParameter": "$value(Companies.Select(path -> c => c.CompanyData.Name))"
    }
    
The result (i.e., an instance of `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationResult.cs>`_) is serialized to a **Result.json** file below.

.. note::
    For brevity, the serialized result includes only serialized evaluated **Example.json** and does not include parent JSON files in **JsonQL.Compilation.ICompilationResult.CompiledJsonFiles**
 
.. sourcecode:: json

    {
      "CompiledJsonFiles":[
        {
          "TextIdentifier": "Example",
          "CompiledParsedValue":
          {
            "SelectFirstThreeEmployeesAcrossAllCompanies": [
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
            "SelectRaisedSalariesOfFirstThreeEmployeesAcrossAllCompanies": [
              199000,
              210000,
              178000
            ],
            "SelectUsingNamedParameter": [
              "Strange Things, Inc",
              "Sherwood Forest Timber, Inc",
              "Atlantic Transfers, Inc"
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