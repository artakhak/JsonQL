========
'assert'
========

.. contents::
   :local:
   :depth: 2
   
The `assert` unary operator is a postfix operator used in JsonQL to validate that a value is not null or undefined. When applied to an expression, it ensures the expression evaluates to a non-null and defined value. If the assertion fails (i.e., the value is null or undefined), the compilation stops with an assertion error.

**Operator Priority**: 300

Syntax
======

<expression> assert

The `assert` operator is placed after the expression it validates.

Operator Operands
=================

- **Operand 1**:    
    - Type: any valid JsonQL expression (including invalid path).
    - Description: The expression whose result should be validated as non-null and defined

Evaluation Rules
================

The `assert` operator follows these evaluation rules:
1. **Successful Assertion**: If the operand evaluates to a non-null, defined value, the assertion succeeds and returns the original value (i.e., numeric, string, boolean, DateTime or Date value).
2. **Failed Assertion**: If the operand evaluates to `null` or is undefined (e.g., a missing JSON property), the compilation fails with the error message: `"Value not-null assertion failed"`
3. **Propagates Errors**: If the operand expression contains evaluation errors, those errors are propagated without adding assertion errors
4. **Type Preservation**: The `assert` operator preserves the type of the operand (boolean, string, double, DateTime, etc.)

Use Cases
=========

The `assert` operator is particularly useful for:
- **Validating Required Fields**: Ensuring that required JSON properties exist and are not null
- **Data Validation**: Enforcing data integrity constraints during compilation
- **Filtering with Guarantees**: Using in `Where` clauses to ensure all filtered items have required properties
- **Debugging**: Identifying missing or null values early in the compilation process

Implementation Details
======================

The `assert` operator is implemented through a family of specialized classes:
    - `AssertOperatorFunction <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/JsonFunction/JsonFunctions/AssertFunctions/AssertOperatorFunction.cs>`_: Generic implementation for any JSON function
    - `AssertOperatorBooleanFunction <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/JsonFunction/JsonFunctions/AssertFunctions/AssertOperatorBooleanFunction.cs>`_: Specialized for boolean expressions
    - `AssertOperatorStringFunction <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/JsonFunction/JsonFunctions/AssertFunctions/AssertOperatorStringFunction.cs>`_: Specialized for string expressions
    - `AssertOperatorDoubleFunction <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/JsonFunction/JsonFunctions/AssertFunctions/AssertOperatorDoubleFunction.cs>`_: Specialized for numeric expressions
    - `AssertOperatorDateTimeFunction <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/JsonFunction/JsonFunctions/AssertFunctions/AssertOperatorDateTimeFunction.cs>`_: Specialized for DateTime expressions

Best Practices
==============

- **Use for Required Data**: Apply `assert` to fields that must exist for your logic to work correctly
- **Early Validation**: Place assertions early in expression chains to catch issues before complex processing
- **Avoid Over-Use**: Don't assert every value; use only where null/undefined would indicate a data problem
- **Combine with Comparisons**: The `assert` operator works seamlessly with comparison operators (`==`, `>=`, etc.)

Notes
=====

- The `assert` operator causes compilation to stop immediately on the first assertion failure
- It provides stronger guarantees than null-coalescing or conditional operators by failing fast
- The operator is evaluated as part of the expression compilation phase, not at runtime

Examples
========

**Examples.json** file below demonstrate using `assert` operator

.. note:: The following JSON files are referenced in JsonQL expressions in **Examples.json** in example below:
    
    - :doc:`Examples/data`


