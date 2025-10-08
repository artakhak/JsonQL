============
Multiply '*'
============

.. contents::
   :local:
   :depth: 2
   
Binary operator '/' is used to multiply two numeric values (integer or fractional). 

- If both operands evaluate to valid numeric values, the result will be the multiplication of evaluated values.
- Otherwise the result will be **undefined**.

**Operator Priority**: 400

Operator Operands
=================

- **Operand 1**:    
    - Type: any valid JsonQL expression (including invalid path).
    - Description: Any valid JsonQL expression.
    
- **Operand 2**:    
    - Type: any valid JsonQL expression (including invalid path).
    - Description: Any valid JsonQL expression.
    
Examples
========
    
.. sourcecode:: json

    {
      "Operator_*_1": "$value((15 * text1) is undefined)",
      "Comment_Operator_*_2": "(e.SalaryInvalid * 1.1 > 0) is evaluated to false for all employees",
      "Operator_*_2": "$value(Companies.Select(c => c.Employees.Where(e => e.SalaryInvalid * 1.1 > 0)))",
      "Operator_*_3": "$value(35 * Int1)",
      "Operator_*_4": "$value(Int1 * Int2)",
      "Operator_*_5": "$value(Double1 * Double2)",
      "Operator_*_6": "$value(Int1 * Double1)",
      "Operator_*_7": "$value(NegativeInt1 * Double1)"
    }
