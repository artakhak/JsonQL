===
Abs
===

.. contents::
   :local:
   :depth: 2
   
- The function 'Abs' is used to get absolute value of a number.

Function Parameters
===================

- **value**:
    - Required: Yes
    - Type: any valid JsonQL expression.
    - Description: Any valid JsonQL. If JsonQL evaluates the value to a numeric number, function will return absolute value of the number. Otherwise, undefined value will be returned. 

Examples
========
    
.. sourcecode:: json

    {
      "Abs_1": "$value(AbsData.Array1.Where(x => typeof x == 'Number').Select(x => Abs(x)))",
      "Abs_2": "$value(AbsData.Array1.Where(x => Abs(x) > 3))",
      "Abs_3": "$(Abs(AbsData.Invalid) is undefined) is true"
    }