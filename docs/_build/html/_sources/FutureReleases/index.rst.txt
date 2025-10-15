===============
Future Releases
===============

.. contents::
   :local:
   :depth: 2
   
Number of future improvements are planned. 
The author at the moment works on other projects, but at some point will implement the planned features.

.. note::
    The planned features might be prioritized higher if there is a demand for these features:
    
Some of examples of the planned features are demonstrated in **JsonQL.Tests** projects at this link `JsonQL.Tests.FutureReleases <https://github.com/artakhak/JsonQL/tree/main/JsonQL.Tests/FutureReleases>`_

    .. note::
        The tests project **JsonQL.Tests** has only several features. The rest are in JIRA tickets.

Most important big features are demonstrated here:

JsonQL Expressions on Multiple Lines
====================================

JsonQL expressions can be to large for a single in JSON string. 
To support spreading JsonQL expressions to multiple lines, a JSON object with ``$multiline`` key will be used, with value as array or lines with JsonQL expression as in example below.

.. sourcecode:: json
    
    {
      "MultilineQueryWith_$": {
        "$multiline": [
          "Max salary of all employees in all companies except one", 
          "is $(Max(Companies.Where",
          "(x => x.Name != 'Sherwood Forest Timber, Inc')",
          ".Select(c => c.Employees.Where(e => e.Salary > 100000))",
          ".Select(x => x.Salary)))"      
        ]
      }
    }
    
During compilation, JsonQL will convert the value of key `MultilineQueryWith_$` to a single string that is a concatenation of lines in array `$multiline` and will evaluate JsonQL expression in this text.

For example the JSON in example above is evaluated to a JSON below.

.. sourcecode:: json
    
    {
       "MultilineQueryWith_$":  "Max salary of all employees in all companies except one is 144186"
    }


Complex Projections Using in Select
===================================

Currently JsonQL has a `Select` path function (see :doc:`../../JsonPathFunctions/Select/index`) that can supports projections from one value to another.
However, `Select` currently does not support projections that create new JSON objects or arrays. This type of projections will be supported as part of this feature.

Examples of complex projections are shown in this example (see usage of **Select** JsonQL path function): 

.. sourcecode:: json
    
    {
      "ProjectEmployeesToNewJsonObject": {
        "$multiline":  [
          "$value(Companies.Select(c => c.Employees.Where(e => e.Salary > 100000).Select(e =>",
          "{'Id': e.Id, 'Name': Concatenate(e.FirstName, ' ', e.LastName)",
          "'Salary': e.Salary",
          "'AddressCities': ['NY', Merge(e.Addresses.Select(x => c.City))],",
          "'AddressCitiesAsString': Join(', ', e.Addresses.Select(x => c.City)),",      
          "'Index': index",
          "})))"
        ]
      },
      "ProjectEmployeesToNewJsonObjectAndFilterOnNewProjectedFields": {
        "$multiline":  [
          "$value(Companies.Select(c => c.Employees.Where(e => e.Salary > 100000).Select(e =>",
          "{'Id': e.Id, 'Name': Concatenate(e.FirstName, ' ', e.LastName)",
          "'Salary': e.Salary",
          "'AddressCities': ['NY', Merge(e.Addresses.Select(x => c.City))],",
          "'AddressCitiesAsString': Join(', ', e.Addresses.Select(x => c.City)),",
          "'Index': index",
          "}).Where(e => !Any(e.AddressCities.Where(x => x == 'Phoenix')))))"
        ]
      },
      "ProjectCompanyToDataAndAverageSalary": {
        "$multiline":  [
          "$value(Companies.Where(e => e.CompanyData.Name != 'Sherwood Forest Timber, Inc').",
          "Select(c => {'CompanyName': c.CompanyData.Name, 'CEO': c.CEO,'",
          "'AverageSalary': Average(c.Employees.Select(e => e.Salary))",
          "}))"
        ]
      },
      "ProjectCompanyToDataAndAverageSalaryAndFilterOnNewProjectedFields": {
        "$multiline":  [
          "$value(Companies.Where(e => e.CompanyData.Name != 'Sherwood Forest Timber, Inc').",
          "Select(c => {'CompanyName': c.CompanyData.Name, 'CEO': c.CEO,'",
          "'AverageSalary': Average(c.Employees.Select(e => e.Salary))",
          "}).Where(c => c.AverageSalary > 100000))"
        ]
      },
      "QueryProjectedArray": {
        "Comments": "Merges arrays Companies[0].Employee and Companies[2].Employees and applies a Where filter using ToQueryable.",
        "Query": {
          "$multiline": [
            "$value(ToQueryable([Merge(Companies[0].Employees, Companies[2].Employees)]).Where(e => e.FirstName != 'Robert'))"
          ]
        }
      },
      "CopyFieldsWithFilter": {
        "Comments": "Tests projection using CopyFields with filter.",
        "Query": {
          "$multiline": [
            "$value(Companies.Where(c => c.CompanyData.Name == 'Sherwood Forest Timber, Inc')",
            ".Select(c => c.Employees.Select(e =>",
            "{",
            "'Name': Concatenate(e.FirstName, ' ', e.LastName)",
            "'employeeFields': CopyFields(e, x => x.metadata.name != 'FirstName'",
            "&& x.metadata.name != 'LastName' && !(x.metadata.path match '^Addresses[(\\d+)].Street$')",
            "&& (x.metadata.path != 'Age' || x.value != 29))",
            "})))"
          ]
        }
      },
      "QueryProjectedJson": {
        "Comments": "Projects a JSON to a new JSON object and accesses fields from the new object using ToQueryable() function. The example does not make much sense, but is good enough for testing.",
        "Query": {
          "$multiline": [
            "$value(Companies.Where(c => c.CompanyData.Name == 'Sherwood Forest Timber, Inc')",
            ".Select(c => c.Employees.Select(e =>",
            "ToQueryable({",
            "'Name': Concatenate(e.FirstName, ' ', e.LastName)",       
            "}).Name)))"
          ]
        }
      }
    }


