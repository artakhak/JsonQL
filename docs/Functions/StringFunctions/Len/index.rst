===
Len
===

.. contents::
   :local:
   :depth: 2
   
- The function 'Len' returns length of a string.  

Function Parameters
===================

- **text**:
    - Required: Yes
    - Type: Any JsonQL expression that evaluates to string.

Examples
========
    
.. sourcecode:: json

    {
      "Len_1": "$value(Companies.Select(c => c.Employees).First(e => Len(e.Address.Street) >= 15))",
      "Len_2": "$value(Len(LenData.Array1[0].Address.Street))",
      "Len_3": "$(Len(LenData.InvalidPath) is undefined) is true"
    }