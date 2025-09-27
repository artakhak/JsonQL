========================
String Interpolation '$'
========================

.. contents::
   :local:
   :depth: 2
   
- The string interpolation function '$' allows injecting one or more JsonQL expressions inside a string JsonValue using a format of type "$([Json QL Expression as a parameter])". 
- JsonQL evaluates all the occurrences of "$()" in JSON string value, and replaces them with evaluated expressions.
- The JsonQL expressions used in '$' function should evaluate to a string (e.g., string, numeric, date/time or boolean values).

Example
=======

Example in **Example.json** below demonstrates using '$' function.

.. note:: The following JSON files are referenced in JsonQL expressions in **Example.json** in example below:  :doc:`../../MutatingJsonFiles/SampleFiles/companies`.

.. sourcecode:: json

    {
      "TestArray": [ 1, 3, 7, 9, 11, 13 ],

      "Comment1": "We can have multiple occurrences of '$' functions in",
      "Comment2": "JSON, values below. Using one in each to make lines shorter",
      "Example1": "Sum(TestArray): $(Sum(TestArray.Where(x => x > 3)))",
      "Example2": "Average salary: $(Average(Companies.Select(c => c.Employees.Select(e => e.Salary))))."
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
            "Comment1":  "We can have multiple occurrences of '$' functions in",
            "Comment2":  "JSON, values below. Using one in each to make lines shorter",
            "Example1":  "Sum(TestArray): $(Sum(TestArray.Where(x => x > 3)))",
            "Example2":  "Average salary: $(Average(Companies.Select(c => c.Employees.Select(e => e.Salary))))."
          }
        }
      ],
      "CompilationErrors":
      {
        "$type": "System.Collections.Generic.List`1[[JsonQL.Compilation.ICompilationErrorItem, JsonQL]], System.Private.CoreLib",
        "$values": [
          {
            "$type": "JsonQL.Compilation.CompilationErrorItem, JsonQL",
            "JsonTextIdentifier": "Example",
            "LineInfo": {
              "$type": "JsonQL.JsonObjects.JsonLineInfo, JsonQL",
              "LineNumber": 4,
              "LinePosition": 54
            },
            "ErrorMessage": "Mutator function [$] should be followed by opening brace '('"
          }
        ]
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