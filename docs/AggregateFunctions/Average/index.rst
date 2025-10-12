=======
Average
=======

.. contents::
   :local:
   :depth: 2

The ``Average`` function is a numeric aggregate function that computes the arithmetic mean (average) of numeric values in a collection. It sums all qualifying values and divides by the count to produce the average.

**Function Name**: ``Average``

**Return Type**: Number (Double)

Syntax
======

The ``Average`` function can be called with the following syntax:

**Syntax 1 - Direct Average:**

::

    Average(numeric_collection)

**Syntax 2 - With Filter Criteria:**

::

    Average(collection, criteria)

**Syntax 3 - With Value Selector:**

::

    Average(collection, criteria, value)

**Named Parameter Syntax:**

::

    Average(collection -> <collection_expression>, criteria -> <lambda_expression>, value -> <lambda_expression>)

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
     - A lambda expression that filters which elements to include in the average calculation. Takes one parameter representing the current element and returns a boolean.
   * - ``value``
     - Lambda
     - No
     - A lambda expression that extracts or calculates the numeric value from each element. Takes one parameter and returns a number.

Return Value
============

- **Type**: ``Number`` (double precision)
- **Returns**: 
    - The arithmetic mean of all numeric values in the collection
    - The average of values among elements that satisfy the criteria (if criteria is provided)
    - The average of extracted/calculated values (if value selector is provided)
    - ``undefined`` when no elements match the criteria or when the collection is empty

Evaluation Rules
================

The ``Average`` function follows these evaluation rules:

1. **Direct Numbers**: When collection contains numbers, calculates average directly
2. **With Criteria**: Filters elements using criteria, then calculates average of remaining values
3. **With Value Selector**: Extracts numeric values using the value lambda, then calculates average
4. **Type Conversion**: Attempts to convert values to numbers; ignores non-numeric values
5. **Null/Undefined Handling**: Ignores ``null`` and ``undefined`` values in the calculation
6. **Complete Iteration**: Processes all (matching) elements to compute accurate average
7. **Division**: Divides the sum by the count of evaluated values
8. **Empty Collection**: Returns ``0`` when no values are evaluated

Calculation Formula
===================

The average is calculated as:

::

    Average = Sum of all values / Count of values

For example:
- Values: [10, 20, 30, 40]
- Sum: 100
- Count: 4
- Average: 100 / 4 = 25

Use Cases
=========

The ``Average`` function is useful for:

- **Statistical Analysis**: Computing mean values for datasets
- **Performance Metrics**: Calculating average response times, scores, or ratings
- **Financial Analysis**: Determining average prices, costs, or revenues
- **Demographic Analysis**: Computing average ages, incomes, or other population metrics
- **Quality Metrics**: Calculating average quality scores or satisfaction ratings
- **Trend Analysis**: Understanding typical or central tendency values

Implementation Details
======================

The ``Average`` function is implemented through the `AverageAggregateLambdaExpressionFunction <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/JsonFunction/JsonFunctions/AggregateFunctions/AverageAggregateLambdaExpressionFunction.cs>`_ class, which:

- Maintains a running sum and count of evaluated values
- Iterates through all elements in the collection
- Applies the criteria filter if provided (only evaluates matching elements)
- Extracts numeric values using the value selector lambda if provided
- Accumulates the sum and increments the count for each valid value
- Divides the final sum by the count to produce the average
- Returns ``undefined`` for empty collections
- Handles type conversion to ensure values are numeric

Best Practices
==============

- **Pre-Filter vs Criteria**: Use ``Where`` before ``Average`` for simple filters; use criteria parameter for integrated filtering
- **Value Selector**: Use the value parameter when you need to extract or calculate the numeric value
- **Named Parameters**: Use named parameters for complex expressions with all three parameters
- **Type Safety**: Ensure values are numeric or can be converted to numbers
- **Null Handling**: Be aware that null values are ignored; use criteria to filter them explicitly if needed
- **Empty Collections**: Handle cases where filtering might result in empty collections (returns 0)
- **Precision**: Remember the result is double precision; consider rounding for display

