============================================
Custom Function **ReverseTextAndAddMarkers**
============================================

.. contents::
   :local:
   :depth: 4
   
Parameters
==========

.. list-table::
   :header-rows: 1
   :widths: 20 15 15 50
   
   * - Parameter
     - Type
     - Required
     - Description
   * - ``value``
     - String
     - Yes
     - The input string to be reversed.
   * - ``addMarkers``
     - Boolean
     - No
     - When `true` or omitted, the reversed string is wrapped with `#` markers (e.g., `#tseT#`). When `false`, returns only the reversed string without markers. **Default**: `true`.

Return Value
============

- **Type**: `string`
- **Returns**:
    - The reversed input string wrapped with `#` markers when `addMarkers` is `true` or omitted (e.g., `#tseT#`)
    - The reversed input string without markers when `addMarkers` is `false` (e.g., `tseT`)

Examples:
=========

.. code-block:: json
    
    {
      "Example1": "$(ReverseTextAndAddMarkers('Test'))",
      // Result: "#tseT#"
      
      "Example2": "$(ReverseTextAndAddMarkers('Test', true))",
      // Result: "#tseT#"
      
      "Example3": "$(ReverseTextAndAddMarkers('Test', false))",
      // Result: "tseT"
      
      "Example4": "$(ReverseTextAndAddMarkers(value->'Test', addMarkers->false))",
      // Result: "tseT"
      
      "Example5": "$(ReverseTextAndAddMarkers(addMarkers->false, value->'Hello World'))",
      // Result: "dlroW olleH"
    }
