:orphan:

===========
Result.json
===========

.. sourcecode:: json
  
  {
    "$type": "JsonQL.Query.ObjectQueryResult`1[[System.Collections.Generic.IReadOnlyList`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib]], JsonQL",
    "Value": {
      "$type": "System.Collections.Generic.List`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib",
      "$values": [
        {
          "$type": "JsonQL.Demos.Examples.DataModels.Manager, JsonQL.Demos",
          "Employees": {
            "$type": "System.Collections.Generic.List`1[[JsonQL.Demos.Examples.DataModels.IEmployee, JsonQL.Demos]], System.Private.CoreLib",
            "$values": [
              {
                "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
                "Id": 100000005,
                "FirstName": "Christopher",
                "LastName": "Garcia",
                "Address": {
                  "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
                  "Street": "654 Cedar Road",
                  "City": "Phoenix",
                  "State": "AZ",
                  "ZipCode": "85001",
                  "County": null
                },
                "Salary": 111000,
                "Age": 29,
                "Manager": null,
                "Phones": {
                  "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
                  "$values": [
                    "525-000-0001"
                  ]
                }
              },
              {
                "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
                "Id": 100000007,
                "FirstName": "David",
                "LastName": "Martinez",
                "Address": {
                  "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
                  "Street": "147 Birch Street",
                  "City": "San Antonio",
                  "State": "TX",
                  "ZipCode": "78201",
                  "County": null
                },
                "Salary": 95000,
                "Age": 46,
                "Manager": null,
                "Phones": {
                  "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
                  "$values": [
                    "515-100-0001"
                  ]
                }
              }
            ]
          },
          "Id": 100000006,
          "FirstName": "Sarah",
          "LastName": "Wilson",
          "Address": {
            "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
            "Street": "987 Ash Boulevard",
            "City": "Philadelphia",
            "State": "PA",
            "ZipCode": "19101",
            "County": null
          },
          "Salary": 160000,
          "Age": 35,
          "Manager": null,
          "Phones": {
            "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
            "$values": [
              "632-111-1112",
              "632-111-1113"
            ]
          }
        },
        {
          "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
          "Id": 100000007,
          "FirstName": "David",
          "LastName": "Martinez",
          "Address": {
            "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
            "Street": "147 Birch Street",
            "City": "San Antonio",
            "State": "TX",
            "ZipCode": "78201",
            "County": null
          },
          "Salary": 95000,
          "Age": 46,
          "Manager": null,
          "Phones": {
            "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
            "$values": [
              "515-100-0001"
            ]
          }
        },
        {
          "$type": "JsonQL.Demos.Examples.DataModels.Employee, JsonQL.Demos",
          "Id": 100000008,
          "FirstName": "Emma",
          "LastName": "Johnson",
          "Address": {
            "$type": "JsonQL.Demos.Examples.DataModels.Address, JsonQL.Demos",
            "Street": "58 Maple Avenue",
            "City": "Austin",
            "State": "TX",
            "ZipCode": "73301",
            "County": null
          },
          "Salary": 88000,
          "Age": 32,
          "Manager": null,
          "Phones": {
            "$type": "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib]], System.Private.CoreLib",
            "$values": [
              "512-200-0002"
            ]
          }
        }
      ]
    },
    "ErrorsAndWarnings": {
      "$type": "JsonQL.Query.QueryResultErrorsAndWarnings, JsonQL",
      "CompilationErrors": {
        "$type": "JsonQL.Compilation.ICompilationErrorItem[], JsonQL",
        "$values": []
      },
      "ConversionErrors": {
        "$type": "JsonQL.JsonToObjectConversion.ConversionErrors, JsonQL",
        "Errors": {
          "$type": "System.Collections.Generic.List`1[[JsonQL.JsonToObjectConversion.IConversionError, JsonQL]], System.Private.CoreLib",
          "$values": []
        }
      },
      "ConversionWarnings": {
        "$type": "JsonQL.JsonToObjectConversion.ConversionErrors, JsonQL",
        "Errors": {
          "$type": "System.Collections.Generic.List`1[[JsonQL.JsonToObjectConversion.IConversionError, JsonQL]], System.Private.CoreLib",
          "$values": []
        }
      }
    }
  }

