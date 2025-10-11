=======
Or '||'
=======

.. contents::
   :local:
   :depth: 2
   
The binary operator '||' is used to perform a logical OR operation between two boolean expressions in JsonQL. It returns `true` when at least one of the operands evaluates to `true`, and `false` only when both operands evaluate to `false`.

**Operator Priority**: 800

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

The `||` operator follows these evaluation rules:

- **True || True**: Returns `true`
- **True || False**: Returns `true`
- **False || True**: Returns `true`
- **False || False**: Returns `false`
- **Invalid paths**: When an operand is an invalid path, it is treated as `false`, but if the other operand is `true`, the result is `true`
- **Null values**: When an operand is `null`, it is treated as `false`, but if the other operand is `true`, the result is `true`
- **Non-boolean values**: Non-boolean values (like integers, strings) are treated as `false` unless they represent a truthy condition

Short-circuit Evaluation
=========================

The `||` operator uses short-circuit evaluation:

- If the first operand evaluates to `true`, the second operand is not evaluated and the result is `true`
- The second operand is only evaluated when the first operand is `false`

Examples
========
    
.. sourcecode:: json

    {
      "Comment_Operator_||_1": "(invalid.path || Int1==15) is evaluated to true",
      "Operator_||_1": "$value(invalid.path || Int1==15)",

      "Comment_Operator_||_2": "(true || invalid.path) is evaluated to true",
      "Operator_||_2": "$value(true || invalid.path)",

      "Comment_Operator_||_3": "(NullValue || Int1==15) is evaluated to true",
      "Operator_||_3": "$value(NullValue || Int1==15)",

      "Comment_Operator_||_4": "(Int1==15 || NullValue) is evaluated to true",
      "Operator_||_4": "$value(Int1==15 || NullValue)",

      "Comment_Operator_||_5": "(e.InvalidBooleanPath && 1 == 1) is evaluated to true for all employees",
      "Operator_||_5": "$value(Count(Companies.Select(c => c.Employees.Where(e => e.InvalidBooleanPath || 1 == 1))) == 12)",

      "Operator_||_6": "$value(Companies.Select(c => c.Employees.Where(e => e.Age > 40 || e.Salary > 100000)).First())",

      "Operator_||_7": "$value(TrueValue || Int1==15)",
      "Operator_||_8": "$value(FalseValue || Int1==15)",
      "Operator_||_9": "$value(Int1==15 || FalseValue)",
      "Operator_||_10": "$value((Int1==16 || FalseValue) == false)"
    }