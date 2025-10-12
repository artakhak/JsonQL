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
      "Operator_&&": {
        "Comment_Operator_&&_1": "(invalid.path && Int1==15) is evaluated to false",
        "Operator_&&_1": "$value((invalid.path && Int1==15) == false)",

        "Comment_Operator_&&_2": "(true && invalid.path) is evaluated to false",
        "Operator_&&_2": "$value((true && invalid.path) == false)",

        "Comment_Operator_&&_3": "(NullValue && Int1==15) is evaluated to false",
        "Operator_&&_3": "$value((NullValue && Int1==15) == false)",

        "Comment_Operator_&&_4": "(Int1==15 && NullValue) is evaluated to false",
        "Operator_&&_4": "$value((Int1==15 && NullValue) == false)",

        "Comment_Operator_&&_5": "(e.InvalidBooleanPath && 1 == 1) is evaluated to false for all employees",
        "Operator_&&_5": "$value(Count(Companies.Select(c => c.Employees.Where(e => e.InvalidBooleanPath && 1 == 1))) == 0)",

        "Operator_&&_6": "$value(Companies.Select(c => c.Employees.Where(e => e.Age > 40 && e.Salary > 100000)).First())",

        "Operator_&&_7": "$value((TrueValue && Int1==15) == true)",
        "Operator_&&_8": "$value((FalseValue && Int1==15) == false)",
        "Operator_&&_9": "$value((Int1==15 && FalseValue) == false)",
        "Operator_&&_10": "$value((Int1==16 && FalseValue) == false)"
      },
      "Operator_||": {
        "Comment_Operator_||_1": "(invalid.path || Int1==15) is evaluated to true",
        "Operator_||_1": "$value(invalid.path || Int1==15)",

        "Comment_Operator_||_2": "(true || invalid.path) is evaluated to true",
        "Operator_||_2": "$value(true || invalid.path)",

        "Comment_Operator_||_3": "(NullValue || Int1==15) is evaluated to true",
        "Operator_||_3": "$value(NullValue || Int1==15)",

        "Comment_Operator_||_4": "(Int1==15 || NullValue) is evaluated to true",
        "Operator_||_4": "$value(Int1==15 || NullValue)",

        "Comment_Operator_||_5": "(e.InvalidBooleanPath && 1 == 1) is evaluated to true for all employees",
        "Operator_||_5": "$value(Count(Companies.Select(c => c.Employees.Where(e => e.InvalidBooleanPath || 1 == 1))) == 12)",

        "Operator_||_6": "$value(Companies.Select(c => c.Employees.Where(e => e.Age > 40 || e.Salary > 100000)).First())",

        "Operator_||_7": "$value(TrueValue || Int1==15)",
        "Operator_||_8": "$value(FalseValue || Int1==15)",
        "Operator_||_9": "$value(Int1==15 || FalseValue)",
        "Operator_||_10": "$value((Int1==16 || FalseValue) == false)"
      },
      "Operator_!": {
        "Comment_Operator_!_1": "(!invalid.path) is evaluated to true",
        "Operator_!_1": "$value(!invalid.path)",

        "Comment_Operator_!_2": "(!NullValue) is evaluated to true",
        "Operator_!_2": "$value(!NullValue)",

        "Comment_Operator_!_3": "(!Int1) is evaluated to true. Any value other than boolean false is evaluated to true",
        "Operator_!_3": "$value(!Int1)",

        "Operator_!_4": "$value(!(Int1 == 15) == false)",
        "Operator_!_5": "$value(!TrueValue == false)",
        "Operator_!_6": "$value(!FalseValue)",

        "Operator_!_7": "$value(Companies.Select(c => c.Employees.Where(e => !(e.Age > 40 || e.Salary > 100000))).First())"
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
            "Operator_&&": {
              "Comment_Operator_&&_1":  "(invalid.path && Int1==15) is evaluated to false",
              "Operator_&&_1":  true,
              "Comment_Operator_&&_2":  "(true && invalid.path) is evaluated to false",
              "Operator_&&_2":  true,
              "Comment_Operator_&&_3":  "(NullValue && Int1==15) is evaluated to false",
              "Operator_&&_3":  true,
              "Comment_Operator_&&_4":  "(Int1==15 && NullValue) is evaluated to false",
              "Operator_&&_4":  true,
              "Comment_Operator_&&_5":  "(e.InvalidBooleanPath && 1 == 1) is evaluated to false for all employees",
              "Operator_&&_5":  true,
              "Operator_&&_6": {
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
              "Operator_&&_7":  true,
              "Operator_&&_8":  true,
              "Operator_&&_9":  true,
              "Operator_&&_10":  true
            },
            "Operator_||": {
              "Comment_Operator_||_1":  "(invalid.path || Int1==15) is evaluated to true",
              "Operator_||_1":  true,
              "Comment_Operator_||_2":  "(true || invalid.path) is evaluated to true",
              "Operator_||_2":  true,
              "Comment_Operator_||_3":  "(NullValue || Int1==15) is evaluated to true",
              "Operator_||_3":  true,
              "Comment_Operator_||_4":  "(Int1==15 || NullValue) is evaluated to true",
              "Operator_||_4":  true,
              "Comment_Operator_||_5":  "(e.InvalidBooleanPath && 1 == 1) is evaluated to true for all employees",
              "Operator_||_5":  true,
              "Operator_||_6": {
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
              "Operator_||_7":  true,
              "Operator_||_8":  true,
              "Operator_||_9":  true,
              "Operator_||_10":  true
            },
            "Operator_!": {
              "Comment_Operator_!_1":  "(!invalid.path) is evaluated to true",
              "Operator_!_1":  true,
              "Comment_Operator_!_2":  "(!NullValue) is evaluated to true",
              "Operator_!_2":  true,
              "Comment_Operator_!_3":  "(!Int1) is evaluated to true. Any value other than boolean false is evaluated to true",
              "Operator_!_3":  true,
              "Operator_!_4":  true,
              "Operator_!_5":  true,
              "Operator_!_6":  true,
              "Operator_!_7": {
                "Id":  100000006,
                "Name":  "Sarah Wilson",
                "Address":  null,
                "Salary":  78000,
                "Age":  35
              }
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