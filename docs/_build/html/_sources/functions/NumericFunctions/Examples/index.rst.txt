========
Examples
========

.. contents::
   :local:
   :depth: 2
   

Examples in **Examples.json** file below demonstrate numeric functions described in :doc:`../index`

.. note:: The following JSON files are referenced in JsonQL expressions in **Examples.json** in example below:
    
    - :doc:`data`
    - :doc:`../../../MutatingJsonFiles/SampleFiles/companies`


.. sourcecode:: json

    {
      "Abs": {
        "Abs_1": "$value(AbsData.Array1.Where(x => typeof x == 'Number').Select(x => Abs(x)))",
        "Abs_2": "$value(AbsData.Array1.Where(x => Abs(x) > 3))",
        "Abs_3": "$(Abs(AbsData.Invalid) is undefined) is true"
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
            "Abs": {
              "Abs_1": [
                1,
                4,
                2,
                4,
                3,
                12
              ],
              "Abs_2": [
                -4,
                4,
                12
              ],
              "Abs_3":  "true is true"
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