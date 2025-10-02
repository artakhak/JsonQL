===========
Concatenate
===========

.. contents::
   :local:
   :depth: 2
   
- The function 'Concatenate' concatenates values evaluated from jsonQL expressions. 
    .. note::
    
        - Some non-string values like booleans, date/time, numbers, etc are automatically converted to strings before being concatenated, but to have better control over the format of converted values, use conversion functions outlines in :doc:`../../ConversionFunctions/ToString/index`.

Function Parameters
===================

- **values**:
    - Required: Yes
    - Type: List of comma separated valid JsonQL expressions.
    - Description: Collection of expressions to concatenate. JsonQL will try to convert the values in this parameter to string type, if the values are not strings. 

Examples
========
    
.. sourcecode:: json

    {
      "ConcatenateData_1": "$value(ConcatenateData.Array1.Where(x => HasField(x, 'FirstName')).Select(e => Concatenate(e.FirstName, ' ', e.LastName)))",
      "ConcatenateData_2": "$value(Concatenate('Mr ', ConcatenateData.Array1[0].FirstName, ' ', ConcatenateData.Array1[0].LastName))",
      "ConcatenateData_3": "$(Concatenate('Mr ', ConcatenateData.Array1[1000].FirstName, ' ', ConcatenateData.Array1[0].LastName) is undefined) is true",
      "ConcatenateData_4": "$value(Concatenate(1, ' test', ' ', true, ' ', false))"
    }