.. sourcecode:: json

    {
      "Assert_Succeeds_1": "$value(Int1 assert == 15)",
      "Assert_Succeeds_2": "$value(Text1 assert starts with 'Text')",

      "Comments_Assert_Succeeds_3": "'e.Age assert' succeeds for employees since 'Age' is not null or undefined for all employees",
      "Assert_Succeeds_3": "$value(Employees.Where(e => e.Age assert >= 40))",

      "Comments_No_Assert_1": "Some values e.Salary are null or undefined (keys are missing).",
      "Comments_No_Assert_2": "'e.Salary >= 100000' will be false for all employees with missing or null Salary value",
      "No_Assert": "$value(Employees.Where(e => e.Salary >= 100000))",

      "Comments_AssertFails_1": "Some values e.Salary are null or undefined (keys are missing).",
      "Comments_AssertFails_2": "Check 'e.Salary assert >= 1000000' will fail the compilation (compilation will stop with assertion errors)",
      "Comments_AssertFails_3": "on first employee with missing or null value for 'Salary' (e.g., employee with Id=100000001 which has no 'Salary' key)",
      "AssertFails": "$value(Employees.Where(e => e.Salary assert >= 100000))"
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
            "Assert_Succeeds_1":  true,
            "Assert_Succeeds_2":  true,
            "Comments_Assert_Succeeds_3":  "'e.Age assert' succeeds for employees since 'Age' is not null or undefined for all employees",
            "Assert_Succeeds_3": [
              {
                "Id":  100000001,
                "Name":  "John Smith",
                "Address": {
                  "Street":  "456 Oak Avenue",
                  "City":  "Chicago",
                  "State":  "IL",
                  "ZipCode":  "60601"
                },
                "Age":  45
              },
              {
                "Id":  100000003,
                "Name":  "Michael Brown",
                "Address": {
                  "Street":  "789 Pine Lane",
                  "City":  "Los Angeles",
                  "State":  "CA",
                  "ZipCode":  "90001"
                },
                "Salary":  105000,
                "Age":  50
              },
              {
                "Id":  100000004,
                "Name":  "Emily Davis",
                "Address": {
                  "Street":  "321 Elm Drive",
                  "City":  "Houston",
                  "State":  "TX",
                  "ZipCode":  "77001"
                },
                "Salary":  92000,
                "Age":  42
              }
            ],
            "Comments_No_Assert_1":  "Some values e.Salary are null or undefined (keys are missing).",
            "Comments_No_Assert_2":  "'e.Salary >= 100000' will be false for all employees with missing or null Salary value",
            "No_Assert": [
              {
                "Id":  100000003,
                "Name":  "Michael Brown",
                "Address": {
                  "Street":  "789 Pine Lane",
                  "City":  "Los Angeles",
                  "State":  "CA",
                  "ZipCode":  "90001"
                },
                "Salary":  105000,
                "Age":  50
              }
            ],
            "Comments_AssertFails_1":  "Some values e.Salary are null or undefined (keys are missing).",
            "Comments_AssertFails_2":  "Check 'e.Salary assert >= 1000000' will fail the compilation (compilation will stop with assertion errors)",
            "Comments_AssertFails_3":  "on first employee with missing or null value for 'Salary' (e.g., employee with Id=100000001 which has no 'Salary' key)",
            "AssertFails":  "$value(Employees.Where(e => e.Salary assert >= 100000))"
          }
        }
      ],
      "CompilationErrors":
      {
        "$type": "System.Collections.Generic.List`1[[JsonQL.Compilation.ICompilationErrorItem, JsonQL]], System.Private.CoreLib",
        "$values": [
          {
            "$type": "JsonQL.Compilation.CompilationErrorItem, JsonQL",
            "JsonTextIdentifier": "Examples",
            "LineInfo": {
              "$type": "JsonQL.JsonObjects.JsonLineInfo, JsonQL",
              "LineNumber": 15,
              "LinePosition": 47
            },
            "ErrorMessage": "Value not-null assertion failed"
          }
        ]
      }
    }

.. raw:: html

   </details><br/><br/>
   
The screenshot below shows the error details logged using the error data in `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationResult.cs>`_ serialized to **Result.json** above.

 .. image:: Examples/assert-error-logs.jpg

   
The code snippet shows how the JSON file **Examples.json** was parsed using `JsonQL.Compilation.IJsonCompiler <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/IJsonCompiler.cs>`_

.. sourcecode:: csharp

    // Set the value of jsonCompiler to an instance of JsonQL.Compilation.IJsonCompiler here.
    // The value of JsonQL.Compilation.JsonCompiler is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.
    JsonQL.Compilation.IJsonCompiler jsonCompiler = null!;

    var dataJsonTextData = new JsonTextData("Data", this.LoadExampleJsonFile("Data.json"));       
    var result = jsonCompiler.Compile(new JsonTextData("Examples", this.LoadExampleJsonFile("Examples.json"), dataJsonTextData));
