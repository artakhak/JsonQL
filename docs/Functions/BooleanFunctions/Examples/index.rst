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
      "HasField": {
        "HasField_1": "$value(HasFieldData.Array1.Where(x => HasField(x, 'Id')))",
        "HasField_2": "$value(HasFieldData.Array1.Where(x => HasField(x.Address, 'Street')))",
        "HasField_3": "$(HasField(HasFieldData.InvalidPath, 'Street')) is false"
      },
      "IsEven": {
        "IsEven_1": "$value(IsEven(Count(Companies.Select(c => c.Employees))))",
        "IsEven_2": "$value(IsEvenIsOddData.Array1.Where(x => IsEven(x)))",
        "IsEven_3": "$(IsEven(IsEvenIsOddData.InvalidPath) is undefined) is true"
      },
      "IsOdd": {
        "IsOdd_1": "$value(IsOdd(Count(Companies.Select(c => c.Employees))))",
        "IsOdd_2": "$value(IsEvenIsOddData.Array1.Where(x => IsOdd(x)))",
        "IsOdd_3": "$(IsOdd(IsEvenIsOddData.InvalidPath) is undefined) is true"
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
            "HasField": {
              "HasField_1": [
                {
                  "Id":  1,
                  "Salary":  101000,
                  "Address": {
                    "Street":  "654 Cedar Road",
                    "City":  "Phoenix",
                    "State":  "AZ",
                    "ZipCode":  "85001"
                  }
                }
              ],
              "HasField_2": [
                {
                  "Id":  1,
                  "Salary":  101000,
                  "Address": {
                    "Street":  "654 Cedar Road",
                    "City":  "Phoenix",
                    "State":  "AZ",
                    "ZipCode":  "85001"
                  }
                }
              ],
              "HasField_3":  "false is false"
            },
            "IsEven": {
              "IsEven_1":  true,
              "IsEven_2": [
                4,
                2,
                12
              ],
              "IsEven_3":  "true is true"
            },
            "IsOdd": {
              "IsOdd_1":  false,
              "IsOdd_2": [
                1,
                3
              ],
              "IsOdd_3":  "true is true"
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