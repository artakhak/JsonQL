==============
Array Indexers
==============

.. contents::
   :local:
   :depth: 2
   
- Array indexers are used to access items in JSON arrays. 
- Multiple array indexes can be specified to access items in multi-dimensional JSON arrays

Example
=======

.. note:: The following JSON files are referenced in JsonQL expressions in **Example.json** in example below:  :doc:`../../MutatingJsonFiles/SampleFiles/employees`.

**Example.json** below demonstrates using array indexers with JsonQL.


.. sourcecode:: json

    {
      "Object1": {
        "Array1": [
          1,
          [ 3, 4, 7 ],
          {
            "EmployeeId": 11,
            "Age": 35
          },
          [
            [
              59,
              9,
              {
                "EmployeeId": 1,
                "Age": 55
              },
              {
                "EmployeeId": 2,
                "Age": 67
              },
              [ 17, 8, 13 ]
            ],
            19
          ],
          41,
          [ 148, 156 ],
          [
            [ "test", 9, 158 ]
          ]
        ]
      },

      "SingleIndexExample_ReferenceJsonValue": "$(Object1.Array1[0]) is 1",
      "SingleIndexExample_ReferenceJsonArray": "$value(Object1.Array1[1])",
      "SingleIndexExample_ReferenceJsonObject": "$value(Object1.Array1[2])",
      "MultipleIndexesExample_ReferenceJsonValue": "$(Object1.Array1[1, 1]) is 4",
      "MultipleIndexesExample_ReferenceJsonArray": "$value(Object1.Array1[3, 0, 4])",
      "MultipleIndexesExample_ReferenceJsonObject": "$value(Object1.Array1[3, 0, 2])",
      "UsingIndexersInWhere": [
        "$merge(Object1.Array1.Where(x => x[0, 1] == 9))"
      ],
      "ApplyOtherPathFunctionsAfterIndexer_1": "$value(Companies[1].CompanyData.Address.Street)",
      "ApplyOtherPathFunctionsAfterIndexer_2": "$value(Object1.Array1[3, 0, 2].EmployeeId)"
    }
    
The result (i.e., an instance of `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationResult.cs>`_) is serialized to a **Result.json** file below.

.. note::
    For brevity, the serialized result includes only serialized evaluated **Example.json** and does not include parent JSON files in **JsonQL.Compilation.ICompilationResult.CompiledJsonFiles**
 
.. sourcecode:: json

    {
      "CompiledJsonFiles":[
        {
          "TextIdentifier": "Example",
          "CompiledParsedValue":
          {
            "Object1": {
              "Array1": [
                1,
                [
                  3,
                  4,
                  7
                ],
                {
                  "EmployeeId":  11,
                  "Age":  35
                },
                [
                  [
                    59,
                    9,
                    {
                      "EmployeeId":  1,
                      "Age":  55
                    },
                    {
                      "EmployeeId":  2,
                      "Age":  67
                    },
                    [
                      17,
                      8,
                      13
                    ]
                  ],
                  19
                ],
                41,
                [
                  148,
                  156
                ],
                [
                  [
                    "test",
                    9,
                    158
                  ]
                ]
              ]
            },
            "SingleIndexExample_ReferenceJsonValue":  "1 is 1",
            "SingleIndexExample_ReferenceJsonArray": [
              3,
              4,
              7
            ],
            "SingleIndexExample_ReferenceJsonObject": {
              "EmployeeId":  11,
              "Age":  35
            },
            "MultipleIndexesExample_ReferenceJsonValue":  "4 is 4",
            "MultipleIndexesExample_ReferenceJsonArray": [
              17,
              8,
              13
            ],
            "MultipleIndexesExample_ReferenceJsonObject": {
              "EmployeeId":  1,
              "Age":  55
            },
            "UsingIndexersInWhere": [
              [
                [
                  59,
                  9,
                  {
                    "EmployeeId":  1,
                    "Age":  55
                  },
                  {
                    "EmployeeId":  2,
                    "Age":  67
                  },
                  [
                    17,
                    8,
                    13
                  ]
                ],
                19
              ],
              [
                [
                  "test",
                  9,
                  158
                ]
              ]
            ],
            "ApplyOtherPathFunctionsAfterIndexer_1":  "789 Pine Lane",
            "ApplyOtherPathFunctionsAfterIndexer_2":  1
          }
        }
      ],
      "CompilationErrors":
      {
        "$type": "System.Collections.Generic.List`1[[JsonQL.Compilation.ICompilationErrorItem, JsonQL]], System.Private.CoreLib",
        "$values": []
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