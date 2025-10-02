=====
ToInt
=====

.. contents::
   :local:
   :depth: 2
   
- The function 'ToInt' is used to convert a value to a **integer** value.

Function Parameters
===================

- **value**:
    - Required: Yes
    - Type: Any valid JsonQL expression (including invalid path, null, etc).
    - Description: A JSON value of a JsonQL expression that JsonQL will try to evaluate and convert to an integer value. Otherwise, if the value cannot be converted to an integer value, the function returns undefined.

- **throwOnError**:
    - Required: No    
    - Type: boolean
    - Default value: false
    - Description: Controls error handling behavior when a conversion fails. The conversion might fail if the value evaluated from parameter value is in an invalid format (or is an invalid path). The 'throwOnError' parameter determines what happens in such cases. 
            
            - throwOnError: false (default): If conversion fails, the function returns undefined without stopping compilation.
            - throwOnError: true: If conversion fails, an error is reported that stops the compilation.

Examples
========
    
.. sourcecode:: json

    {
      "ToInt_1": "$(ToInt(value -> ToIntData.InvalidIntText) is undefined) is true",

      "ToInt_2": "$value(ToInt(ToIntData.DoubleText1, throwOnError -> true) == 17)",
      "ToInt_3": "$value(ToInt(ToIntData.NegativeDoubleText1, throwOnError -> true) == -17)",

      "ToInt_4": "$value(ToInt(ToIntData.IntText1))",
      "ToInt_5": "$value(ToInt(ToIntData.NegativeIntText1))",

      "ToInt_6": "$value(ToInt(ToIntData.Double1))",
      "ToInt_7": "$value(ToInt(ToIntData.NegativeDouble1))",

      "ToInt_8": "$value(ToInt(ToIntData.Int1))",
      "ToInt_9": "$value(ToInt(ToIntData.NegativeInt1))"
    }