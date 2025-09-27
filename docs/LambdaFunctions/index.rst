================
Lambda Functions
================

.. contents::
   :local:
   :depth: 2

JsonQL uses lambda functions as parameters to functions. Therefore, a simple explanation of lambda expression is given below to help with understanding of some expressions. 
  
A lambda function is a small, anonymous function that you can use to specify behavior or operations directly inline in your code. It is often used for filtering, transforming, or selecting data from collections.

For example:

Employees.Where(x => x.Age > 35) filters the employees to include only those older than 35.
Employees.Select(x => x.Salary) creates a list of salaries from the employees.
In simple terms, it's a quick way to define what should be done with each item in a collection without writing a full separate function.