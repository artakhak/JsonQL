===
All
===

.. contents::
   :local:
   :depth: 2

The ``All`` function is a boolean aggregate function that determines whether all elements in a collection satisfy a specified condition. It returns ``true`` if every element meets the criteria, or ``false`` if any element fails the condition.

**Function Name**: ``All``

**Return Type**: Boolean

Syntax
======

The ``All`` function can be called with the following syntax:

**Basic Syntax:**

::

    All(collection, criteria)

**Named Parameter Syntax:**

::

    All(collection -> <collection_expression>, criteria -> <lambda_expression>)

Parameters
==========

.. list-table::
   :header-rows: 1
   :widths: 20 15 15 50

   * - Parameter
     - Type
     - Required
     - Description
   * - ``collection``
     - Collection
     - Yes
     - The collection of elements to evaluate. Can be an array, query result, or any collection expression.
   * - ``criteria``
     - Lambda
     - Yes
     - A lambda expression that defines the condition each element must satisfy. Takes one parameter representing the current element and returns a boolean.

Return Value
============

- **Type**: ``Boolean``
- **Returns**: 
    - ``true`` if all elements in the collection satisfy the criteria
    - ``true`` if the collection is empty
    - ``false`` if any element fails to satisfy the criteria

Evaluation Rules
================

The ``All`` function follows these evaluation rules:

1. **Short-Circuit Evaluation**: Stops evaluating as soon as any element fails the criteria and returns ``false``
2. **Empty Collections**: Returns ``true`` for empty collections (vacuous truth)
3. **Null/Undefined Handling**: Null or undefined values are passed to the criteria lambda; the lambda determines how to handle them
4. **All Must Pass**: Every single element must satisfy the criteria for the function to return ``true``
5. **Boolean Criteria**: The criteria lambda must return a boolean value
6. **Order of Evaluation**: Elements are evaluated in the order they appear in the collection

Use Cases
=========

The ``All`` function is useful for:

- **Validation**: Verifying that all items in a collection meet required criteria
- **Quality Checks**: Ensuring all data meets quality standards
- **Business Rules**: Validating that all entities satisfy business rules
- **Preconditions**: Checking that all elements meet prerequisites before processing
- **Data Integrity**: Verifying data consistency across collections
- **Filtering Verification**: Confirming that filtering logic worked correctly

Examples
========
    
.. sourcecode:: json

    {
      "Comment_All_1": "Check if salaries of all employees older than 40 is greater than 85000.",
      "All_1": "$value(All(Companies.Select(c => c.Employees).Where(e => e.Age >= 40), e => e.Salary > 85000))",

      "Comment_All_2": "Another way to check if salaries of all employees older than 40 is greater than 85000.",
      "All_2": "$value(All(Companies.Select(c => c.Employees), e => e.Age < 40 || e.Salary > 85000))",

      "Comment_All_3": "Demo of using named parameters to make the intent clear.",
      "All_3": "$value(All(collection -> Companies.Select(c => c.Employees), criteria -> e => e.Age < 40 || e.Salary > 85000))"
    }