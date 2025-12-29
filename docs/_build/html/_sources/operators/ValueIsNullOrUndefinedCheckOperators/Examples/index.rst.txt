========
Examples
========

.. contents::
   :local:
   :depth: 2
   

Examples in **Examples.json** file below demonstrate logical operators described in :doc:`../index`

.. note:: The following JSON files are referenced in JsonQL expressions in **Examples.json** in example below:
    
    - :doc:`data`
    - :doc:`../../../MutatingJsonFiles/SampleFiles/companies`


.. sourcecode:: json

    {
      "Operator_is_null": {
        "Comment_Operator_is_null_1": "(invalid.path is null) evaluates to false. For invalid path check use 'is undefined' operator",
        "Operator_is_null_1": "$value(invalid.path is null == false)",
        "Operator_is_null_2": "$value(NullValue is null)",
        "Operator_is_null_3": "$value(Int1 is null == false)",
        "Operator_is_null_5": "$value(Companies.Select(c => c.Employees).First() is null == false)",

        "Comment_Operator_is_null_6_1": "Even though the query below results in no employee being selected. The value is not null",
        "Comment_Operator_is_null_6_2": "'is null' succeeds if there is JsonPath with null value.",
        "Comment_Operator_is_null_6_3": "'Use 'is undefined' instead to check if query resulted in any value",
        "Operator_is_null_6": "$value(Companies.Select(c => c.Employees).First(e => e.Age > 200) is null == false)"
      },
      "Operator_is_not_null": {
        "Comment_Operator_is_not_null_1": "(invalid.path is not null) evaluates to true.",
        "Operator_is_not_null_1": "$value(invalid.path is not null)",
        "Operator_is_not_null_2": "$value(NullValue is not null == false)",
        "Operator_is_not_null_3": "$value(Int1 is not null)",
        "Operator_is_not_null_5": "$value(Companies.Select(c => c.Employees).First() is not null)",
        "Operator_is_not_null_6": "$value(Companies.Select(c => c.Employees).First(e => e.Age > 200) is not null)"
      },
      "Operator_is_undefined": {
        "Comment_Operator_is_undefined_1": "(invalid.path is undefined) evaluates to true.",
        "Operator_is_undefined_1": "$value(invalid.path is undefined)",
        "Operator_is_undefined_2": "$value(NullValue is undefined == false)",
        "Operator_is_undefined_3": "$value(Int1 is undefined == true)",
        "Operator_is_undefined_5": "$value(Companies.Select(c => c.Employees).First() is undefined == false)",
        "Operator_is_undefined_6": "$value(Companies.Select(c => c.Employees).First(e => e.Age > 200) is undefined)"
      },
      "Operator_is_ not_undefined": {
        "Comment_Operator_is_not_undefined_1": "(invalid.path is not undefined) evaluates to false.",
        "Operator_is_not_undefined_1": "$value(invalid.path is not undefined == false)",
        "Operator_is_not_undefined_2": "$value(NullValue is not undefined)",
        "Operator_is_not_undefined_3": "$value(Double1 is not undefined)",
        "Operator_is_not_undefined_5": "$value(Companies.Select(c => c.Employees).First() is not undefined)",
        "Operator_is_not_undefined_6": "$value(Companies.Select(c => c.Employees).First(e => e.Age > 200) is not undefined == false)"
      }
    }

    
The result (i.e., an instance of `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationResult.cs>`_) is serialized to a **Result.json** file below.

.. note::
    For brevity, the serialized result includes only serialized evaluated **Examples.json** and does not include parent JSON files in **JsonQL.Compilation.ICompilationResult.CompiledJsonFiles**

.. raw:: html

   <details>
   <summary>Click to expand the result in instance of <b>JsonQL.Compilation.ICompilationResult</b> serialized into <b>Result.json</b></summary>

.. code-block:: json

    {
      "CompiledJsonFiles":[
        {
          "TextIdentifier": "Examples",
          "CompiledParsedValue":
          {
            "Operator_is_null": {
              "Comment_Operator_is_null_1":  "(invalid.path is null) evaluates to false. For invalid path check use 'is undefined' operator",
              "Operator_is_null_1":  true,
              "Operator_is_null_2":  true,
              "Operator_is_null_3":  true,
              "Operator_is_null_5":  true,
              "Comment_Operator_is_null_6_1":  "Even though the query below results in no employee being selected. The value is not null",
              "Comment_Operator_is_null_6_2":  "'is null' succeeds if there is JsonPath with null value.",
              "Comment_Operator_is_null_6_3":  "'Use 'is undefined' instead to check if query resulted in any value",
              "Operator_is_null_6":  true
            },
            "Operator_is_not_null": {
              "Comment_Operator_is_not_null_1":  "(invalid.path is not null) evaluates to true.",
              "Operator_is_not_null_1":  true,
              "Operator_is_not_null_2":  true,
              "Operator_is_not_null_3":  true,
              "Operator_is_not_null_5":  true,
              "Operator_is_not_null_6":  true
            },
            "Operator_is_undefined": {
              "Comment_Operator_is_undefined_1":  "(invalid.path is undefined) evaluates to true.",
              "Operator_is_undefined_1":  true,
              "Operator_is_undefined_2":  true,
              "Operator_is_undefined_3":  true,
              "Operator_is_undefined_5":  true,
              "Operator_is_undefined_6":  true
            },
            "Operator_is_ not_undefined": {
              "Comment_Operator_is_not_undefined_1":  "(invalid.path is not undefined) evaluates to false.",
              "Operator_is_not_undefined_1":  true,
              "Operator_is_not_undefined_2":  true,
              "Operator_is_not_undefined_3":  true,
              "Operator_is_not_undefined_5":  true,
              "Operator_is_not_undefined_6":  true
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

.. raw:: html

   </details><br/><br/>

   
The code snippet shows how the JSON file **Examples.json** was parsed using `JsonQL.Compilation.IJsonCompiler <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/IJsonCompiler.cs>`_

.. sourcecode:: csharp

    // Set the value of jsonCompiler to an instance of JsonQL.Compilation.IJsonCompiler here.
    // The value of JsonQL.Compilation.JsonCompiler is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.
    JsonQL.Compilation.IJsonCompiler jsonCompiler = null!;

    var sharedExamplesFolderPath = new []
    {
        "DocFiles", "MutatingJsonFiles", "Examples"
    };

    var dataJsonTextData = new JsonTextData("Data", this.LoadExampleJsonFile("Data.json"));

    var companiesJsonTextData = new JsonTextData("Companies",
        LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath), dataJsonTextData);

    var result = jsonCompiler.Compile(new JsonTextData("Examples",
        this.LoadExampleJsonFile("Examples.json"), companiesJsonTextData));