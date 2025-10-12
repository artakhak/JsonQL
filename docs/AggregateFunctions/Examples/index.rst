========
Examples
========

.. contents::
   :local:
   :depth: 2
   

Examples in **Examples.json** file below demonstrate aggregate functions described in :doc:`../index`

.. note:: The following JSON files are referenced in JsonQL expressions in **Examples.json** in example below:    
    
    - :doc:`../../../MutatingJsonFiles/SampleFiles/companies`

.. sourcecode:: json

    {
      "All": {
        "Comment_All_1": "Check if salaries of all employees older than 40 is greater than 85000.",
        "All_1": "$value(All(Companies.Select(c => c.Employees).Where(e => e.Age >= 40), e => e.Salary > 85000))",

        "Comment_All_2": "Another way to check if salaries of all employees older than 40 is greater than 85000.",
        "All_2": "$value(All(Companies.Select(c => c.Employees), e => e.Age < 40 || e.Salary > 85000))",

        "Comment_All_3": "Demo of using named parameters to make the intent clear.",
        "All_3": "$value(All(collection -> Companies.Select(c => c.Employees), criteria -> e => e.Age < 40 || e.Salary > 85000))"
      },
      "Any": {
        "Comment_Any_1": "Check if salary of any employee older than 40 is greater than 144000.",
        "Any_1": "$value(Any(Companies.Select(c => c.Employees).Where(e => e.Age >= 40), e => e.Salary > 144000))",

        "Comment_Any_2": "Another way to check check if salary of any employee older than 40 is greater than 144000.",
        "Any_2": "$value(Any(Companies.Select(c => c.Employees), e => e.Age < 40 || e.Salary > 144000))",

        "Comment_Any_3": "Check if collection has any item. Optional 'criteria' parameter is not used.",
        "Any_3": "$value(Any(Companies.Select(c => c.Employees)))",

        "Comment_Any_4": "Demo of using named parameters to make the intent clear.",
        "Any_4": "$value(Any(collection -> Companies.Select(c => c.Employees), criteria -> e => e.Age < 40 || e.Salary > 144000))"
      },
      "Count": {
        "Comment_Count_1": "Retrieve count of all employees older than 40 with salary greater than 100000.",
        "Count_1": "$value(Count(Companies.Select(c => c.Employees.Where(e => e.Age >= 40 && e.Salary > 100000))))",

        "Comment_Count_2": "Another way to Retrieve count of all employees older than 40 with salary greater than 100000.",
        "Count_2": "$value(Count(Companies.Select(c => c.Employees).Where(e => e.Age >= 40), e => e.Salary > 100000))",

        "Comment_Count_3": "Demo of using named parameters to make the intent clear.",
        "Count_3": "$value(Count(collection -> Companies.Select(c => c.Employees), criteria -> e => e.Age >= 40 && e.Salary > 100000))"
      },
      "Min": {
        "Comment_Min_1": "Retrieve minimum salary of employees older than 40.",
        "Min_1": "$value(Min(Companies.Select(c => c.Employees.Where(e => e.Age >= 40)).Select(e => e.Salary)))",

        "Comment_Min_2": "Another way to retrieve minimum salary of employees older than 40.",
        "Min_2": "$value(Min(Companies.Select(c => c.Employees), e => e.Age >= 40, e => e.Salary))",

        "Comment_Min_3": "The value evaluated for the minimum of collection items is undefined.",
        "Min_3": "$value(Min(collection -> Companies.Select(c => c.Employees), criteria -> e => e.Age >= 200, value -> e => e.Salary) is undefined)",

        "Comment_Min_4": "Demo of using named parameters to make the intent clear.",
        "Min_4": "$value(Min(collection -> Companies.Select(c => c.Employees), criteria -> e => e.Age >= 40, value -> e => e.Salary))"
      },
      "Max": {
        "Comment_Max_1": "Retrieve maximum salary of employees older than 40.",
        "Max_1": "$value(Max(Companies.Select(c => c.Employees.Where(e => e.Age >= 40)).Select(e => e.Salary)))",

        "Comment_Max_2": "Another way to retrieve maximum salary of employees older than 40.",
        "Max_2": "$value(Max(Companies.Select(c => c.Employees), e => e.Age >= 40, e => e.Salary))",

        "Comment_Max_3": "The value evaluated for the maximum of collection items is undefined.",
        "Max_3": "$value(Max(collection -> Companies.Select(c => c.Employees), criteria -> e => e.Age >= 200, value -> e => e.Salary) is undefined)",

        "Comment_Max_4": "Demo of using named parameters to make the intent clear.",
        "Max_4": "$value(Max(collection -> Companies.Select(c => c.Employees), criteria -> e => e.Age >= 40, value -> e => e.Salary))"
      },
      "Average": {
        "Comment_Average_1": "Retrieve average salary of employees older than 40.",
        "Average_1": "$value(Average(Companies.Select(c => c.Employees.Where(e => e.Age >= 40)).Select(e => e.Salary)))",

        "Comment_Average_2": "Another way to retrieve average salary of employees older than 40.",
        "Average_2": "$value(Average(Companies.Select(c => c.Employees), e => e.Age >= 40, e => e.Salary))",

        "Comment_Average_3": "The value evaluated for the average of collection items is undefined.",
        "Average_3": "$value(Average(collection -> Companies.Select(c => c.Employees), criteria -> e => e.Age >= 200, value -> e => e.Salary) is undefined)",

        "Comment_Average_4": "Demo of using named parameters to make the intent clear.",
        "Average_4": "$value(Average(collection -> Companies.Select(c => c.Employees), criteria -> e => e.Age >= 40, value -> e => e.Salary))"
      },
      "Sum": {
        "Comment_Sum_1": "Retrieve sum of all salaries of employees older than 40.",
        "Sum_1": "$value(Sum(Companies.Select(c => c.Employees.Where(e => e.Age >= 40)).Select(e => e.Salary)))",

        "Comment_Sum_2": "Another way to retrieve sum of all salaries of employees older than 40.",
        "Sum_2": "$value(Sum(Companies.Select(c => c.Employees), e => e.Age >= 40, e => e.Salary))",

        "Comment_Sum_3": "The value evaluated for the sum of collection items is undefined.",
        "Sum_3": "$value(Sum(collection -> Companies.Select(c => c.Employees), criteria -> e => e.Age >= 200, value -> e => e.Salary) is undefined)",

        "Comment_Sum_4": "Demo of using named parameters to make the intent clear.",
        "Sum_4": "$value(Sum(collection -> Companies.Select(c => c.Employees), criteria -> e => e.Age >= 40, value -> e => e.Salary))"
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
            "All": {
              "Comment_All_1":  "Check if salaries of all employees older than 40 is greater than 85000.",
              "All_1":  true,
              "Comment_All_2":  "Another way to check if salaries of all employees older than 40 is greater than 85000.",
              "All_2":  true,
              "Comment_All_3":  "Demo of using named parameters to make the intent clear.",
              "All_3":  true
            },
            "Any": {
              "Comment_Any_1":  "Check if salary of any employee older than 40 is greater than 144000.",
              "Any_1":  true,
              "Comment_Any_2":  "Another way to check check if salary of any employee older than 40 is greater than 144000.",
              "Any_2":  true,
              "Comment_Any_3":  "Check if collection has any item. Optional 'criteria' parameter is not used.",
              "Any_3":  true,
              "Comment_Any_4":  "Demo of using named parameters to make the intent clear.",
              "Any_4":  true
            },
            "Count": {
              "Comment_Count_1":  "Retrieve count of all employees older than 40 with salary greater than 100000.",
              "Count_1":  2,
              "Comment_Count_2":  "Another way to Retrieve count of all employees older than 40 with salary greater than 100000.",
              "Count_2":  2,
              "Comment_Count_3":  "Demo of using named parameters to make the intent clear.",
              "Count_3":  2
            },
            "Min": {
              "Comment_Min_1":  "Retrieve minimum salary of employees older than 40.",
              "Min_1":  88000,
              "Comment_Min_2":  "Another way to retrieve minimum salary of employees older than 40.",
              "Min_2":  88000,
              "Comment_Min_3":  "The value evaluated for the minimum of collection items is undefined.",
              "Min_3":  true,
              "Comment_Min_4":  "Demo of using named parameters to make the intent clear.",
              "Min_4":  88000
            },
            "Max": {
              "Comment_Max_1":  "Retrieve maximum salary of employees older than 40.",
              "Max_1":  144186,
              "Comment_Max_2":  "Another way to retrieve maximum salary of employees older than 40.",
              "Max_2":  144186,
              "Comment_Max_3":  "The value evaluated for the maximum of collection items is undefined.",
              "Max_3":  true,
              "Comment_Max_4":  "Demo of using named parameters to make the intent clear.",
              "Max_4":  144186
            },
            "Average": {
              "Comment_Average_1":  "Retrieve average salary of employees older than 40.",
              "Average_1":  104297.28571428571,
              "Comment_Average_2":  "Another way to retrieve average salary of employees older than 40.",
              "Average_2":  104297.28571428571,
              "Comment_Average_3":  "The value evaluated for the average of collection items is undefined.",
              "Average_3":  true,
              "Comment_Average_4":  "Demo of using named parameters to make the intent clear.",
              "Average_4":  104297.28571428571
            },
            "Sum": {
              "Comment_Sum_1":  "Retrieve sum of all salaries of employees older than 40.",
              "Sum_1":  730081,
              "Comment_Sum_2":  "Another way to retrieve sum of all salaries of employees older than 40.",
              "Sum_2":  730081,
              "Comment_Sum_3":  "The value evaluated for the sum of collection items is undefined.",
              "Sum_3":  true,
              "Comment_Sum_4":  "Demo of using named parameters to make the intent clear.",
              "Sum_4":  730081
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
            
    var companiesJsonTextData = new JsonTextData("Companies",
        LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath)/*, dataJsonTextData*/);

    var result = jsonCompiler.Compile(new JsonTextData("Examples",
        this.LoadExampleJsonFile("Examples.json"), companiesJsonTextData));