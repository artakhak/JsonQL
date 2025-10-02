======
ToDate
======

.. contents::
   :local:
   :depth: 2
   
- The function 'ToDate' is used to convert a value to a **Date** value.

.. note::
    The following interface is used to parse a value evaluated to string to a datetime (if the value is already a Date, it will be returned without conversion) `JsonQL.IDateTimeOperations <https://github.com/artakhak/JsonQL/blob/main/JsonQL/IDateTimeOperations.cs>`_. Provide a custom implementation of this interface to change the conversion format.


Function Parameters
===================

- **value**:
    - Required: Yes
    - Type: Any valid JsonQL expression (including invalid path, null, etc).
    - Description: If the JsonQL expression in this parameter is evaluated to a DateTime or to a string containing a Date in a valid format, the value will be converted to a DateTime value. Otherwise the function returns undefined.

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
      "ToDate_1": "$(ToDate(value -> ToDateData.InvalidDateTimeText) is undefined) is true",
      "ToDate_2": "$value(ToDate(ToDateData.DateTimeText1, throwOnError -> true))",
      "ToDate_3": "$value(ToDate(ToDateData.DateText1, throwOnError -> true))",
      "ToDate_4": "$value(ToDate('2022-05-23', throwOnError -> true))",
      "ToDate_5": "$value(ToDate('2022-05-23T18:25:43.511Z') < ToDate('2025-05-23T18:25:43.511Z'))",
      "ToDate_6": "$value(ToDate(ToDateData.DateTimeText1) < ToDateTime(ToDateData.DateTimeText1))"
    }