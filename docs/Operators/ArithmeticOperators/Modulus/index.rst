===========
Modulus '%'
===========

.. contents::
   :local:
   :depth: 2
   
Binary operator '%' computes the remainder after dividing the first operand by the second operand. 
    
    .. note::
        Both operands are normally expected to be integers, but the operator works with fractional values too.

- If both operands evaluate to valid numeric values, the result will be the remainder after dividing the first evaluated value by the second evaluated value.
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
      "Operator_%_1": "$value((15 % text1) is undefined)",
      "Comment_Operator_%_2": "(e.SalaryInvalid % 2 > 0) is evaluated to false for all employees",
      "Operator_%_2": "$value(Companies.Select(c => c.Employees.Where(e => e.SalaryInvalid % 2 > 0)))",

      "Operator_%_3": "$value(15 % Int1)",
      "Operator_%_4": "$value(15 % 3)",
      "Operator_%_5": "$value(17 % 3)",
      "Operator_%_6": "$value(Companies.Where(c => Count(c.Employees) % 2 == 1).Select(c => c.CompanyData.Name))"
    }
