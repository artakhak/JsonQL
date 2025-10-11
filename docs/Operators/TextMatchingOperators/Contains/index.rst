==========
'contains'
==========

.. contents::
   :local:
   :depth: 2
   
The binary operator 'contains' is used to evaluate whether a string contains a specified substring in JsonQL expressions. It returns a boolean result (`true` or `false`).
  
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
    - Description: The substring to search for within the target string.

Evaluation Rules
================

The `contains` operator follows these evaluation rules:

- **Valid string operands**: Returns `true` if the first operand contains the second operand anywhere within it, `false` otherwise
- **Case sensitivity**: The comparison is case-sensitive (e.g., `'Text1' contains 'Ext1'` returns `false`)
- **Invalid paths**: When either operand is an invalid path, returns `false`
- **Null values**: When either operand is `null`, returns `false`
- **Non-string values**: The operator expects both operands to be string values

Examples
========
    
.. sourcecode:: json

    {
      "Comment_Operator_contains_1": "(invalid.path contains 'test') is evaluated to false",
      "Operator_contains_1": "$value(invalid.path contains 'test' == false)",

      "Comment_Operator_contains_2": "('test' contains invalid.path) is evaluated to false",
      "Operator_contains_2": "$value('test' contains invalid.path == false)",

      "Comment_Operator_contains_3": "(NullValue contains 'test') is evaluated to false",
      "Operator_contains_3": "$value(NullValue contains 'test' == false)",

      "Comment_Operator_contains_4": "('test' contains NullValue) is evaluated to false",
      "Operator_contains_4": "$value('test' contains NullValue == false)",

      "Operator_contains_5": "$value('Text123' contains 'ext12')",
      "Operator_contains_6": "$value('Text12' contains Text1)",
      "Operator_contains_7": "$value(Text1 contains 'ext')",
      "Operator_contains_8": "$value(Text12 contains Text1)",
      "Operator_contains_9": "$value(Text1 contains Text12 == false)",

      "Comment_Operator_contains_10": "Text matching is case sensitive",
      "Operator_contains_10": "$value('Text1' contains 'Ext1' == false)"
    }
