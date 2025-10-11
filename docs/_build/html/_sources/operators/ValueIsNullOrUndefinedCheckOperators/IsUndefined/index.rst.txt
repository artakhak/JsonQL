==============
'is undefined'
==============

.. contents::
   :local:
   :depth: 2
   
The unary postfix operator 'is not undefined' is used to check whether a value exists and is defined in JsonQL expressions. It returns a boolean result (`true` or `false`).

.. note::
    This operator verifies that a value exists and is accessible. To check specifically that a value is not JSON null, use the :doc:`../IsNotNull/index` operator instead.

**Operator Priority**: 300

Operator Operands
=================

- **Operand 1**:    
    - Type: any valid JsonQL expression (including invalid path).
    - Description: The value or expression to be checked for defined status.

Evaluation Rules
================

The `is not undefined` operator follows these evaluation rules:

- **Invalid paths**: Returns `false` for paths that don't exist in the JSON structure
- **Empty query results**: Returns `false` when a query returns no results (e.g., `First()` with no matching items)
- **Valid values**: Returns `true` for any existing value, including:
  
  - JSON null values
  - Numbers, strings, booleans
  - Objects and arrays
  - Empty collections

.. important::
    The `is not undefined` operator is the logical inverse of `is undefined`:
    
    - Returns `true` when a value or path exists and is accessible
    - Returns `false` when a value is missing or a path is invalid
    
    This operator is useful for:
    
    - Validating that a JSON path exists before using it
    - Checking if a query produced any results
    - Ensuring a value is available for further operations
    
    Note that JSON null values are considered "defined" and will return `true`.


Examples
========
    
.. sourcecode:: json

    {
      "Comment_Operator_is_undefined_1": "(invalid.path is undefined) evaluates to true.",
      "Operator_is_undefined_1": "$value(invalid.path is undefined)",
      "Operator_is_undefined_2": "$value(NullValue is undefined == false)",
      "Operator_is_undefined_3": "$value(Int1 is undefined == false)",
      "Operator_is_undefined_5": "$value(Companies.Select(c => c.Employees).First() is undefined == false)",
      "Operator_is_undefined_6": "$value(Companies.Select(c => c.Employees).First(e => e.Age > 200) is undefined)"
    }
