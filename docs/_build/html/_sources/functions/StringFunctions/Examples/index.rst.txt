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
      "Len": {
        "Len_1": "$value(Companies.Select(c => c.Employees).First(e => Len(e.Address.Street) >= 15))",
        "Len_2": "$value(Len(LenData.Array1[0].Address.Street))",
        "Len_3": "$(Len(LenData.InvalidPath) is undefined) is true"
      },
      "Concatenate": {
        "ConcatenateData_1": "$value(ConcatenateData.Array1.Where(x => HasField(x, 'FirstName')).Select(e => Concatenate(e.FirstName, ' ', e.LastName)))",
        "ConcatenateData_2": "$value(Concatenate('Mr ', ConcatenateData.Array1[0].FirstName, ' ', ConcatenateData.Array1[0].LastName))",
        "ConcatenateData_3": "$(Concatenate('Mr ', ConcatenateData.Array1[1000].FirstName, ' ', ConcatenateData.Array1[0].LastName) is undefined) is true",
        "ConcatenateData_4": "$value(Concatenate(1, ' test', ' ', true, ' ', false))"
      },
      "Lower": {
        "Lower_1": "$value(LowerUpperData.Array1.Where(x => HasField(x, 'FirstName')).Select(e => Lower(e.LastName)))",
        "Lower_2": "$value(Lower(Concatenate('Mr ', LowerUpperData.Array1[0].FirstName, ' ', LowerUpperData.Array1[0].LastName)))",
        "Lower_3": "$(Lower(LowerUpperData.Array1[1000].LastName) is undefined) is true"
      },
      "Upper": {
        "Upper_1": "$value(LowerUpperData.Array1.Where(x => HasField(x, 'FirstName')).Select(e => Upper(e.LastName)))",
        "Upper_2": "$value(Upper(Concatenate('Mr ', LowerUpperData.Array1[0].FirstName, ' ', LowerUpperData.Array1[0].LastName)))",
        "Upper_3": "$(Upper(LowerUpperData.Array1[1000].LastName) is undefined) is true"
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
            "Len": {
              "Len_1": {
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
              "Len_2":  14,
              "Len_3":  "true is true"
            },
            "Concatenate": {
              "ConcatenateData_1": [
                "Christopher Garcia",
                "David Martinez"
              ],
              "ConcatenateData_2":  "Mr Christopher Garcia",
              "ConcatenateData_3":  "true is true",
              "ConcatenateData_4":  "1 test True False"
            },
            "Lower": {
              "Lower_1": [
                "garcia",
                "martinez"
              ],
              "Lower_2":  "mr christopher garcia",
              "Lower_3":  "true is true"
            },
            "Upper": {
              "Upper_1": [
                "GARCIA",
                "MARTINEZ"
              ],
              "Upper_2":  "MR CHRISTOPHER GARCIA",
              "Upper_3":  "true is true"
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