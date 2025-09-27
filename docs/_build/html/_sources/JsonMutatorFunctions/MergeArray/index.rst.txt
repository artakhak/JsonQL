====================
Merge Array '$merge'
====================

.. contents::
   :local:
   :depth: 2
   
- The function '$merge' allows merging an array evaluated by JsonQL expression that is passed to '$merge' into another JSON array.
- JsonQL will replace the string value which contains '$merge([JsonQL expression])' with array items generated from evaluating the JsonQL expression passed to '$merge' items.
    
    .. note::
        JsonQL injects array items generated from JsonQL expression used in '$merge', and not the array itself. In other words, the injected array items will not include '[' and ']' braces when injected into host array.
        
- The value of "$merge(Json QL expression)" cannot be preceded or succeeded by any other text.

Example
=======

Example in **Example.json** below demonstrates using '$value' function.

.. note:: The following JSON files are referenced in JsonQL expressions in **Example.json** in example below:  :doc:`../../MutatingJsonFiles/SampleFiles/companies`.

.. sourcecode:: json

    {
      "TestArray": [ 1, 3, 7, 9, "Test 1", 11, 13 ],

      "MergeArrayExample": [
        "TestArray array merged next",
        "$merge(TestArray)",
        "First 3 employees across all companies merged next",
        "$merge(Companies.Select(c => c.Employees).Where(e => index <= 2))",
        "End"
      ]
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
              "Test 1",
              11,
              13
            ],
            "MergeArrayExample": [
              "TestArray array merged next",
              1,
              3,
              7,
              9,
              "Test 1",
              11,
              13,
              "First 3 employees across all companies merged next",
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
              },
              "End"
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