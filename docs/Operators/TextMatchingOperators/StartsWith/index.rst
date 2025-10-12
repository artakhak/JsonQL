=============
'starts with'
=============

.. contents::
   :local:
   :depth: 2
   
The binary operator 'starts with' is used to evaluate whether a string starts with a specified prefix in JsonQL expressions. It returns a boolean result (`true` or `false`).
    
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
    - Description: The prefix string to search for at the beginning of the target string.

Evaluation Rules
================

The `starts with` operator follows these evaluation rules:

- **Valid string operands**: Returns `true` if the first operand starts with the second operand, `false` otherwise
- **Case sensitivity**: The comparison is case-sensitive (e.g., `'Text123' starts with 'text'` returns `false`)
- **Invalid paths**: When either operand is an invalid path, returns `false`
- **Null values**: When either operand is `null`, returns `false`
- **Non-string values**: The operator expects both operands to be string values


Examples
========
    
.. sourcecode:: json

    {
      "Comment_Operator_starts_with_1": "(invalid.path starts with 'test') is evaluated to false",
      "Operator_starts_with_1": "$value(invalid.path starts with 'test' == false)",

      "Comment_Operator_starts_with_2": "('test' starts with invalid.path) is evaluated to false",
      "Operator_starts_with_2": "$value('test' starts with invalid.path == false)",

      "Comment_Operator_starts_with_3": "(NullValue starts with 'test') is evaluated to false",
      "Operator_starts_with_3": "$value(NullValue starts with 'test' == false)",

      "Comment_Operator_starts_with_4": "('test' starts with NullValue) is evaluated to false",
      "Operator_starts_with_4": "$value('test' starts with NullValue == false)",

      "Operator_starts_with_5": "$value('Text123' starts with 'Text')",
      "Operator_starts_with_6": "$value('Text123' starts with Text1)",
      "Operator_starts_with_7": "$value(Text1 starts with 'Text1')",
      "Operator_starts_with_8": "$value(Text12 starts with Text1)",
      "Operator_starts_with_9": "$value(Text1 starts with Text12 == false)",

      "Comment_Operator_starts_with_10": "Text matching is case sensitive",
      "Operator_starts_with_10": "$value('Text1' starts with 'text' == false)"
    }