Support for Grouping 
====================

Examples in JSON below demonstrate grouping (see usage of **Group** JsonQL path function):

.. sourcecode:: json
    
    {
      "EmployeesGroupedByCompaniesWithValuesNotFlattened": {
        "Comments1": "The referenced Companies.json file has multiple JSON objects in Companies array with the same CompanyData.Name.",
        "Comments2": "We group employees of companies with similar name.",
        "query": {
          "$multiline": [
            "$value(Companies.Where(c => c.CompanyData.Name != 'Atlantic Transfers, Inc')", 
            ".Group(c => c.CompanyData.Name, c => c.Employees,",
            "(key, value) => {'CompanyName': key, 'CompanyEmployees': value}, flattenValues->false))"        
          ]
        }
      },
      "EmployeesGroupedByCompaniesWithValuesFlattened": {
        "Comments1": "The referenced Companies.json file has multiple JSON objects in Companies array with the same CompanyData.Name.",
        "Comments2": "We group employees of companies with similar name.",
        "query": {
          "$multiline": [
            "$value(Companies.Where(c => c.CompanyData.Name != 'Atlantic Transfers, Inc')",
            ".Group(c => c.CompanyData.Name, c => c.Employees,",
            "(key, value) => {'CompanyName': key, 'CompanyEmployees': value}, flattenValues->true))"
          ]
        }
      },
      "EmployeesGroupedByCompaniesWithCompanyDataUsedAsKey": {
        "Comments1": "The referenced Companies.json file has multiple JSON objects in Companies array with the same CompanyData.Name.",
        "Comments2": "We group employees of companies with similar name.",
        "query": {
          "$multiline": [
            "$value(Companies.Where(c => c.CompanyData.Name != 'Atlantic Transfers, Inc')",
            ".Group(c => c.CompanyData, c => c.Employees,",
            "(key, value) => {'CompanyData': key, 'CompanyEmployees': value}, flattenValues->true))"
          ]
        }
      },
      "CompanyEmployeesStats": {
        "Comments1": "The referenced Companies.json file has multiple JSON objects in Companies array with the same CompanyData.Name.",
        "Comments2": "We group employees of companies with similar name.",
        "query": {
          "$multiline": [
            "$value(Companies.Where(c => c.CompanyData.Name != 'Atlantic Transfers, Inc')",
            ".Group(c => c.CompanyData.Name, c => c.Employees,",
            "(key, value) => {'CompanyName': key,  'EmployeesCount': Count(value),",
            "'MaxSalary': Max(value.Select(e => e.Salary))}, flattenValues -> true))"
          ]
        }
      }
    }


Support for Sorting Using Labda Expressions 
===========================================

Examples in JSON below demonstrate sorting using lambda functions (see usage of **Sort** and **SortDesc** JsonQL path functions):

.. sourcecode:: json
    
    {
      "TestData": {
        "ArrayOfEmployeesAndOffices": [
          {
            "Id": 100000001,
            "Name": "John Smith",
            "Address": {
              "Street": "456 Oak Avenue",
              "City": "Chicago",
              "State": "IL",
              "ZipCode": "60601"
            },
            "Salary": 99500,
            "Age": 45
          },
          { "OfficeName": "Office2" },
          {
            "Id": 100000002,
            "Name": "Alice Johnson",
            "Address": {
              "Street": "123 Maple Street",
              "City": "New York",
              "State": "NY",
              "ZipCode": "10001"
            },
            "Salary": 105000,
            "Age": 38
          },

          { "OfficeName": "Office1" }
        ]
      },
      "AllCompanyEmployeesSortedBySalary": "$value(Companies.Where(x => x.CompanyData.Name == 'Strange Things, Inc' || x.CompanyData.Name == 'Atlantic Transfers, Inc').Select(x => x.Employees.Where(x => x.Salary is not null)).Sort(x => x.Salary))",
      "AllCompanyEmployeesSortedDescBySalary": "$value(Companies.Where(x => x.CompanyData.Name == 'Strange Things, Inc' || x.CompanyData.Name == 'Atlantic Transfers, Inc').Select(x => x.Employees.Where(x => x.Salary is not null)).SortDesc(x => x.Salary))",

      "CompanyNamesSortedByMaxSalary": "$value(Companies.Sort(c => Max(c.Employees.Select(e => e.Salary))).Select(x => x.CompanyData.Name))",
      "CompanyNamesSortedInDescendingOrderOfMaxSalary": "$value(Companies.SortDesc(c => Max(c.Employees.Select(e => e.Salary))).Select(x => x.CompanyData.Name))",

      "AdvancedSort": {
        "Comment": "Sorts offices first, and then employees in TestData.ArrayOfEmployeesAndOffices. Offices are sorted by ascending order of ''OfficeName'' field name. Employees are sorted by descending order of ''Id'' field value.",
        "SortedOfficesAndEmployees": "$value(TestData.ArrayOfEmployeesAndOffices.Sort((x, y) => if (HasField(x, 'OfficeName'), if (HasField(y, 'OfficeName'), x.OfficeName - y.OfficeName, 1), if (HasField(y, 'OfficeName'), 1, y.Id - x.Id))))"
      }
    }

