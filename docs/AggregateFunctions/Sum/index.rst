===
Sum
===

.. contents::
   :local:
   :depth: 2

The ``Sum`` function is a numeric aggregate function that calculates the total sum of numeric values in a collection. It adds all qualifying values together to produce a cumulative total.

**Function Name**: ``Sum``

**Return Type**: Number (Double)

Syntax
======

The ``Sum`` function can be called with the following syntax:

**Syntax 1 - Direct Sum:**

::

    Sum(numeric_collection)

**Syntax 2 - With Filter Criteria:**

::

    Sum(collection, criteria)

**Syntax 3 - With Value Selector:**

::

    Sum(collection, criteria, value)

**Named Parameter Syntax:**

::

    Sum(collection -> <collection_expression>, criteria -> <lambda_expression>, value -> <lambda_expression>)

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
     - The collection of elements to sum. Can be an array of numbers, query result, or any collection expression.
   * - ``criteria``
     - Lambda
     - No
     - A lambda expression that filters which elements to include in the sum calculation. Takes one parameter representing the current element and returns a boolean.
   * - ``value``
     - Lambda
     - No
     - A lambda expression that extracts or calculates the numeric value from each element. Takes one parameter and returns a number.

Return Value
============

- **Type**: ``Number`` (double precision)
- **Returns**: 
    - The total sum of all numeric values in the collection
    - The sum of values among elements that satisfy the criteria (if criteria is provided)
    - The sum of extracted/calculated values (if value selector is provided)
    - ``undefined`` when no elements match the criteria or when the collection is empty

Evaluation Rules
================

The ``Sum`` function follows these evaluation rules:

1. **Direct Numbers**: When collection contains numbers, sums them directly
2. **With Criteria**: Filters elements using criteria, then sums remaining values
3. **With Value Selector**: Extracts numeric values using the value lambda, then sums them
4. **Type Conversion**: Attempts to convert values to numbers; ignores non-numeric values
5. **Null/Undefined Handling**: Ignores ``null`` and ``undefined`` values in the calculation
6. **Complete Iteration**: Processes all (matching) elements to compute accurate sum
7. **Accumulation**: Maintains a running total that increases with each valid value
8. **Empty Result**: Returns ``undefined`` when no values are summed

Use Cases
=========

The ``Sum`` function is useful for:

- **Financial Calculations**: Computing total revenues, costs, or expenses
- **Inventory Management**: Calculating total quantities or stock values
- **Statistical Analysis**: Computing totals for further analysis
- **Payroll Processing**: Calculating total salaries or compensation
- **Sales Analysis**: Determining total sales amounts
- **Resource Planning**: Summing resource allocations or consumption

Implementation Details
======================

The ``Sum`` function is implemented through the `SumAggregateLambdaExpressionFunction <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/JsonFunction/JsonFunctions/AggregateFunctions/SumAggregateLambdaExpressionFunction.cs>`_ class, which:

- Maintains a running sum initialized to 0
- Iterates through all elements in the collection
- Applies the criteria filter if provided (only evaluates matching elements)
- Extracts numeric values using the value selector lambda if provided
- Adds each valid value to the running total
- Returns the final sum
- Returns ``undefined`` if no values were summed
- Handles type conversion to ensure values are numeric

Best Practices
==============

- **Pre-Filter vs Criteria**: Use ``Where`` before ``Sum`` for simple filters; use criteria parameter for integrated filtering
- **Value Selector**: Use the value parameter when you need to extract or calculate the numeric value
- **Named Parameters**: Use named parameters for complex expressions with all three parameters
- **Type Safety**: Ensure values are numeric or can be converted to numbers
- **Null Handling**: Be aware that null values are ignored; use criteria to filter them explicitly if needed
- **Empty Collections**: Handle cases where filtering might result in no values (returns ``undefined``)
- **Large Numbers**: Be mindful of numeric overflow with very large sums

Performance Considerations
==========================

