===============
Not Equals '!='
===============

.. contents::
   :local:
   :depth: 2
   
The binary operator '!=' is used to evaluate whether two values are not equal in JsonQL expressions. It returns a boolean result (`true` or `false`).

**Operator Priority**: 600

Operator Operands
=================

- **Operand 1**:    
    - Type: any valid JsonQL expression (including invalid path).
    - Description: Any valid JsonQL expression.
    
- **Operand 2**:    
    - Type: any valid JsonQL expression (including invalid path).
    - Description: Any valid JsonQL expression.

Type Comparison Rules
=====================

- **Numeric comparisons**: Integer and double values can be compared (e.g., `15 != 15.00` returns `false`)
- **String comparisons**: Must be exact matches; no type coercion (e.g., `15 != '15'` returns `true`)
- **Boolean comparisons**: Must match exactly; no string conversion (e.g., `true != 'true'` returns `true`)
- **Date/DateTime comparisons**: Date values ignore time components (e.g., `ToDate('2022-05-23')` is not different from `ToDate('2022-05-23T18:25:43.511Z')`)
- **Undefined values**: Any comparison with undefined returns `true`
    
Examples
========
    
.. sourcecode:: json

    {
      "Comment_Operator_!=_1": "(15 != invalid.path) is evaluated to true",
      "Operator_!=_1": "$value((15 != invalid.path) == true)",
      "Comment_Operator_!=_2": "(e.SalaryInvalid != 100000) is evaluated to true for all employees",
      "Operator_!=_2": "$value(Count(Companies.Select(c => c.Employees.Where(e => e.SalaryInvalid != 100000))) > 0)",

      "Operator_!=_3": "$value((15 != Int1) == false)",
      "Operator_!=_4": "$value((15 != '15') == true)",

      "Operator_!=_5": "$value((15.00 != Int1) == false)",
      "Operator_!=_6": "$value(15.01 != Int1)",

      "Operator_!=_7": "$value(TrueValue != true == false)",
      "Operator_!=_8": "$value((TrueValue != 'true'))",

      "Operator_!=_9": "$value((FalseValue != false) == false)",
      "Operator_!=_10": "$value(FalseValue != 'false')",

      "Operator_!=_11": "$value((ToDate('2022-05-23') != ToDate('2022-05-23T18:25:43.511Z')) == false)",
      "Operator_!=_12": "$value(ToDate('2022-05-23') != ToDate('2022-06-23T18:25:43.511Z'))",

      "Operator_!=_13": "$value(Text1 != 'TExt1')",
      "Operator_!=_14": "$value(Text1 !='Text1' == false)"
    }