Performance Considerations
==========================

- **Complete Iteration**: ``Average`` must process all (matching) elements to compute the accurate average
- **Single Pass**: Calculation is done in a single pass through the data
- **Criteria First**: When using criteria, non-matching elements are skipped before value extraction
- **Pre-Filtering**: Pre-filtering with ``Where`` can sometimes be more efficient than using criteria parameter

Notes
=====

- The ``Average`` function returns a numeric value (double precision)
- It ignores ``null`` and ``undefined`` values during calculation
- Non-numeric values are skipped (after attempting conversion)
- Returns ``undefined`` when no elements match the criteria or when the collection is empty
- The function processes all elements to ensure an accurate average
- The result is the sum divided by the count of valid numeric values
- Division by zero is avoided by returning ``0`` when count is zero

Common Patterns
===============

**Simple Average Pattern:**

::

    Average(numeric_collection)

**Filtered Average Pattern:**

::

    Average(collection.Where(predicate).Select(selector))

**Criteria-Based Average Pattern:**

::

    Average(collection, item => item.Condition, item => item.NumericProperty)

**Property Average Pattern:**

::

    Average(collection.Select(item => item.Property))

**Calculated Average Pattern:**

::

    Average(collection, filter => filter.IsValid, item => item.Value * factor)

Practical Use Cases
====================

**Salary Analysis:**

.. code-block:: json

    {
      "AverageSalary": "$value(Average(Employees.Select(e => e.Salary)))",
      "AverageDepartmentSalary": "$value(Average(Employees, e => e.Department == 'Engineering', e => e.Salary))"
    }

Calculate average salaries for compensation analysis.

**Performance Metrics:**

.. code-block:: json

    {
      "AverageResponseTime": "$value(Average(ApiCalls, c => c.Status == 'Success', c => c.ResponseTime))"
    }

Compute average response times for performance monitoring.

**Student Grades:**

.. code-block:: json

    {
      "ClassAverage": "$value(Average(Students.Select(s => s.FinalGrade)))",
      "PassingAverage": "$value(Average(Students, s => s.FinalGrade >= 60, s => s.FinalGrade))"
    }

Calculate class averages and averages for passing students.

**Financial Analysis:**

.. code-block:: json

    {
      "AverageRevenue": "$value(Average(Departments.Select(d => d.MonthlyRevenue)))"
    }

Compute average monthly revenue across departments.

**Customer Metrics:**

.. code-block:: json

    {
      "AverageOrderValue": "$value(Average(Orders, o => o.Status == 'Completed', o => o.Total))",
      "AverageRating": "$value(Average(Products.Select(p => p.CustomerRating)))"
    }

Calculate average order values and product ratings.

**Age Demographics:**

.. code-block:: json

    {
      "AverageEmployeeAge": "$value(Average(Employees.Where(e => e.IsActive).Select(e => e.Age)))"
    }

Find the average age of active employees.

Examples
========
    
.. sourcecode:: json

    {
      "Comment_Average_1": "Retrieve average salary of employees older than 40.",
      "Average_1": "$value(Average(Companies.Select(c => c.Employees.Where(e => e.Age >= 40)).Select(e => e.Salary)))",

      "Comment_Average_2": "Another way to retrieve average salary of employees older than 40.",
      "Average_2": "$value(Average(Companies.Select(c => c.Employees), e => e.Age >= 40, e => e.Salary))",

      "Comment_Average_3": "The value evaluated for the average of collection items is undefined.",
      "Average_3": "$value(Average(collection -> Companies.Select(c => c.Employees), criteria -> e => e.Age >= 200, value -> e => e.Salary) is undefined)",

      "Comment_Average_4": "Demo of using named parameters to make the intent clear.",
      "Average_4": "$value(Average(collection -> Companies.Select(c => c.Employees), criteria -> e => e.Age >= 40, value -> e => e.Salary))"
    }