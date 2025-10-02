=====
Upper
=====

.. contents::
   :local:
   :depth: 2
   
- The function 'Upper' converts the string value to upper case.       

Function Parameters
===================

- **text**:
    - Required: Yes
    - Type: Any JsonQL expression that evaluates to string.

Examples
========
    
.. sourcecode:: json

    {
      "Upper_1": "$value(LowerUpperData.Array1.Where(x => HasField(x, 'FirstName')).Select(e => Upper(e.LastName)))",
      "Upper_2": "$value(Upper(Concatenate('Mr ', LowerUpperData.Array1[0].FirstName, ' ', LowerUpperData.Array1[0].LastName)))",
      "Upper_3": "$(Upper(LowerUpperData.Array1[1000].LastName) is undefined) is true"
    }