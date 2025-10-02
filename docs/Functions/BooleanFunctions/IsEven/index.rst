======
IsEven
======

.. contents::
   :local:
   :depth: 2
   
- The function 'IsEven' is used to check if a JsonQL expression evaluates to an even number.

Function Parameters
===================

- **value**:
    - Required: Yes
    - Type: any valid JsonQL expression.
    - Description: Any valid JsonQL. If JsonQL evaluates the value to a numeric even number, function will return true. 

Examples
========
    
.. sourcecode:: json

    {
      "IsEven_1": "$value(IsEven(Count(Companies.Select(c => c.Employees))))",
      "IsEven_2": "$value(IsEvenIsOddData.Array1.Where(x => IsEven(x)))",
      "IsEven_3": "$(IsEven(IsEvenIsOddData.InvalidPath) is undefined) is true"
    }