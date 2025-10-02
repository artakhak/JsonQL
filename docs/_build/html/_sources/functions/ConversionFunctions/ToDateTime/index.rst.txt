==========
ToDateTime
==========

.. contents::
   :local:
   :depth: 2
   
- The function 'ToDateTime' is used to convert a value to a **DateTime** value.

.. note::
    The following interface is used to parse a value evaluated to string to a datetime (if the value is already a DateTime, it will be returned without conversion) `JsonQL.IDateTimeOperations <https://github.com/artakhak/JsonQL/blob/main/JsonQL/IDateTimeOperations.cs>`_. Provide a custom implementation of this interface to change the conversion format.


Function Parameters
===================

- **value**:
    - Required: Yes
    - Type: Any valid JsonQL expression (including invalid path, null, etc).
    - Description: If the JsonQL expression in this parameter is evaluated to DateTime or to a string containing a DateTime in a valid format, the value will be converted to a DateTime value. Otherwise the function returns undefined.

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
      "ToDateTime_1": "$(ToDateTime(value -> ToDateTimeData.InvalidDateTimeText) is undefined) is true",
      "ToDateTime_2": "$value(ToDateTime(ToDateTimeData.DateTimeText1, throwOnError -> true))",
      "ToDateTime_3": "$value(ToDateTime('2022-05-23', throwOnError -> true))",
      "ToDateTime_4": "$value(ToDateTime('2022-05-23T18:25:43.511Z') < ToDateTime('2025-05-23T18:25:43.511Z'))"
    }