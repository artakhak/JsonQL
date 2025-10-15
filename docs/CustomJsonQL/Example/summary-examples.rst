================
Summary Examples
================

.. contents::
   :local:
   :depth: 4
   
**CustomFeaturesOverview.json** file below demonstrates using custom functions, operators, and Json path functions.

.. note:: The following JSON files are referenced in JsonQL expressions in **Examples.json** in example below:   
   
    - :doc:`companies`

**CustomFeaturesOverview.json**:

.. code-block:: json

    {
      "TestData": [ 1, 5, 8, 17, "Test" ],

      "CustomBinaryOperator_+-_1": "(TestData[0] +- TestData[2])=$(TestData[0] +- TestData[2])==-9",
      "CustomBinaryOperator_+-_2": "(-TestData[0] +- -TestData[2])=$(-TestData[0] +- -TestData[2])==9",
      "CustomPrefixOperator_++2": "(++2 TestData[2])=$(++2 TestData[2])==10",
      "CustomPostfixOperator_is_even": "(TestData[2] is even)=$(TestData[2] is even)==true",

      "CustomFunction_ReverseTextAndAddMarkers_1": "ReverseTextAndAddMarkers(TestData[4])='$(ReverseTextAndAddMarkers(TestData[4]))' == '#tseT#'",
      "CustomFunction_ReverseTextAndAddMarkers_2": "'ReverseTextAndAddMarkers(TestData[4], true)=$(ReverseTextAndAddMarkers(TestData[4], true))' == '#tseT#'",
      "CustomFunction_ReverseTextAndAddMarkers_3": "ReverseTextAndAddMarkers(TestData[4], false)='$(ReverseTextAndAddMarkers(TestData[4], false))' == 'tseT'",
      "CustomFunction_ReverseTextAndAddMarkers_4": "ReverseTextAndAddMarkers(value->TestData[4], addMarkers->true)='$(ReverseTextAndAddMarkers(value->TestData[4], addMarkers->true))' == '#tseT#'",
      "CustomFunction_ReverseTextAndAddMarkers_5": "ReverseTextAndAddMarkers(addMarkers->false, value->TestData[4])='$(ReverseTextAndAddMarkers(addMarkers->false, value->TestData[4]))' == 'tseT'",

      "CustomSpecialFunction_JsonQLReleaseDate": "JsonQLReleaseDate is '$(JsonQLReleaseDate)'. The type of value of 'JsonQLReleaseDate' function is '$(typeof JsonQLReleaseDate)'.",
      "CustomMultipleItemsSelectorExample_SelectEvenIndexes": "$value(Companies.Select(x => x.Employees).Flatten().SelectEvenIndexes(x => x.Id != 100000001).Where(x => x.Salary > 89000))",
      "CustomSingleItemSelectorExample_SecondItemSelector": "Second employee name with salary less than 100K is '$(Companies[0].Employees.Second(x => x.Salary < 100000).Name)'=='Michael Brown'"
    }

    
The result (i.e., an instance of `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationResult.cs>`_) is serialized to a **Result.json** file below.

.. note::
    For brevity, the serialized result includes only serialized evaluated **CustomFeaturesOverview.json** and does not include parent JSON files in **JsonQL.Compilation.ICompilationResult.CompiledJsonFiles**

.. raw:: html

   <details>
   <summary>Click to expand the result in instance of <b>JsonQL.Compilation.ICompilationResult</b> serialized into <b>Result.json</b></summary>

.. code-block:: json

    {
      "CompiledJsonFiles":[
        {
          "TextIdentifier": "CustomFeaturesOverview",
          "CompiledParsedValue":
          {
            "TestData": [
              1,
              5,
              8,
              17,
              "Test"
            ],
            "CustomBinaryOperator_+-_1":  "(TestData[0] +- TestData[2])=-9==-9",
            "CustomBinaryOperator_+-_2":  "(-TestData[0] +- -TestData[2])=9==9",
            "CustomPrefixOperator_++2":  "(++2 TestData[2])=10==10",
            "CustomPostfixOperator_is_even":  "(TestData[2] is even)=true==true",
            "CustomFunction_ReverseTextAndAddMarkers_1":  "ReverseTextAndAddMarkers(TestData[4])='#tseT#' == '#tseT#'",
            "CustomFunction_ReverseTextAndAddMarkers_2":  "'ReverseTextAndAddMarkers(TestData[4], true)=#tseT#' == '#tseT#'",
            "CustomFunction_ReverseTextAndAddMarkers_3":  "ReverseTextAndAddMarkers(TestData[4], false)='tseT' == 'tseT'",
            "CustomFunction_ReverseTextAndAddMarkers_4":  "ReverseTextAndAddMarkers(value->TestData[4], addMarkers->true)='#tseT#' == '#tseT#'",
            "CustomFunction_ReverseTextAndAddMarkers_5":  "ReverseTextAndAddMarkers(addMarkers->false, value->TestData[4])='tseT' == 'tseT'",
            "CustomSpecialFunction_JsonQLReleaseDate":  "JsonQLReleaseDate is '2025-06-01 00:00:00.0000000'. The type of value of 'JsonQLReleaseDate' function is 'DateTime'.",
            "CustomMultipleItemsSelectorExample_SelectEvenIndexes": [
              {
                "Id":  100000005,
                "Name":  "Christopher Garcia",
                "Address": {
                  "Street":  "654 Cedar Road",
                  "City":  "Phoenix",
                  "State":  "AZ",
                  "ZipCode":  "85001"
                },
                "Salary":  111000,
                "Age":  29,
                "Logins": [
                  "cgarcia@sherwood.com",
                  "cgarcia@gmail.com"
                ]
              },
              {
                "Id":  100000007,
                "Name":  "David Martinez",
                "Address": {
                  "Street":  "147 Birch Street",
                  "City":  "San Antonio",
                  "State":  "TX",
                  "ZipCode":  "78201"
                },
                "Salary":  95000,
                "Age":  46,
                "Logins": [
                  "dmartinez@sherwood.com",
                  "dmartinez@gmail.com"
                ]
              },
              {
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
              }
            ],
            "CustomSingleItemSelectorExample_SecondItemSelector":  "Second employee name with salary less than 100K is 'Michael Brown'=='Michael Brown'"
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