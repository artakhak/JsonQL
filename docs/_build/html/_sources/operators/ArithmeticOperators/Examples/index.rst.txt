========
Examples
========

.. contents::
   :local:
   :depth: 2
   

Examples in **Examples.json** file below demonstrate boolean functions described in :doc:`../index`

.. note:: The following JSON files are referenced in JsonQL expressions in **Examples.json** in example below:
    
    - :doc:`data`
    - :doc:`../../../MutatingJsonFiles/SampleFiles/companies`


.. sourcecode:: json

    {
      "Operator_+": {
        "Operator_+_1": "$value((15 + text1) is undefined)",
        "Comment_Operator_+_2": "(e.SalaryInvalid + 10 > 0) is evaluated to false for all employees",
        "Operator_+_2": "$value(Companies.Select(c => c.Employees.Where(e => e.SalaryInvalid + 10 > 0)))",
        "Operator_+_3": "$value(35 + Int1)",
        "Operator_+_4": "$value(Int1 + Int2)",
        "Operator_+_5": "$value(Double1 + Double2)",
        "Operator_+_6": "$value(Int1 + Double1)",
        "Operator_+_7": "$value(NegativeInt1 + Double1)"
      },
      "Operator_-": {
        "Operator_-_1": "$value((15 - text1) is undefined)",
        "Comment_Operator_-_2": "(e.SalaryInvalid - 10 > 0) is evaluated to false for all employees",
        "Operator_-_2": "$value(Companies.Select(c => c.Employees.Where(e => e.SalaryInvalid - 10 > 0)))",
        "Operator_-_3": "$value(35 - Int1)",
        "Operator_-_4": "$value(Int1 - Int2)",
        "Operator_-_5": "$value(Double1 - Double2)",
        "Operator_-_6": "$value(Int1 - Double1)",
        "Operator_-_7": "$value(NegativeInt1 - Double1)"
      },
      "Operator_*": {
        "Operator_*_1": "$value((15 * text1) is undefined)",
        "Comment_Operator_*_2": "(e.SalaryInvalid * 1.1 > 0) is evaluated to false for all employees",
        "Operator_*_2": "$value(Companies.Select(c => c.Employees.Where(e => e.SalaryInvalid * 1.1 > 0)))",
        "Operator_*_3": "$value(35 * Int1)",
        "Operator_*_4": "$value(Int1 * Int2)",
        "Operator_*_5": "$value(Double1 * Double2)",
        "Operator_*_6": "$value(Int1 * Double1)",
        "Operator_*_7": "$value(NegativeInt1 * Double1)"
      },
      "Operator_/": {
        "Operator_/_1": "$value((15 / text1) is undefined)",
        "Comment_Operator_/_2": "(e.SalaryInvalid / 1.1 > 0) is evaluated to false for all employees",
        "Operator_/_2": "$value(Companies.Select(c => c.Employees.Where(e => e.SalaryInvalid / 1.1 > 0)))",
        "Operator_/_3": "$value(35 / Int1)",
        "Operator_/_4": "$value(Int1 / Int2)",
        "Operator_/_5": "$value(Double1 / Double2)",
        "Operator_/_6": "$value(Int1 / Double1)",
        "Operator_/_7": "$value(NegativeInt1 / Double1)"
      },
      "Operator_%": {   
        "Operator_%_1": "$value((15 % text1) is undefined)",
        "Comment_Operator_%_2": "(e.SalaryInvalid % 2 > 0) is evaluated to false for all employees",
        "Operator_%_2": "$value(Companies.Select(c => c.Employees.Where(e => e.SalaryInvalid % 2 > 0)))",

        "Operator_%_3": "$value(15 % Int1)",
        "Operator_%_4": "$value(15 % 3)",
        "Operator_%_5": "$value(17 % 3)",
        "Operator_%_6": "$value(Companies.Where(c => Count(c.Employees) % 2 == 1).Select(c => c.CompanyData.Name))"
      },
      "UnaryOperator_-": {
        "UnaryOperator_-_1": "$value((-text1) is undefined)",
        "Comment_UnaryOperator_-_2": "(e.SalaryInvalid % 2 > 0) is evaluated to false for all employees",
        "UnaryOperator_-_2": "$value(Companies.Select(c => c.Employees.Where(e => -e.SalaryInvalid < -100000)))",

        "UnaryOperator_-_3": "$value(-1 + 17)",
        "UnaryOperator_-4": "$value(-Max(Companies.Select(c => c.Employees.Select(e => e.Salary))) + 100)"
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
            "Operator_+": {
              "Operator_+_1":  true,
              "Comment_Operator_+_2":  "(e.SalaryInvalid + 10 > 0) is evaluated to false for all employees",
              "Operator_+_2": [
              ],
              "Operator_+_3":  50,
              "Operator_+_4":  40,
              "Operator_+_5":  40.5,
              "Operator_+_6":  30.25,
              "Operator_+_7":  0.25
            },
            "Operator_-": {
              "Operator_-_1":  true,
              "Comment_Operator_-_2":  "(e.SalaryInvalid - 10 > 0) is evaluated to false for all employees",
              "Operator_-_2": [
              ],
              "Operator_-_3":  20,
              "Operator_-_4":  -10,
              "Operator_-_5":  -10,
              "Operator_-_6":  -0.25,
              "Operator_-_7":  -30.25
            },
            "Operator_*": {
              "Operator_*_1":  true,
              "Comment_Operator_*_2":  "(e.SalaryInvalid * 1.1 > 0) is evaluated to false for all employees",
              "Operator_*_2": [
              ],
              "Operator_*_3":  525,
              "Operator_*_4":  375,
              "Operator_*_5":  385.0625,
              "Operator_*_6":  228.75,
              "Operator_*_7":  -228.75
            },
            "Operator_/": {
              "Operator_/_1":  true,
              "Comment_Operator_/_2":  "(e.SalaryInvalid / 1.1 > 0) is evaluated to false for all employees",
              "Operator_/_2": [
              ],
              "Operator_/_3":  2.3333333333333335,
              "Operator_/_4":  0.6,
              "Operator_/_5":  0.6039603960396039,
              "Operator_/_6":  0.9836065573770492,
              "Operator_/_7":  -0.9836065573770492
            },
            "Operator_%": {
              "Operator_%_1":  true,
              "Comment_Operator_%_2":  "(e.SalaryInvalid % 2 > 0) is evaluated to false for all employees",
              "Operator_%_2": [
              ],
              "Operator_%_3":  0,
              "Operator_%_4":  0,
              "Operator_%_5":  2,
              "Operator_%_6": [
                "Sherwood Forest Timber, Inc",
                "Atlantic Transfers, Inc"
              ]
            },
            "UnaryOperator_-": {
              "UnaryOperator_-_1":  true,
              "Comment_UnaryOperator_-_2":  "(e.SalaryInvalid % 2 > 0) is evaluated to false for all employees",
              "UnaryOperator_-_2": [
              ],
              "UnaryOperator_-_3":  16,
              "UnaryOperator_-4":  -144086
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