- **Complete Iteration**: ``Sum`` must process all (matching) elements to compute the accurate total
- **Single Pass**: Calculation is done in a single pass through the data
- **Criteria First**: When using criteria, non-matching elements are skipped before value extraction
- **Pre-Filtering**: Pre-filtering with ``Where`` can sometimes be more efficient than using criteria parameter
- **Accumulation Cost**: Addition operations are very fast, making ``Sum`` efficient even for large collections

Notes
=====

- The ``Sum`` function returns a numeric value (double precision)
- It ignores ``null`` and ``undefined`` values during calculation
- Non-numeric values are skipped (after attempting conversion)
- Returns ``undefined`` when no values are summed (not 0)
- The function processes all elements to ensure an accurate sum
- The result is the cumulative total of all valid numeric values
- Useful for financial calculations, totals, and aggregations

Common Patterns
===============

**Simple Sum Pattern:**

::

    Sum(numeric_collection)

**Filtered Sum Pattern:**

::

    Sum(collection.Where(predicate).Select(selector))

**Criteria-Based Sum Pattern:**

::

    Sum(collection, item => item.Condition, item => item.NumericProperty)

**Property Sum Pattern:**

::

    Sum(collection.Select(item => item.Property))

**Calculated Sum Pattern:**

::

    Sum(collection, filter => filter.IsValid, item => item.Value * factor)

Practical Use Cases
====================

**Payroll Calculations:**

.. code-block:: json

    {
      "TotalPayroll": "$value(Sum(Employees.Select(e => e.Salary)))",
      "DepartmentPayroll": "$value(Sum(Employees, e => e.Department == 'Engineering', e => e.Salary))"
    }

Calculate total payroll and department-specific payroll costs.

**Sales Analysis:**

.. code-block:: json

    {
      "TotalRevenue": "$value(Sum(Orders, o => o.Status == 'Completed', o => o.Total))"
    }

Compute total revenue from completed orders.

**Inventory Valuation:**

.. code-block:: json

    {
      "TotalInventoryValue": "$value(Sum(Products, p => p.InStock, p => p.Price * p.Quantity))"
    }

Calculate total inventory value by multiplying price and quantity for in-stock products.

**Financial Reporting:**

.. code-block:: json

    {
      "TotalExpenses": "$value(Sum(Transactions, t => t.Type == 'Expense', t => t.Amount))",
      "TotalIncome": "$value(Sum(Transactions, t => t.Type == 'Income', t => t.Amount))",
      "NetProfit": "$value(Sum(Transactions, t => t.Type == 'Income', t => t.Amount) - Sum(Transactions, t => t.Type == 'Expense', t => t.Amount))"
    }

Calculate total expenses, income, and net profit from financial transactions.

**Resource Allocation:**

.. code-block:: json

    {
      "TotalAllocatedHours": "$value(Sum(Projects.Select(p => p.AssignedHours)))"
    }

Sum allocated hours across all projects for resource planning.

**Budget Analysis:**

.. code-block:: json

    {
      "TotalBudget": "$value(Sum(Departments.Select(d => d.Budget)))",
      "RemainingBudget": "$value(Sum(Departments, d => d.Status == 'Active', d => d.Budget - d.Spent))"
    }

Calculate total budgets and remaining budgets across departments.


Examples
========
    
.. sourcecode:: json

    {
      "Comment_Sum_1": "Retrieve sum of all salaries of employees older than 40.",
      "Sum_1": "$value(Sum(Companies.Select(c => c.Employees.Where(e => e.Age >= 40)).Select(e => e.Salary)))",

      "Comment_Sum_2": "Another way to retrieve sum of all salaries of employees older than 40.",
      "Sum_2": "$value(Sum(Companies.Select(c => c.Employees), e => e.Age >= 40, e => e.Salary))",

      "Comment_Sum_3": "The value evaluated for the sum of collection items is undefined.",
      "Sum_3": "$value(Sum(collection -> Companies.Select(c => c.Employees), criteria -> e => e.Age >= 200, value -> e => e.Salary) is undefined)",

      "Comment_Sum_4": "Demo of using named parameters to make the intent clear.",
      "Sum_4": "$value(Sum(collection -> Companies.Select(c => c.Employees), criteria -> e => e.Age >= 40, value -> e => e.Salary))"
    }