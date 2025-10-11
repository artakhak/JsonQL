==========
Negate '!'
==========

.. contents::
   :local:
   :depth: 2
   
The unary operator '!' is used to perform a logical NOT operation on a boolean expression in JsonQL. It negates the boolean value of its operand.

**Operator Priority**: 100

Operator Operands
=================

- **Operand 1**:    
    - Type: any valid JsonQL expression (including invalid path).
    - Description: Any valid JsonQL expression.
    
- **Operand 2**:    
    - Type: any valid JsonQL expression (including invalid path).
    - Description: Any valid JsonQL expression.

Evaluation Rules
================

The `!` operator follows these evaluation rules:

- **!true**: Returns `false`
- **!false**: Returns `true`
- **Invalid paths**: When the operand is an invalid path, the negation returns `true`
- **Null values**: When the operand is `null`, the negation returns `true`
- **Non-boolean values**: Any non-boolean value (such as integers, strings, etc.) is treated as "not false", so the negation returns `true`

.. note::
    The `!` operator returns `false` only when applied to the boolean value `true`. All other values (including boolean `false`, invalid paths, null values, and non-boolean values) result in `true` when negated.

Examples
========
    
.. sourcecode:: json

    {
      "Comment_Operator_!_1": "(!invalid.path) is evaluated to true",
      "Operator_!_1": "$value(!invalid.path)",

      "Comment_Operator_!_2": "(!NullValue) is evaluated to true",
      "Operator_!_2": "$value(!NullValue)",

      "Comment_Operator_!_3": "(!Int1) is evaluated to true. Any value other than boolean false is evaluated to true",
      "Operator_!_3": "$value(!Int1)",

      "Operator_!_4": "$value(!(Int1 == 15) == false)",
      "Operator_!_5": "$value(!TrueValue == false)",
      "Operator_!_6": "$value(!FalseValue)",

      "Operator_!_7": "$value(Companies.Select(c => c.Employees.Where(e => !(e.Age > 40 || e.Salary > 100000))).First())"
    }
