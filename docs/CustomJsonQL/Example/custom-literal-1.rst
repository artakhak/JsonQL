====================================
Custom Literal **JsonQLReleaseDate**
====================================

.. contents::
   :local:
   :depth: 4
   

The ``JsonQLReleaseDate`` literal (a special keyword) returns a constant DateTime value representing the release date of the JsonQL library.

Return Value
============

- **Type**: ``DateTime``
- **Returns**: A DateTime value of ``2025-06-01 00:00:00`` representing the JsonQL library release date.

Examples:
=========

.. code-block:: json
    
    {
      "Example1": "JsonQLReleaseDate is '$(JsonQLReleaseDate)'",
      // Result: "JsonQLReleaseDate is '2025-06-01 00:00:00.0000000'"
      
      "Example2": "The type of value of 'JsonQLReleaseDate' function is '$(typeof JsonQLReleaseDate)'",
      // Result: "The type of value of 'JsonQLReleaseDate' function is 'DateTime'"
    }