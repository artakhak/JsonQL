=======
Reverse
=======

.. contents::
   :local:
   :depth: 2
   
- Path function **Reverse** is used to reverse items in a collection.

Function Parameters
===================
- None

Example
=======

.. note:: The following JSON files are referenced in JsonQL expressions in **Example.json** in example below:  :doc:`../../MutatingJsonFiles/SampleFiles/companies`.

**Example.json** below demonstrates using **Reverse** path function.

.. sourcecode:: json

    {
      "SelectFirstThreeEmployeesAcrossAllCompanies": 
        "$value(Companies.Select(c => c.Employees).Where(x => index <= 2).Reverse())",

      "SelectRaisedSalariesOfFirstThreeEmployeesAcrossAllCompanies": 
        "$value(Companies.Select(c => c.Employees).Reverse().Where(x => index <= 2))"
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
              }
            ],
            "SelectRaisedSalariesOfFirstThreeEmployeesAcrossAllCompanies": [
              {
                "Id":  783328759,
                "Name":  "Robert Brown",
                "Address": {
                  "Street":  "Pine St",
                  "City":  "Los Angeles",
                  "State":  "CA",
                  "ZipCode":  "90001"
                },
                "Salary":  122395,
                "Age":  58
              },
              {
                "Id":  250150245,
                "Name":  "Jane Doe",
                "Address": {
                  "Street":  "Main St",
                  "City":  "San Jose",
                  "State":  "PA",
                  "ZipCode":  "95101"
                },
                "Salary":  144186,
                "Age":  63
              },
              {
                "Id":  100000010,
                "Name":  "Jessica Thompson",
                "Address": {
                  "Street":  "159 Cherry Lane",
                  "City":  "Austin",
                  "State":  "TX",
                  "ZipCode":  "73301"
                },
                "Salary":  98700,
                "Age":  37
              }
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