========
ToDouble
========

.. contents::
   :local:
   :depth: 2
   
- The function 'ToDouble' is used to convert a value to a **double** value.

Function Parameters
===================

- **value**:
    - Required: Yes
    - Type: Any valid JsonQL expression (including invalid path, null, etc).
    - Description: A JSON value of a JsonQL expression that JsonQL will try to evaluate and convert to a double value. Otherwise, if the value cannot be converted to double value, the function returns undefined.

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
      "ToDouble_1": "$(ToDouble(value -> ToDoubleData.InvalidDoubleText) is undefined) is true",

      "ToDouble_2": "$value(ToDouble(ToDoubleData.DoubleText1, throwOnError -> true) == 1.36)",
      "ToDouble_3": "$value(ToDouble(ToDoubleData.NegativeDoubleText1, throwOnError -> true) == -1.36)",

      "ToDouble_4": "$value(ToDouble(ToDoubleData.IntText1))",
      "ToDouble_5": "$value(ToDouble(ToDoubleData.NegativeIntText1))",

      "ToDouble_6": "$value(ToDouble(ToDoubleData.Double1))",
      "ToDouble_7": "$value(ToDouble(ToDoubleData.NegativeDouble1))",

      "ToDouble_8": "$value(ToDouble(ToDoubleData.Int1))",
      "ToDouble_9": "$value(ToDouble(ToDoubleData.NegativeInt1))"
    }