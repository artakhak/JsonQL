========
Examples
========

.. contents::
   :local:
   :depth: 2
   

Examples in **Examples.json** file below demonstrate comparison operators described in :doc:`../index`

.. note:: The following JSON files are referenced in JsonQL expressions in **Examples.json** in example below:
    
    - :doc:`data`
    - :doc:`../../../MutatingJsonFiles/SampleFiles/companies`


.. sourcecode:: json

    {
      "Operator_==": {
        "Comment_Operator_==_1": "(15 == invalid.path) is evaluated to false",
        "Operator_==_1": "$value((15 == invalid.path) == false)",
        "Comment_Operator_==_2": "(e.SalaryInvalid == 100000) is evaluated to false for all employees",
        "Operator_==_2": "$value(Count(Companies.Select(c => c.Employees.Where(e => e.SalaryInvalid == 100000))) == 0)",

        "Operator_==_3": "$value(15 == Int1)",
        "Operator_==_4": "$value((15 == '15') == false)",

        "Operator_==_5": "$value(15.00 == Int1)",
        "Operator_==_6": "$value((15.01 == Int1) == false)",

        "Operator_==_7": "$value(TrueValue == true)",
        "Operator_==_8": "$value((TrueValue == 'true') == false)",

        "Operator_==_9": "$value(FalseValue == false)",
        "Operator_==_10": "$value((FalseValue == 'false') == false)",

        "Operator_==_11": "$value(ToDate('2022-05-23') == ToDate('2022-05-23T18:25:43.511Z'))",
        "Operator_==_12": "$value((ToDate('2022-05-23') == ToDate('2022-06-23T18:25:43.511Z')) == false)",

        "Operator_==_13": "$value(Text1 == 'Text1')",
        "Operator_==_14": "$value(Text1 == 'TExt1' == false)"
      },
      "Operator_!=": {
        "Comment_Operator_!=_1": "(15 != invalid.path) is evaluated to true",
        "Operator_!=_1": "$value((15 != invalid.path) == true)",
        "Comment_Operator_!=_2": "(e.SalaryInvalid != 100000) is evaluated to true for all employees",
        "Operator_!=_2": "$value(Count(Companies.Select(c => c.Employees.Where(e => e.SalaryInvalid != 100000))) > 0)",

        "Operator_!=_3": "$value((15 != Int1) == false)",
        "Operator_!=_4": "$value((15 != '15') == true)",

        "Operator_!=_5": "$value((15.00 != Int1) == false)",
        "Operator_!=_6": "$value(15.01 != Int1)",

        "Operator_!=_7": "$value(TrueValue != true == false)",
        "Operator_!=_8": "$value((TrueValue != 'true'))",

        "Operator_!=_9": "$value((FalseValue != false) == false)",
        "Operator_!=_10": "$value(FalseValue != 'false')",

        "Operator_!=_11": "$value((ToDate('2022-05-23') != ToDate('2022-05-23T18:25:43.511Z')) == false)",
        "Operator_!=_12": "$value(ToDate('2022-05-23') != ToDate('2022-06-23T18:25:43.511Z'))",

        "Operator_!=_13": "$value(Text1 != 'TExt1')",
        "Operator_!=_14": "$value(Text1 !='Text1' == false)"
      },
      "Operator_<": {
        "Comment_Operator_<_1": "(15 < invalid.path) is evaluated to undefined",
        "Operator_<_1": "$value((15 < invalid.path) is undefined)",
        "Comment_Operator_<_2": "(e.SalaryInvalid < 100000) is evaluated to undefined for all employees",
        "Operator_<_2": "$value(Count(Companies.Select(c => c.Employees.Where(e => e.SalaryInvalid < 100000))) == 0)",

        "Operator_<_3": "$value(Int1 < 16)",
        "Operator_<_4": "$value((Int1 < 15) == false)",

        "Operator_<_5": "$value(Int1 < 16.00)",
        "Operator_<_6": "$value((Int1 < 14.9) == false)",

        "Operator_<_7": "$value(Double1 < 16)",
        "Operator_<_8": "$value((Double2 < Double1) == false)",

        "Operator_<_9": "$value(ToDate('2022-05-23T18:25:43.511Z') < ToDate('2022-06-23T18:25:43.511Z'))",
        "Operator_<_10": "$value(ToDate('2022-06-23T18:25:43.511Z') < ToDate('2022-05-23T18:25:43.511Z') == false)",

        "Operator_<_11": "$value(ToDateTime('2022-05-23T18:25:43.511Z') < ToDate('2022-06-23T18:25:43.511Z'))",
        "Operator_<_12": "$value(ToDateTime('2022-06-23T18:25:43.511Z') < ToDateTime('2022-05-23T18:25:43.511Z') == false)",

        "Operator_<_13": "$value(ToDate('2022-06-23T18:25:43.511Z') < ToDateTime('2022-06-23T18:25:43.511Z'))",
        "Operator_<_14": "$value(ToDateTime('2022-05-23T18:25:43.511Z') < ToDate('2022-06-23T18:25:43.511Z'))",

        "Operator_<_15": "$value(Text1 < 'Text2')",
        "Operator_<_16": "$value(Text1 < 'Text0' == false)"
      },
      "Operator_<=": {
        "Comment_Operator_<=_1": "(15 <= invalid.path) is evaluated to undefined",
        "Operator_<=_1": "$value((15 <= invalid.path) is undefined)",
        "Comment_Operator_<=_2": "(e.SalaryInvalid <= 100000) is evaluated to undefined for all employees",
        "Operator_<=_2": "$value(Count(Companies.Select(c => c.Employees.Where(e => e.SalaryInvalid <= 100000))) == 0)",

        "Operator_<=_3": "$value(Int1 <= 15)",
        "Operator_<=_4": "$value(Int1 <= 16)",
        "Operator_<=_5": "$value((Int1 <= 14) == false)",

        "Operator_<=_6": "$value(Int1 <= 15.00)",
        "Operator_<=_7": "$value(Int1 <= 16.00)",
        "Operator_<=_8": "$value((Int1 <= 14.9) == false)",

        "Operator_<=_9": "$value(Double1 <= 15.25)",
        "Operator_<=_10": "$value(Double1 <= 15.26)",
        "Operator_<=_11": "$value((Double1 <= 15.24) == false)",

        "Operator_<=_12": "$value(ToDate('2022-05-23T18:25:43.511Z') <= ToDate('2022-05-23T18:25:43.511Z'))",
        "Operator_<=_13": "$value(ToDate('2022-05-23T18:25:43.511Z') <= ToDate('2022-06-23T18:25:43.511Z'))",
        "Operator_<=_14": "$value(ToDate('2022-06-23T18:25:43.511Z') <= ToDate('2022-05-23T18:25:43.511Z') == false)",

        "Operator_<=_15": "$value(ToDateTime('2022-05-23T18:25:43.511Z') <= ToDateTime('2022-05-23T18:25:43.511Z'))",
        "Operator_<=_16": "$value(ToDateTime('2022-05-23T18:25:43.511Z') <= ToDate('2022-06-23T18:25:43.511Z'))",
        "Operator_<=_17": "$value(ToDateTime('2022-06-23T18:25:43.511Z') <= ToDateTime('2022-05-23T18:25:43.511Z') == false)",

        "Operator_<=_18": "$value(ToDate('2022-06-23T18:25:43.511Z') <= ToDateTime('2022-06-23T18:25:43.511Z'))",
        "Operator_<=_19": "$value(ToDateTime('2022-05-23T18:25:43.511Z') <= ToDate('2022-06-23T18:25:43.511Z'))",

        "Operator_<=_20": "$value(Text1 <= 'Text1')",
        "Operator_<=_21": "$value(Text1 <= 'Text2')",
        "Operator_<=_22": "$value(Text1 <= 'Text0' == false)"
      },
      "Operator_>": {
        "Comment_Operator_>_1": "(15 > invalid.path) is evaluated to undefined",
        "Operator_>_1": "$value((15 > invalid.path) is undefined)",
        "Comment_Operator_>_2": "(e.SalaryInvalid > 100000) is evaluated to undefined for all employees",
        "Operator_>_2": "$value(Count(Companies.Select(c => c.Employees.Where(e => e.SalaryInvalid > 100000))) == 0)",

        "Operator_>_3": "$value(Int1 > 14)",
        "Operator_>_4": "$value((Int1 > 15) == false)",

        "Operator_>_5": "$value(Int1 > 14.00)",
        "Operator_>_6": "$value((Int1 > 15.1) == false)",

        "Operator_>_7": "$value(Double1 > 15.20)",
        "Operator_>_8": "$value((Double1 > Double2) == false)",

        "Operator_>_9": "$value(ToDate('2022-06-23T18:25:43.511Z') > ToDate('2022-05-23T18:25:43.511Z'))",
        "Operator_>_10": "$value(ToDate('2022-06-23T18:25:43.511Z') > ToDate('2022-07-23T18:25:43.511Z') == false)",

        "Operator_>_11": "$value(ToDateTime('2022-06-23T18:25:43.511Z') > ToDate('2022-05-23T18:25:43.511Z'))",
        "Operator_>_12": "$value(ToDateTime('2022-05-23T18:25:43.511Z') > ToDateTime('2022-06-23T18:25:43.511Z') == false)",

        "Operator_>_13": "$value(ToDate('2022-06-23T18:25:43.511Z') > ToDateTime('2022-05-23T18:25:43.511Z'))",
        "Operator_>_14": "$value(ToDateTime('2022-06-23T18:25:43.511Z') > ToDate('2022-06-23T18:25:43.511Z'))",

        "Operator_>_15": "$value(Text1 > 'Text0')",
        "Operator_>_16": "$value(Text1 > 'Text2' == false)"
      },
      "Operator_>=": {
        "Comment_Operator_>=_1": "(15 >= invalid.path) is evaluated to undefined",
        "Operator_>=_1": "$value((15 >= invalid.path) is undefined)",
        "Comment_Operator_<=_2": "(e.SalaryInvalid >= 100000) is evaluated to undefined for all employees",
        "Operator_>=_2": "$value(Count(Companies.Select(c => c.Employees.Where(e => e.SalaryInvalid >= 100000))) == 0)",

        "Operator_>=_3": "$value(Int1 >= 15)",
        "Operator_>=_4": "$value(Int1 >= 14)",
        "Operator_>=_5": "$value((Int1 >= 16) == false)",

        "Operator_>=_6": "$value(Int1 >= 15.00)",
        "Operator_>=_7": "$value(Int1 >= 14.00)",
        "Operator_>=_8": "$value((Int1 >= 15.1) == false)",

        "Operator_>=_9": "$value(Double1 >= 15.25)",
        "Operator_>=_10": "$value(Double1 >= 15.24)",
        "Operator_>=_11": "$value((Double1 >= 15.26) == false)",

        "Operator_>=_12": "$value(ToDate('2022-05-23T18:25:43.511Z') >= ToDate('2022-05-23T18:25:43.511Z'))",
        "Operator_>=_13": "$value(ToDate('2022-06-23T18:25:43.511Z') >= ToDate('2022-06-23T18:25:43.511Z'))",
        "Operator_>=_14": "$value(ToDate('2022-05-23T18:25:43.511Z') >= ToDate('2022-06-23T18:25:43.511Z') == false)",

        "Operator_>=_15": "$value(ToDateTime('2022-05-23T18:25:43.511Z') >= ToDateTime('2022-05-23T18:25:43.511Z'))",
        "Operator_>=_16": "$value(ToDateTime('2022-06-23T18:25:43.511Z') >= ToDate('2022-05-23T18:25:43.511Z'))",
        "Operator_>=_17": "$value(ToDateTime('2022-05-23T18:25:43.511Z') >= ToDateTime('2022-06-23T18:25:43.511Z') == false)",

        "Operator_>=_18": "$value(ToDate('2022-06-23T18:25:43.511Z') >= ToDateTime('2022-05-23T18:25:43.511Z'))",
        "Operator_>=_19": "$value(ToDateTime('2022-06-23T18:25:43.511Z') >= ToDate('2022-06-23T18:25:43.511Z'))",

        "Operator_>=_20": "$value(Text1 >= 'Text1')",
        "Operator_>=_21": "$value(Text1 >= 'Text0')",
        "Operator_>=_22": "$value(Text1 >= 'Text2' == false)"
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
            "Operator_==": {
              "Comment_Operator_==_1":  "(15 == invalid.path) is evaluated to false",
              "Operator_==_1":  true,
              "Comment_Operator_==_2":  "(e.SalaryInvalid == 100000) is evaluated to false for all employees",
              "Operator_==_2":  true,
              "Operator_==_3":  true,
              "Operator_==_4":  true,
              "Operator_==_5":  true,
              "Operator_==_6":  true,
              "Operator_==_7":  true,
              "Operator_==_8":  true,
              "Operator_==_9":  true,
              "Operator_==_10":  true,
              "Operator_==_11":  true,
              "Operator_==_12":  true,
              "Operator_==_13":  true,
              "Operator_==_14":  true
            },
            "Operator_!=": {
              "Comment_Operator_!=_1":  "(15 != invalid.path) is evaluated to true",
              "Operator_!=_1":  true,
              "Comment_Operator_!=_2":  "(e.SalaryInvalid != 100000) is evaluated to true for all employees",
              "Operator_!=_2":  true,
              "Operator_!=_3":  true,
              "Operator_!=_4":  true,
              "Operator_!=_5":  true,
              "Operator_!=_6":  true,
              "Operator_!=_7":  true,
              "Operator_!=_8":  true,
              "Operator_!=_9":  true,
              "Operator_!=_10":  true,
              "Operator_!=_11":  true,
              "Operator_!=_12":  true,
              "Operator_!=_13":  true,
              "Operator_!=_14":  true
            },
            "Operator_<": {
              "Comment_Operator_<_1":  "(15 < invalid.path) is evaluated to undefined",
              "Operator_<_1":  true,
              "Comment_Operator_<_2":  "(e.SalaryInvalid < 100000) is evaluated to undefined for all employees",
              "Operator_<_2":  true,
              "Operator_<_3":  true,
              "Operator_<_4":  true,
              "Operator_<_5":  true,
              "Operator_<_6":  true,
              "Operator_<_7":  true,
              "Operator_<_8":  true,
              "Operator_<_9":  true,
              "Operator_<_10":  true,
              "Operator_<_11":  true,
              "Operator_<_12":  true,
              "Operator_<_13":  true,
              "Operator_<_14":  true,
              "Operator_<_15":  true,
              "Operator_<_16":  true
            },
            "Operator_<=": {
              "Comment_Operator_<=_1":  "(15 <= invalid.path) is evaluated to undefined",
              "Operator_<=_1":  true,
              "Comment_Operator_<=_2":  "(e.SalaryInvalid <= 100000) is evaluated to undefined for all employees",
              "Operator_<=_2":  true,
              "Operator_<=_3":  true,
              "Operator_<=_4":  true,
              "Operator_<=_5":  true,
              "Operator_<=_6":  true,
              "Operator_<=_7":  true,
              "Operator_<=_8":  true,
              "Operator_<=_9":  true,
              "Operator_<=_10":  true,
              "Operator_<=_11":  true,
              "Operator_<=_12":  true,
              "Operator_<=_13":  true,
              "Operator_<=_14":  true,
              "Operator_<=_15":  true,
              "Operator_<=_16":  true,
              "Operator_<=_17":  true,
              "Operator_<=_18":  true,
              "Operator_<=_19":  true,
              "Operator_<=_20":  true,
              "Operator_<=_21":  true,
              "Operator_<=_22":  true
            },
            "Operator_>": {
              "Comment_Operator_>_1":  "(15 > invalid.path) is evaluated to undefined",
              "Operator_>_1":  true,
              "Comment_Operator_>_2":  "(e.SalaryInvalid > 100000) is evaluated to undefined for all employees",
              "Operator_>_2":  true,
              "Operator_>_3":  true,
              "Operator_>_4":  true,
              "Operator_>_5":  true,
              "Operator_>_6":  true,
              "Operator_>_7":  true,
              "Operator_>_8":  true,
              "Operator_>_9":  true,
              "Operator_>_10":  true,
              "Operator_>_11":  true,
              "Operator_>_12":  true,
              "Operator_>_13":  true,
              "Operator_>_14":  true,
              "Operator_>_15":  true,
              "Operator_>_16":  true
            },
            "Operator_>=": {
              "Comment_Operator_>=_1":  "(15 >= invalid.path) is evaluated to undefined",
              "Operator_>=_1":  true,
              "Comment_Operator_<=_2":  "(e.SalaryInvalid >= 100000) is evaluated to undefined for all employees",
              "Operator_>=_2":  true,
              "Operator_>=_3":  true,
              "Operator_>=_4":  true,
              "Operator_>=_5":  true,
              "Operator_>=_6":  true,
              "Operator_>=_7":  true,
              "Operator_>=_8":  true,
              "Operator_>=_9":  true,
              "Operator_>=_10":  true,
              "Operator_>=_11":  true,
              "Operator_>=_12":  true,
              "Operator_>=_13":  true,
              "Operator_>=_14":  true,
              "Operator_>=_15":  true,
              "Operator_>=_16":  true,
              "Operator_>=_17":  true,
              "Operator_>=_18":  true,
              "Operator_>=_19":  true,
              "Operator_>=_20":  true,
              "Operator_>=_21":  true,
              "Operator_>=_22":  true
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