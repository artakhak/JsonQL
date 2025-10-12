=========
'is null'
=========

.. contents::
   :local:
   :depth: 2
   
The unary postfix operator 'is null' is used to check whether a value is explicitly set to JSON null in JsonQL expressions. It returns a boolean result (`true` or `false`).

**Operator Priority**: 300

Operator Operands
=================

- **Operand 1**:    
    - Type: any valid JsonQL expression (including invalid path).
    - Description: The value to be checked for JSON null.

Evaluation Rules
================

The `is null` operator follows these evaluation rules:

- **JSON null value**: Returns `true` if the operand evaluates to a JSON null value
- **Non-null values**: Returns `false` for any non-null value (including numbers, strings, booleans, objects, and arrays)
- **Invalid paths**: Returns `false` for invalid paths (use `is undefined` to check for invalid paths)
- **Missing values from queries**: Returns `false` even when a query returns no results (e.g., `First()` with no matches)

.. important::
    The `is null` operator only returns `true` when a JSON path explicitly references a null value. It does not return `true` for:
    
    - Invalid or non-existent paths
    - Empty query results
    - Undefined values
    
    Use the `is undefined` operator to check for these cases.


Examples
========
    
.. sourcecode:: json

    {
      "Comment_Operator_is_null_1": "(invalid.path is null) evaluates to false. For invalid path check use 'is undefined' operator",
      "Operator_is_null_1": "$value(invalid.path is null == false)",
      "Operator_is_null_2": "$value(NullValue is null)",
      "Operator_is_null_3": "$value(Int1 is null == false)",
      "Operator_is_null_5": "$value(Companies.Select(c => c.Employees).First() is null == false)",

      "Comment_Operator_is_null_6_1": "Even though the query below results in no employee being selected. The value is not null",
      "Comment_Operator_is_null_6_2": "'is null' succeeds if there is JsonPath with null value.",
      "Comment_Operator_is_null_6_3": "'Use 'is undefined' instead to check if query resulted in any value",
      "Operator_is_null_6": "$value(Companies.Select(c => c.Employees).First(e => e.Age > 200) is null == false)"
    }
