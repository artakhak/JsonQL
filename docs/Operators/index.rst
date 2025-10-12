=========
Operators
=========

.. contents::
   :local:
   :depth: 2
   
- Number of operators are supported in JsonQL. All of them are described in sections below.
- New operators can be added or existing ones can be modified by providing custom implementations of some interfaces in dependency injection setup (like everything else in JsonQL).

- JsonQL operators are assigned numeric priorities which determine the order in which operators are applied, if braces are not used to change operator execution order.

  Operators assigned smaller priority values are executed before operators with higher values of priorities.
  
      .. note::
          Operator priorities are set in constructor of `JsonQL.Compilation.UniversalExpressionParserJsonQL.JsonQLExpressionLanguageProvider <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/UniversalExpressionParserJsonQL/IJsonQLExpressionLanguageProvider.cs>`_.
  
  For example arithmetic operators '+' and '-' have a priority 500 and operators '*' and '/' have a priority 400. Because of differences in priorities of these operators, the expression "x + 2*z" will be evaluated to the same value as the expression "x + (2*z)".
  To change the execution order of operators in this examples, braces can be used, as in this expression: "(x + 2) * z".

.. toctree::
   ArithmeticOperators/index.rst
   ComparisonOperators/index.rst
   LogicalOperators/index.rst
   TextMatchingOperators/index.rst
   ValueIsNullOrUndefinedCheckOperators/index.rst
   OtherOperators/index.rst