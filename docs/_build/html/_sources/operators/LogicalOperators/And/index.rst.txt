========
And '&&'
========

.. contents::
   :local:
   :depth: 2
   
The binary operator '&&' is used to perform a logical AND operation between two boolean expressions in JsonQL. It returns `true` when both operands evaluate to `true`, and `false` otherwise.

**Operator Priority**: 700

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

The `&&` operator follows these evaluation rules:

- **True && True**: Returns `true` when both operands evaluate to `true`
- **True && False**: Returns `false`
- **False && True**: Returns `false`
- **False && False**: Returns `false`
- **Invalid paths**: When an operand is an invalid path, it is treated as `false`
- **Null values**: When an operand is `null`, it is treated as `false`
- **Non-boolean values**: Non-boolean values (like integers, strings) are treated as `false`

Short-circuit Evaluation
=========================

The `&&` operator uses short-circuit evaluation:

- If the first operand evaluates to `false`, the second operand is not evaluated and the result is `false`
- Both operands are only evaluated when the first operand is `true`

Examples
========
    
.. sourcecode:: json

    {
      "Comment_Operator_&&_1": "(invalid.path && Int1==15) is evaluated to false",
      "Operator_&&_1": "$value((invalid.path && Int1==15) == false)",

      "Comment_Operator_&&_2": "(true && invalid.path) is evaluated to false",
      "Operator_&&_2": "$value((true && invalid.path) == false)",

      "Comment_Operator_&&_3": "(NullValue && Int1==15) is evaluated to false",
      "Operator_&&_3": "$value((NullValue && Int1==15) == false)",

      "Comment_Operator_&&_4": "(Int1==15 && NullValue) is evaluated to false",
      "Operator_&&_4": "$value((Int1==15 && NullValue) == false)",

      "Comment_Operator_&&_5": "(e.InvalidBooleanPath && 1 == 1) is evaluated to false for all employees",
      "Operator_&&_5": "$value(Count(Companies.Select(c => c.Employees.Where(e => e.InvalidBooleanPath && 1 == 1))) == 0)",

      "Operator_&&_6": "$value(Companies.Select(c => c.Employees.Where(e => e.Age > 40 && e.Salary > 100000)).First())",

      "Operator_&&_7": "$value((TrueValue && Int1==15) == true)",
      "Operator_&&_8": "$value((FalseValue && Int1==15) == false)",
      "Operator_&&_9": "$value((Int1==15 && FalseValue) == false)",
      "Operator_&&_10": "$value((Int1==16 && FalseValue) == false)"
    }

