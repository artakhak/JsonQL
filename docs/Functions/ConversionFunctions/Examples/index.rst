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
      "ToBoolean": {
        "ToBoolean_1": "$(ToBoolean(value -> ToBooleanData.InvalidBooleanText) is undefined) is true",
        "ToBoolean_2": "$(ToBoolean(ToBooleanData.TrueText1))",
        "ToBoolean_3": "$(ToBoolean('false', throwOnError -> true))",
        "ToBoolean_4": "$(ToBoolean('False', throwOnError -> true))"
      },
      "ToDateTime": {
        "ToDateTime_1": "$(ToDateTime(value -> ToDateTimeData.InvalidDateTimeText) is undefined) is true",
        "ToDateTime_2": "$value(ToDateTime(ToDateTimeData.DateTimeText1, throwOnError -> true))",
        "ToDateTime_3": "$value(ToDateTime('2022-05-23', throwOnError -> true))",
        "ToDateTime_4": "$value(ToDateTime('2022-05-23T18:25:43.511Z') < ToDateTime('2025-05-23T18:25:43.511Z'))"
      },
      "ToDate": {
        "ToDate_1": "$(ToDate(value -> ToDateData.InvalidDateTimeText) is undefined) is true",
        "ToDate_2": "$value(ToDate(ToDateData.DateTimeText1, throwOnError -> true))",
        "ToDate_3": "$value(ToDate(ToDateData.DateText1, throwOnError -> true))",
        "ToDate_4": "$value(ToDate('2022-05-23', throwOnError -> true))",
        "ToDate_5": "$value(ToDate('2022-05-23T18:25:43.511Z') < ToDate('2025-05-23T18:25:43.511Z'))",
        "ToDate_6": "$value(ToDate(ToDateData.DateTimeText1) < ToDateTime(ToDateData.DateTimeText1))"
      },
      "ToDouble": {
        "ToDouble_1": "$(ToDouble(value -> ToDoubleData.InvalidDoubleText) is undefined) is true",

        "ToDouble_2": "$value(ToDouble(ToDoubleData.DoubleText1, throwOnError -> true) == 1.36)",
        "ToDouble_3": "$value(ToDouble(ToDoubleData.NegativeDoubleText1, throwOnError -> true) == -1.36)",

        "ToDouble_4": "$value(ToDouble(ToDoubleData.IntText1))",
        "ToDouble_5": "$value(ToDouble(ToDoubleData.NegativeIntText1))",

        "ToDouble_6": "$value(ToDouble(ToDoubleData.Double1))",
        "ToDouble_7": "$value(ToDouble(ToDoubleData.NegativeDouble1))",

        "ToDouble_8": "$value(ToDouble(ToDoubleData.Int1))",
        "ToDouble_9": "$value(ToDouble(ToDoubleData.NegativeInt1))"
      },
      "ToInt": {
        "ToInt_1": "$(ToInt(value -> ToIntData.InvalidIntText) is undefined) is true",

        "ToInt_2": "$value(ToInt(ToIntData.DoubleText1, throwOnError -> true) == 17)",
        "ToInt_3": "$value(ToInt(ToIntData.NegativeDoubleText1, throwOnError -> true) == -17)",

        "ToInt_4": "$value(ToInt(ToIntData.IntText1))",
        "ToInt_5": "$value(ToInt(ToIntData.NegativeIntText1))",

        "ToInt_6": "$value(ToInt(ToIntData.Double1))",
        "ToInt_7": "$value(ToInt(ToIntData.NegativeDouble1))",

        "ToInt_8": "$value(ToInt(ToIntData.Int1))",
        "ToInt_9": "$value(ToInt(ToIntData.NegativeInt1))"
      },

      "ToString": {
        "ToString_1": "$(ToString(value -> InvalidPath) is undefined) is true",

        "ToString_2": "$value(ToString(ToStringData.Int1, throwOnError -> true) == '15')",
        "ToString_3": "$value(ToString(ToStringData.NegativeInt1, throwOnError -> true))",

        "ToString_4": "$value(ToString(ToStringData.Double1))",
        "ToString_5": "$value(ToString(ToStringData.NegativeDouble1))",

        "ToString_6": "$value(ToString(ToStringData.Text1))",

        "ToString_7": "$value(ToString(ToStringData.TrueValue))",
        "ToString_8": "'$(ToString(false))' == 'false'",

        "ToString_9": "$(ToString(ToDateTime('2022-05-23T18:25:43.511Z'))=='2022-05-23 14:25:43.5110000') is true"
      }
    }
    
The result (i.e., an instance of `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationResult.cs>`_) is serialized to a **Result.json** file below.

.. note::
    For brevity, the serialized result includes only serialized evaluated **Examples.json** and does not include parent JSON files in **JsonQL.Compilation.ICompilationResult.CompiledJsonFiles**

.. raw:: html

   <details>
   <summary>Click to expand the result in instance of `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationResult.cs>`_ serialized into <b>Result.json</b></summary>

.. code-block:: json

    {
      "CompiledJsonFiles":[
        {
          "TextIdentifier": "Examples",
          "CompiledParsedValue":
          {
            "ToBoolean": {
              "ToBoolean_1":  "true is true",
              "ToBoolean_2":  "true",
              "ToBoolean_3":  "false",
              "ToBoolean_4":  "false"
            },
            "ToDateTime": {
              "ToDateTime_1":  "true is true",
              "ToDateTime_2":  "2022-05-23 14:25:43.5110000",
              "ToDateTime_3":  "2022-05-23 00:00:00.0000000",
              "ToDateTime_4":  true
            },
            "ToDate": {
              "ToDate_1":  "true is true",
              "ToDate_2":  "2022-05-23 00:00:00.0000000",
              "ToDate_3":  "2022-05-23 00:00:00.0000000",
              "ToDate_4":  "2022-05-23 00:00:00.0000000",
              "ToDate_5":  true,
              "ToDate_6":  true
            },
            "ToDouble": {
              "ToDouble_1":  "true is true",
              "ToDouble_2":  true,
              "ToDouble_3":  true,
              "ToDouble_4":  15,
              "ToDouble_5":  -15,
              "ToDouble_6":  1.36,
              "ToDouble_7":  -1.36,
              "ToDouble_8":  15,
              "ToDouble_9":  -15
            },
            "ToInt": {
              "ToInt_1":  "true is true",
              "ToInt_2":  true,
              "ToInt_3":  true,
              "ToInt_4":  17,
              "ToInt_5":  -17,
              "ToInt_6":  17,
              "ToInt_7":  -17,
              "ToInt_8":  17,
              "ToInt_9":  -17
            },
            "ToString": {
              "ToString_1":  "true is true",
              "ToString_2":  true,
              "ToString_3":  "-15",
              "ToString_4":  "15.36",
              "ToString_5":  "-15.36",
              "ToString_6":  "Text 1",
              "ToString_7":  "true",
              "ToString_8":  "'false' == 'false'",
              "ToString_9":  "true is true"
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