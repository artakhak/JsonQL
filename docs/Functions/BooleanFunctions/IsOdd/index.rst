=====
IsOdd
=====

.. contents::
   :local:
   :depth: 2
   
- The function 'IsOdd' is used to check if a JsonQL expression evaluates to an odd number.

Function Parameters
===================

- **value**:
    - Required: Yes
    - Type: any valid JsonQL expression.
    - Description: Any valid JsonQL. If JsonQL evaluates the value to a numeric odd number, function will return true. 

Examples
========
    
.. sourcecode:: json

    {
      "IsOdd_1": "$value(IsOdd(Count(Companies.Select(c => c.Employees))))",
      "IsOdd_2": "$value(IsEvenIsOddData.Array1.Where(x => IsOdd(x)))",
      "IsOdd_3": "$(IsOdd(IsEvenIsOddData.InvalidPath) is undefined) is true"
    }