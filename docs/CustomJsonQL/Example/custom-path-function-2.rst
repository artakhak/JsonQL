================================
Custom Path Function **Second**
================================

.. contents::
   :local:
   :depth: 4
   

The ``Second`` path function selects the second element from a collection that satisfies an optional predicate condition.

Description
===========

This path function iterates through a collection and returns the second element that matches the optional predicate filter. If no predicate is provided, it returns the second element in the collection. The function counts matching items and returns the element when the count reaches 2.

Syntax
======

**Syntax 1 - Without Predicate:**

::

    collection.Second()

**Syntax 2 - With Predicate:**

::

    collection.Second(predicate)

Parameters
==========

.. list-table::
   :header-rows: 1
   :widths: 20 15 15 50
   
   * - Parameter
     - Type
     - Required
     - Description
   * - ``predicate``
     - Lambda
     - No
     - An optional lambda expression that takes a single parameter representing the current element and returns a boolean. When provided, only elements satisfying this predicate are considered. **Format**: ``x => condition``

Return Value
============

- **Type**: Single element (any type)
- **Returns**: 
    - The second element in the collection (when no predicate is provided)
    - The second element that satisfies the predicate condition (when predicate is provided)
    - Invalid path result if the collection has fewer than two (matching) elements

Implementation Details
======================

The ``Second`` function is implemented through the `SelectSecondCollectionItemPathElement <https://github.com/artakhak/JsonQL/tree/main/JsonQL.Demos/CustomJsonQL/Compilation/JsonValueLookup/JsonValuePathElements>`_ class, which:

- Iterates through the collection sequentially
- Applies the optional predicate filter to each element
- Maintains a count of matching items
- Returns the element when the match count reaches 2
- Registers and manages lambda parameter variables
- Returns an invalid path result if insufficient matching elements exist

Examples:
=========

.. code-block:: json

    {
      "Example1": "$value([1, 2, 3, 4, 5].Second())",
      // Result: 2 (second element in array)
      
      "Example2": "$value([10, 20, 30, 40, 50].Second(x => x > 15))",
      // Result: 30 (second element where value > 15)
      
      "Example3": "$value(Employees.Second(e => e.Salary < 100000).Name)",
      // Returns the name of the second employee with salary less than 100000
      
      "Example4": "$value(Companies[0].Employees.Second(x => x.Salary < 100000).Name)",
      // Selects second employee from first company with salary under 100K
      
      "Example5": "$value(Products.Second(p => p.Price > 50))",
      // Returns the second product with price greater than 50
    }

Use Cases
=========

The ``Second`` function is useful for:

- **Fallback Selection**: Selecting an alternative when the first item is not suitable
- **Pattern Matching**: Finding the second occurrence of a specific pattern
- **Data Validation**: Verifying that multiple items meet certain criteria
- **Sampling**: Taking the second sample from filtered data
- **Ranking**: Selecting second-place items in ordered collections

.. note::
    This is a demonstration custom path function. Unlike the built-in ``First`` function which returns the first matching element, this custom function specifically targets the second matching element, providing unique selection capabilities for specific use cases.
