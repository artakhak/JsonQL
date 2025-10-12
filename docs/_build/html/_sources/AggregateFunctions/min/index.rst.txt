===
Min
===

.. contents::
   :local:
   :depth: 2

The ``Min`` function is a numeric aggregate function that returns the minimum (smallest) value from a collection of numeric values. It can operate on a direct collection of numbers or extract values using a lambda expression.

**Function Name**: ``Min``

**Return Type**: Number (Double)

Syntax
======

The ``Min`` function can be called with the following syntax:

**Syntax 1 - Direct Minimum:**

::

    Min(numeric_collection)

**Syntax 2 - With Filter Criteria:**

::

    Min(collection, criteria)

**Syntax 3 - With Value Selector:**

::

    Min(collection, criteria, value)

**Named Parameter Syntax:**

::

    Min(collection -> <collection_expression>, criteria -> <lambda_expression>, value -> <lambda_expression>)

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
     - The collection of elements to evaluate. Can be an array of numbers, query result, or any collection expression.
   * - ``criteria``
     - Lambda
     - No
     - A lambda expression that filters which elements to include in the minimum calculation. Takes one parameter representing the current element and returns a boolean.
   * - ``value``
     - Lambda
     - No
     - A lambda expression that extracts or calculates the numeric value from each element. Takes one parameter and returns a number.

Return Value
============

- **Type**: ``Number`` (double precision)
- **Returns**: 
    - The smallest numeric value from the collection
    - The smallest value among elements that satisfy the criteria (if criteria is provided)
    - The smallest extracted/calculated value (if value selector is provided)
    - ``undefined`` if the collection is empty or contains no numeric values

Evaluation Rules
================

The ``Min`` function follows these evaluation rules:

1. **Direct Numbers**: When collection contains numbers, finds the smallest directly
2. **With Criteria**: Filters elements using criteria, then finds minimum among remaining values
3. **With Value Selector**: Extracts numeric values using the value lambda, then finds minimum
4. **Type Conversion**: Attempts to convert values to numbers; ignores non-numeric values
5. **Null/Undefined Handling**: Ignores ``null`` and ``undefined`` values in the comparison
6. **Complete Iteration**: Processes all (matching) elements to find the true minimum
7. **Initialization**: Starts with the maximum possible double value and updates when smaller values are found

Use Cases
=========

The ``Min`` function is useful for:

- **Finding Lowest Values**: Identifying minimum prices, ages, scores, etc.
- **Range Analysis**: Determining the lower bound of a data range
- **Performance Metrics**: Finding worst-case performance numbers
- **Budget Analysis**: Identifying minimum costs or lowest bids
- **Statistical Analysis**: Computing minimum values for statistical reports
- **Data Validation**: Checking if minimum values meet requirements

Implementation Details
======================

The ``Min`` function is implemented through the `MinMaxAggregateLambdaExpressionFunction <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/JsonFunction/JsonFunctions/AggregateFunctions/MinMaxAggregateLambdaExpressionFunction.cs>`_ class, which:

- Initializes the minimum value to ``Double.MaxValue``
- Iterates through all elements in the collection
- Applies the criteria filter if provided (only evaluates matching elements)
- Extracts numeric values using the value selector lambda if provided
- Compares each value with the current minimum and updates if smaller
- Returns the final minimum value found
- Handles type conversion to ensure values are numeric

Best Practices
==============

- **Pre-Filter vs Criteria**: Use ``Where`` before ``Min`` for simple filters; use criteria parameter for integrated filtering
- **Value Selector**: Use the value parameter when you need to extract or calculate the numeric value
- **Named Parameters**: Use named parameters for complex expressions with all three parameters
- **Type Safety**: Ensure values are numeric or can be converted to numbers
- **Null Handling**: Be aware that null values are ignored; use criteria to filter them explicitly if needed
- **Empty Collections**: Handle cases where filtering might result in empty collections

Performance Considerations
==========================

- **Complete Iteration**: ``Min`` must process all (matching) elements to find the true minimum
- **Early Conversion**: Values are converted to numbers early in the process
- **Criteria First**: When using criteria, non-matching elements are skipped before value extraction
- **Pre-Filtering**: Pre-filtering with ``Where`` can sometimes be more efficient than using criteria parameter

Notes
=====

- The ``Min`` function returns a numeric value (double precision)
- It ignores null and ``undefined`` values during comparison
- Non-numeric values are skipped (after attempting conversion)
- Empty collections or collections with no valid numeric values return ``undefined``
- The function processes all elements to ensure the true minimum is found
- Initialization uses ``Double.MaxValue`` to ensure any actual value will be smaller

Common Patterns
===============

**Simple Minimum Pattern:**

::

    Min(numeric_collection)

**Filtered Minimum Pattern:**

::

    Min(collection.Where(predicate).Select(selector))

**Criteria-Based Minimum Pattern:**

::

    Min(collection, item => item.Condition, item => item.NumericProperty)

**Property Minimum Pattern:**

::

    Min(collection.Select(item => item.Property))

**Calculated Minimum Pattern:**

::

    Min(collection, filter => filter.IsValid, item => item.Value * factor)

Practical Use Cases
====================

**Price Analysis:**

.. code-block:: json

    {
      "LowestPrice": "$value(Min(Products.Select(p => p.Price)))",
      "LowestActiveProductPrice": "$value(Min(Products, p => p.IsActive, p => p.Price))"
    }

Find the lowest prices for pricing analysis.

**Performance Metrics:**

.. code-block:: json

    {
      "WorstResponseTime": "$value(Min(ApiCalls, c => c.Status == 'Success', c => c.ResponseTime))"
    }

Identify the worst (minimum in this context might mean slowest) performance metric.

**Budget Planning:**

.. code-block:: json

    {
      "MinimumBudget": "$value(Min(Departments.Select(d => d.Budget)))"
    }

Find the department with the smallest budget.

**Quality Control:**

.. code-block:: json

    {
      "LowestScore": "$value(Min(Inspections, i => i.IsCompleted, i => i.QualityScore))"
    }

Identify the lowest quality score among completed inspections.

**Age Demographics:**

.. code-block:: json

    {
      "YoungestEmployeeAge": "$value(Min(Employees.Where(e => e.IsActive).Select(e => e.Age)))"
    }

Find the age of the youngest active employee.


Examples
========
    
.. sourcecode:: json

    {
      "Comment_Min_1": "Retrieve minimum salary of employees older than 40.",
      "Min_1": "$value(Min(Companies.Select(c => c.Employees.Where(e => e.Age >= 40)).Select(e => e.Salary)))",

      "Comment_Min_2": "Another way to retrieve minimum salary of employees older than 40.",
      "Min_2": "$value(Min(Companies.Select(c => c.Employees), e => e.Age >= 40, e => e.Salary))",

      "Comment_Min_3": "The value evaluated for the minimum of collection items is undefined.",
      "Min_3": "$value(Min(collection -> Companies.Select(c => c.Employees), criteria -> e => e.Age >= 200, value -> e => e.Salary) is undefined)",

      "Comment_Min_4": "Demo of using named parameters to make the intent clear.",
      "Min_4": "$value(Min(collection -> Companies.Select(c => c.Employees), criteria -> e => e.Age >= 40, value -> e => e.Salary))"
    }