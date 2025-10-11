===========
'ends with'
===========

.. contents::
   :local:
   :depth: 2
   
The binary operator 'ends with' is used to evaluate whether a string ends with a specified suffix in JsonQL expressions. It returns a boolean result (`true` or `false`).
  
    .. note::
        The operator uses case sensitive comparison.

**Operator Priority**: 300

Operator Operands
=================

- **Operand 1**:    
    - Type: any valid JsonQL expression (including invalid path).
    - Description: The string value to be checked (the target string).
    
- **Operand 2**:    
    - Type: any valid JsonQL expression (including invalid path).
    - Description: The suffix string to search for at the end of the target string.

Evaluation Rules
================

The `ends with` operator follows these evaluation rules:

- **Valid string operands**: Returns `true` if the first operand ends with the second operand, `false` otherwise
- **Case sensitivity**: The comparison is case-sensitive (e.g., `'Text123' ends with 'EXT1'` returns `false`)
- **Invalid paths**: When either operand is an invalid path, returns `false`
- **Null values**: When either operand is `null`, returns `false`
- **Non-string values**: The operator expects both operands to be string values

Examples
========
    
.. sourcecode:: json

    {
      "Comment_Operator_ends_with_1": "(invalid.path ends with 'test') is evaluated to false",
      "Operator_ends_with_1": "$value(invalid.path ends with 'test' == false)",

      "Comment_Operator_ends_with_2": "('test' ends with invalid.path) is evaluated to false",
      "Operator_ends_with_2": "$value('test' ends with invalid.path == false)",

      "Comment_Operator_ends_with_3": "(NullValue ends with 'test') is evaluated to false",
      "Operator_ends_with_3": "$value(NullValue ends with 'test' == false)",

      "Comment_Operator_ends_with_4": "('test' ends with NullValue) is evaluated to false",
      "Operator_ends_with_4": "$value('test' ends with NullValue == false)",

      "Operator_ends_with_5": "$value('Text123' ends with '123')",
      "Operator_ends_with_6": "$value('Text12' ends with Text_ext12)",
      "Operator_ends_with_7": "$value(Text1 ends with 'Text1')",
      "Operator_ends_with_8": "$value(Text12 ends with Text_ext12)",
      "Operator_ends_with_9": "$value(Text1 ends with Text12 == false)",

      "Comment_Operator_ends_with_10": "Text matching is case sensitive",
      "Operator_starts_ends_10": "$value('Text1' ends with 'Ext1' == false)"
    }

