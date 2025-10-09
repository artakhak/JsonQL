=============
Less Than '<'
=============

.. contents::
   :local:
   :depth: 2
   
The binary operator '<' is used to evaluate whether the first value is less than the second value in JsonQL expressions. It returns a boolean result (`true` or `false`) for valid comparisons, or `undefined` when the comparison cannot be performed.

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

- **Numeric comparisons**: Integer and double values can be compared (e.g., `15 < 16.00` returns `true`)
- **Date/DateTime comparisons**: Both Date and DateTime values can be compared; Date values ignore time components
- **Date with DateTime comparisons**: When comparing Date with DateTime, the Date is treated as having a time component (e.g., `ToDate('2022-06-23T18:25:43.511Z') < ToDateTime('2022-06-23T18:25:43.511Z')` returns `true`)
- **String comparisons**: Strings are compared lexicographically
- **Undefined values**: Any comparison with undefined returns `undefined`
- **Incompatible types**: Comparisons between incompatible types (e.g., number with string, boolean with number) return `undefined`
    
Examples
========
    
.. sourcecode:: json

    {
      "Comment_Operator_<_1": "(15 < invalid.path) is evaluated to undefined",
      "Operator_<_1": "$value((15 < invalid.path) is undefined)",
      "Comment_Operator_<_2": "(e.SalaryInvalid < 100000) is evaluated to undefined for all employees",
      "Operator_<_2": "$value(Count(Companies.Select(c => c.Employees.Where(e => e.SalaryInvalid < 100000))) == 0)",

      "Operator_<_3": "$value(Int1 < 16)",
      "Operator_<_4": "$value((Int1 < 15) == false)",

      "Operator_<_5": "$value(Int1 < 16.00)",
      "Operator_<_6": "$value((Int1 < 14.9) == false)",

      "Operator_<_7": "$value(Double1 < 16)",
      "Operator_<_8": "$value((Double2 < Double1) == false)",

      "Operator_<_9": "$value(ToDate('2022-05-23T18:25:43.511Z') < ToDate('2022-06-23T18:25:43.511Z'))",
      "Operator_<_10": "$value(ToDate('2022-06-23T18:25:43.511Z') < ToDate('2022-05-23T18:25:43.511Z') == false)",

      "Operator_<_11": "$value(ToDateTime('2022-05-23T18:25:43.511Z') < ToDate('2022-06-23T18:25:43.511Z'))",
      "Operator_<_12": "$value(ToDateTime('2022-06-23T18:25:43.511Z') < ToDateTime('2022-05-23T18:25:43.511Z') == false)",

      "Operator_<_13": "$value(ToDate('2022-06-23T18:25:43.511Z') < ToDateTime('2022-06-23T18:25:43.511Z'))",
      "Operator_<_14": "$value(ToDateTime('2022-05-23T18:25:43.511Z') < ToDate('2022-06-23T18:25:43.511Z'))",

      "Operator_<_15": "$value(Text1 < 'Text2')",
      "Operator_<_16": "$value(Text1 < 'Text0' == false)"
    }
