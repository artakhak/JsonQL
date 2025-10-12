========
Overview
========

.. contents::
   :local:
   :depth: 2

Aggregate functions process collections and return computed values such as totals, averages, counts, or extreme values. They are commonly used to:

- Summarize large datasets into meaningful metrics
- Perform statistical analysis on collections
- Calculate business metrics and KPIs
- Generate reports and analytics
- Support data-driven decision making

Available Aggregate Functions
==============================

JsonQL provides the following aggregate functions:

**Numeric Aggregations:**

- **Min**: Returns the minimum (smallest) value in a collection
- **Max**: Returns the maximum (largest) value in a collection  
- **Sum**: Calculates the total sum of numeric values in a collection
- **Average**: Computes the arithmetic mean of numeric values in a collection

**Counting:**

- **Count**: Returns the number of elements in a collection

**Boolean Aggregations:**

- **All**: Checks if all elements in a collection satisfy a specified condition
- **Any**: Checks if any element in a collection satisfies a specified condition (or if collection is non-empty)


Common Characteristics
======================

All aggregate functions share these characteristics:

- **Collection Input**: They operate on collections (arrays or query results)
- **Single Output**: They return a single scalar value as the result
- **Empty Collections**: Behavior with empty collections varies by function
- **Null Handling**: Typically ignore null or undefined values in calculations
- **Type-Specific**: Some functions work only with specific data types (e.g., numeric values)

Usage Patterns
==============

Aggregate functions can be used in various contexts:

**Direct Collection Aggregation:**

.. code-block:: json

    {
      "TotalSales": "$value(Sum(Sales))"
    }

**With Collection Queries:**

.. code-block:: json

    {
      "AverageAge": "$value(Average(Employees.Where(e => e.Department == 'Engineering').Select(e => e.Age)))"
    }

**Multiple Aggregations:**

.. code-block:: json

    {
      "MinPrice": "$value(Min(Products.Select(p => p.Price)))",
      "MaxPrice": "$value(Max(Products.Select(p => p.Price)))",
      "TotalCount": "$value(Count(Products))"
    }

The sections that follow provide detailed documentation for each aggregate function.



   