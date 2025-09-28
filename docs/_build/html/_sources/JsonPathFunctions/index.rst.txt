===================
JSON Path Functions
===================

.. contents::
   :local:
   :depth: 2
   
.. toctree::

   ArrayIndexers/index.rst
   At/index.rst
   First/index.rst
   Last/index.rst
   Where/index.rst 
   Select/index.rst
   Flatten/index.rst
   Reverse/index.rst
   
- JsonQL path functions are used along with one or more  '.' separators (i.e., multiple functions can be applied in sequence) to access values in JSON files. 
- The sections below describe the available JsonQL path functions, but the example below is a simple example demonstrating use of multiple chained functions (i.e., 'Where' and 'First') along with chained access to JSON objects and fields (i.e, 'Employees', and 'Address') :
- There is no limit on how many times you may apply the JSON path functions, or access JSON objects and fields in chained manner.

Example
-------

.. note:: The following JSON files are referenced in JsonQL expressions in **Example.json** in example below:  :doc:`../MutatingJsonFiles/SampleFiles/employees`.

**Example.json** below demonstrates using optional and named parameters with JsonQL.

.. sourcecode:: json

    {
      "FirstEmployeeAddress": "$value(Employees.Where(x => x.Age >= 40).First().Address)"
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
            "FirstEmployeeAddress": {
              "Street":  "456 Oak Avenue",
              "City":  "Chicago",
              "State":  "IL",
              "ZipCode":  "60601"
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

    var companiesJsonTextData = new JsonTextData("Employees",
        LoadJsonFileHelpers.LoadJsonFile("Employees.json", sharedExamplesFolderPath));

    var result = jsonCompiler.Compile(new JsonTextData("Example",
        this.LoadExampleJsonFile("Example.json"), companiesJsonTextData));