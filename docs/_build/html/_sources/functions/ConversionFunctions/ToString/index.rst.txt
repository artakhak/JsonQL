========
ToString
========

.. contents::
   :local:
   :depth: 2
   
- The function 'ToString' is used to convert a value to a **string** value.

.. note::
    The following interface is used to format a valure as a string (if the value is already a string, it will be returned without formatting) `JsonQL.IDateTimeOperations <https://github.com/artakhak/JsonQL/blob/main/JsonQL/IDateTimeOperations.cs>`_. Provide a custom implementation of this interface to change the conversion format.


Function Parameters
===================

- **value**:
    - Required: Yes
    - Type: Any valid JsonQL expression (including invalid path, null, etc).
    - Description: A JSON value of a JsonQL expression that JsonQL will try to evaluate and convert to a string value. Otherwise, if the value cannot be converted to double value, the function returns undefined.

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
      "ToString_1": "$(ToString(value -> InvalidPath) is undefined) is true",

      "ToString_2": "$value(ToString(ToStringData.Int1, throwOnError -> true) == '15')",
      "ToString_3": "$value(ToString(ToStringData.NegativeInt1, throwOnError -> true))",

      "ToString_4": "$value(ToString(ToStringData.Double1))",
      "ToString_5": "$value(ToString(ToStringData.NegativeDouble1))",

      "ToString_6": "$value(ToString(ToStringData.Text1))",

      "ToString_7": "$value(ToString(ToStringData.TrueValue))",
      "ToString_8": "'$(ToString(false))' == 'false'",

      "ToString_9": "$(ToString(ToDateTime('2022-05-23T18:25:43.511Z'))=='2022-05-23 14:25:43.5110000') is true"
    }