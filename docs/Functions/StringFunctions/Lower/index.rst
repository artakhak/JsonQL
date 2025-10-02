=====
Lower
=====

.. contents::
   :local:
   :depth: 2
   
- The function 'Lower' converts the string value to lower case.       

Function Parameters
===================

- **text**:
    - Required: Yes
    - Type: Any JsonQL expression that evaluates to string.

Examples
========
    
.. sourcecode:: json

    {
      "Lower_1": "$value(LowerUpperData.Array1.Where(x => HasField(x, 'FirstName')).Select(e => Lower(e.LastName)))",
      "Lower_2": "$value(Lower(Concatenate('Mr ', LowerUpperData.Array1[0].FirstName, ' ', LowerUpperData.Array1[0].LastName)))",
      "Lower_3": "$(Lower(LowerUpperData.Array1[1000].LastName) is undefined) is true"
    }