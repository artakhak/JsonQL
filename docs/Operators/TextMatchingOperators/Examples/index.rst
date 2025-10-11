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
      "Operator_starts_with": {
        "Comment_Operator_starts_with_1": "(invalid.path starts with 'test') is evaluated to false",
        "Operator_starts_with_1": "$value(invalid.path starts with 'test' == false)",

        "Comment_Operator_starts_with_2": "('test' starts with invalid.path) is evaluated to false",
        "Operator_starts_with_2": "$value('test' starts with invalid.path == false)",

        "Comment_Operator_starts_with_3": "(NullValue starts with 'test') is evaluated to false",
        "Operator_starts_with_3": "$value(NullValue starts with 'test' == false)",

        "Comment_Operator_starts_with_4": "('test' starts with NullValue) is evaluated to false",
        "Operator_starts_with_4": "$value('test' starts with NullValue == false)",

        "Operator_starts_with_5": "$value('Text123' starts with 'Text')",
        "Operator_starts_with_6": "$value('Text123' starts with Text1)",
        "Operator_starts_with_7": "$value(Text1 starts with 'Text1')",
        "Operator_starts_with_8": "$value(Text12 starts with Text1)",
        "Operator_starts_with_9": "$value(Text1 starts with Text12 == false)",

        "Comment_Operator_starts_with_10": "Text matching is case sensitive",
        "Operator_starts_with_10": "$value('Text1' starts with 'text' == false)"
      },

      "Operator_ends_with": {
        "Comment_Operator_ends_with_1": "(invalid.path ends with 'test') is evaluated to false",
        "Operator_ends_with_1": "$value(invalid.path ends with 'test' == false)",

        "Comment_Operator_ends_with_2": "('test' ends with invalid.path) is evaluated to false",
        "Operator_ends_with_2": "$value('test' ends with invalid.path == false)",

        "Comment_Operator_ends_with_3": "(NullValue ends with 'test') is evaluated to false",
        "Operator_ends_with_3": "$value(NullValue ends with 'test' == false)",

        "Comment_Operator_ends_with_4": "('test' ends with NullValue) is evaluated to false",
        "Operator_ends_with_4": "$value('test' ends with NullValue == false)",

        "Operator_ends_with_5": "$value('Text123' ends with '123')",
        "Operator_ends_with_6": "$value('Text12' ends with Text_ext12)",
        "Operator_ends_with_7": "$value(Text1 ends with 'Text1')",
        "Operator_ends_with_8": "$value(Text12 ends with Text_ext12)",
        "Operator_ends_with_9": "$value(Text1 ends with Text12 == false)",

        "Comment_Operator_ends_with_10": "Text matching is case sensitive",
        "Operator_starts_ends_10": "$value('Text1' ends with 'Ext1' == false)"
      },
      "Operator_contains_with": {
        "Comment_Operator_contains_1": "(invalid.path contains 'test') is evaluated to false",
        "Operator_contains_1": "$value(invalid.path contains 'test' == false)",

        "Comment_Operator_contains_2": "('test' contains invalid.path) is evaluated to false",
        "Operator_contains_2": "$value('test' contains invalid.path == false)",

        "Comment_Operator_contains_3": "(NullValue contains 'test') is evaluated to false",
        "Operator_contains_3": "$value(NullValue contains 'test' == false)",

        "Comment_Operator_contains_4": "('test' contains NullValue) is evaluated to false",
        "Operator_contains_4": "$value('test' contains NullValue == false)",

        "Operator_contains_5": "$value('Text123' contains 'ext12')",
        "Operator_contains_6": "$value('Text12' contains Text1)",
        "Operator_contains_7": "$value(Text1 contains 'ext')",
        "Operator_contains_8": "$value(Text12 contains Text1)",
        "Operator_contains_9": "$value(Text1 contains Text12 == false)",

        "Comment_Operator_contains_10": "Text matching is case sensitive",
        "Operator_contains_10": "$value('Text1' contains 'Ext1' == false)"
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
            "Operator_starts_with": {
              "Comment_Operator_starts_with_1":  "(invalid.path starts with 'test') is evaluated to false",
              "Operator_starts_with_1":  true,
              "Comment_Operator_starts_with_2":  "('test' starts with invalid.path) is evaluated to false",
              "Operator_starts_with_2":  true,
              "Comment_Operator_starts_with_3":  "(NullValue starts with 'test') is evaluated to false",
              "Operator_starts_with_3":  true,
              "Comment_Operator_starts_with_4":  "('test' starts with NullValue) is evaluated to false",
              "Operator_starts_with_4":  true,
              "Operator_starts_with_5":  true,
              "Operator_starts_with_6":  true,
              "Operator_starts_with_7":  true,
              "Operator_starts_with_8":  true,
              "Operator_starts_with_9":  true,
              "Comment_Operator_starts_with_10":  "Text matching is case sensitive",
              "Operator_starts_with_10":  true
            },
            "Operator_ends_with": {
              "Comment_Operator_ends_with_1":  "(invalid.path ends with 'test') is evaluated to false",
              "Operator_ends_with_1":  true,
              "Comment_Operator_ends_with_2":  "('test' ends with invalid.path) is evaluated to false",
              "Operator_ends_with_2":  true,
              "Comment_Operator_ends_with_3":  "(NullValue ends with 'test') is evaluated to false",
              "Operator_ends_with_3":  true,
              "Comment_Operator_ends_with_4":  "('test' ends with NullValue) is evaluated to false",
              "Operator_ends_with_4":  true,
              "Operator_ends_with_5":  true,
              "Operator_ends_with_6":  true,
              "Operator_ends_with_7":  true,
              "Operator_ends_with_8":  true,
              "Operator_ends_with_9":  true,
              "Comment_Operator_ends_with_10":  "Text matching is case sensitive",
              "Operator_starts_ends_10":  true
            },
            "Operator_contains_with": {
              "Comment_Operator_contains_1":  "(invalid.path contains 'test') is evaluated to false",
              "Operator_contains_1":  true,
              "Comment_Operator_contains_2":  "('test' contains invalid.path) is evaluated to false",
              "Operator_contains_2":  true,
              "Comment_Operator_contains_3":  "(NullValue contains 'test') is evaluated to false",
              "Operator_contains_3":  true,
              "Comment_Operator_contains_4":  "('test' contains NullValue) is evaluated to false",
              "Operator_contains_4":  true,
              "Operator_contains_5":  true,
              "Operator_contains_6":  true,
              "Operator_contains_7":  true,
              "Operator_contains_8":  true,
              "Operator_contains_9":  true,
              "Comment_Operator_contains_10":  "Text matching is case sensitive",
              "Operator_contains_10":  true
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