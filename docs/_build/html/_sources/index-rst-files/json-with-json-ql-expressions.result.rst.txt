:orphan:

===========
Result.json
===========

.. sourcecode:: json
  
  {
    "CompiledJsonFiles":[
      {
        "TextIdentifier": "Overview",
        "CompiledParsedValue":
        {
          "CountryNamesWithPopulationOf80MlnOrMore": [
            "United States",
            "Germany",
            "India"
          ],
          "CompanyDetailsWithAnyEmployeeWithNullAddress": [
            {
              "Name":  "Sherwood Forest Timber, Inc",
              "CEO":  "Robin Wood",
              "Address": {
                "Street":  "789 Pine Lane",
                "City":  "Denver",
                "State":  "CO",
                "ZipCode":  "80203"
              },
              "CountryDetails": [
                {
                  "Name":  "Germany",
                  "Population":  83000000,
                  "Area":  357022,
                  "GDP":  3.86
                }
              ]
            }
          ],
          "EmployeesWithNonEmptyLoginsList": [
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
            }
          ],
          "FirstEmployeeWithLoginsField": {
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
          "LastEmployeeWithLoginsField": {
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
          "FlattenExamples": {
            "FlattenArraysAndFilterObjects": [
              {
                "Name":  "John"
              },
              7,
              8,
              9,
              {
                "CompanyName":  "Sherwood Forest Timber, Inc",
                "Address": {
                  "Street":  "789 Pine Lane",
                  "City":  "Denver",
                  "State":  "CO",
                  "ZipCode":  "80203"
                }
              },
              10
            ],
            "GetAllNumericValuesInArraysFiltered": [
              1,
              3,
              4,
              5,
              6,
              7,
              8,
              9,
              10,
              14,
              15
            ]
          },
          "SelectSecondCompanyCeoNameWithAssertionThatCeoNameFieldIsPresetAndNotNull":  "Robin Wood",
          "EnhancedCompanyData": {
            "Capitalization":  1000000,
            "Name":  "Sherwood Forest Timber, Inc",
            "CEO":  "Robin Wood",
            "Address": {
              "Street":  "789 Pine Lane",
              "City":  "Denver",
              "State":  "CO",
              "ZipCode":  "80203"
            },
            "CountryDetails": [
              {
                "Name":  "Germany",
                "Population":  83000000,
                "Area":  357022,
                "GDP":  3.86
              }
            ],
            "NumberOfEmployees":  "3"
          },
          "ListOfCompaniesEnhancedWithNewCompanies": [
            {
              "Name":  "Some company 1",
              "CEO":  "Napoleon Bonaparte"
            },
            {
              "Name":  "Sherwood Forest Timber, Inc",
              "CEO":  "Robin Wood",
              "Address": {
                "Street":  "789 Pine Lane",
                "City":  "Denver",
                "State":  "CO",
                "ZipCode":  "80203"
              },
              "CountryDetails": [
                {
                  "Name":  "Germany",
                  "Population":  83000000,
                  "Area":  357022,
                  "GDP":  3.86
                }
              ]
            },
            {
              "Name":  "Atlantic Transfers, Inc",
              "CEO":  "Black Beard",
              "Address": {
                "Street":  "101 Elm Drive",
                "City":  "Dallas",
                "State":  "TX",
                "ZipCode":  "75201"
              },
              "CountryDetails": [
                {
                  "Name":  "United States",
                  "Population":  331000000,
                  "Area":  9833520,
                  "GDP":  21.43
                }
              ]
            },
            {
              "Name":  "Some company 2",
              "CEO":  "Georges Danton"
            }
          ],
          "AverageSalaryOfAllEmployeesInFilteredCompanies":  "104920.11111111111",
          "AddressingObjectsInMultiDimensionalMatrix": {
            "Street":  "789 Pine Lane",
            "City":  "Denver",
            "State":  "CO",
            "ZipCode":  "80203"
          },
          "EmployeeNamesInSecondCompanyExcludingFirstAndLastEmployee": [
            "Sarah Wilson"
          ],
          "MaxRaisedSalaryAccrossAllCompaniesUsingNamedParameter":  151395.30000000002,
          "ThirdEmployeeFromEndWithSalaryGreaterThan_100000_AcrossAllCompanies": {
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
          "UseOf_this_ToEnsureObjectIsLookedUpInCurrentFileAndNotInParentFiles":  "Germany",
          "UseOf_parent_ToEnsureObjectIsLookedUpInParentFilesAndSearchInCurrentFilesIsSkipped":  2,
          "EmployeeWithNonNullIdAndNullAddress": [
            {
              "Id":  100000006,
              "Name":  "Sarah Wilson",
              "Address":  null,
              "Salary":  78000,
              "Age":  35
            }
          ],
          "NumberOfEmployeesWithout_Logins_Field":  "10",
          "NumberOfEmployeesWith_Logins_Field":  "2",
          "InvalidValuesEvaluateToUndefined":  "true = true",
          "ConversionExample_Datetime":  "'2022-05-22T18:25:43.511Z' < '2022-05-23T18:25:43.511Z'=true",
          "DefaultValue":  "Defaulted salary to 100000=100000"
        }
      }
    ],
    "CompilationErrors":
    {
      "$type": "System.Collections.Generic.List`1[[JsonQL.Compilation.ICompilationErrorItem, JsonQL]], System.Private.CoreLib",
      "$values": []
    }
  }

