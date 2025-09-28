=====
Where
=====

.. contents::
   :local:
   :depth: 2
   
- Path function **Where** is used to select items in a collection (i.e., JSON array or a collection resulted by evaluation of other JsonQL functions, such as 'Where', 'Select', etc.).

Function Parameters
===================

- **criteria**:
    - Required: No
    - Type: lambda function of type "x => boolean"
    - Description: A criteria used to filter the result.


Example
=======

.. note:: The following JSON files are referenced in JsonQL expressions in **Example.json** in example below:  :doc:`../../MutatingJsonFiles/SampleFiles/companies`.

**Example.json** below demonstrates using **Where** path function.


.. sourcecode:: json

    {
      "WhereExample": 
        "$value(Companies.Where(c => c.Data.Address.State != 'CO').Select(c => c.Employees.Where(e => e.Age < 40)).Where(e => e.Id != 100000010).Last())"
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
            "WhereExample": {
              "Id":  100000008,
              "Name":  "Laura Lee",
              "Address": {
                "Street":  "258 Willow Lane",
                "City":  "San Diego",
                "State":  "CA",
                "ZipCode":  "92101"
              },
              "Salary":  105500,
              "Age":  32
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

    var sharedExamplesFolderPath = new[]
            {
                "DocFiles", "MutatingJsonFiles", "Examples"
            };

    var companiesJsonTextData = new JsonTextData("Companies",
        LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath));

    var result = jsonCompiler.Compile(new JsonTextData("Example",
        this.LoadExampleJsonFile("Example.json"), companiesJsonTextData));