=================
Default Value ':'
=================

.. contents::
   :local:
   :depth: 2
   
The default value operator ``:`` is a binary operator used in JsonQL to provide fallback values when the primary expression evaluates to null, undefined, or an invalid value. It enables graceful handling of missing data by substituting a default value when needed.

**Operator Priority**: 200

Syntax
======

<expression> : <default_expression>

The ``:`` operator is placed between the main expression and the default value expression.


Operator Operands
=================

- **Operand 1**:    
    - Type: any valid JsonQL expression (including invalid path).
    - Description: The primary expression to evaluate. If this evaluates to a valid value, it is returned.
    
- **Operand 2**:    
    - Type: any valid JsonQL expression (including invalid path).
    - Description: The fallback expression to evaluate. This value is returned if the left operand is null, undefined, or invalid.


Evaluation Rules
================

The ``:`` operator follows these evaluation rules:

1. **Valid Main Value**: If the left operand evaluates to a valid, non-null, and defined value, that value is returned
2. **Invalid Main Value**: If the left operand is null, undefined, or invalid (e.g., non-existent JSON path), the right operand is evaluated and returned
3. **Type Preservation**: The operator can work with any type (string, number, boolean, DateTime, arrays, objects)
4. **Type Matching**: Both operands should ideally be of compatible types, though the operator doesn't enforce strict type matching
5. **Error Propagation**: If either operand has evaluation errors, those errors are propagated
6. **Nested Defaults**: Default operators can be chained for multiple fallback levels

Return Value
============

- Returns the value from the **left operand** if it is valid
- Returns the value from the **right operand** if the left operand is invalid, null, or undefined
- The return type matches the type of whichever operand is returned

Use Cases
=========

The default value operator is useful for:

- **Handling Missing Properties**: Providing fallback values for optional JSON properties
- **Data Migration**: Supplying default values when working with incomplete or legacy data
- **Graceful Degradation**: Ensuring expressions don't fail due to missing data
- **Configuration Defaults**: Specifying fallback configuration values
- **Array/Object Defaults**: Providing default complex structures when paths don't exist

Implementation Details
======================

The ``:`` operator is implemented through the `DefaultValueOperatorFunction <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/JsonFunction/JsonFunctions/DefaultValueOperatorFunctions/DefaultValueOperatorFunction.cs>`_ class, which:

- Evaluates the main value expression first
- Checks if the result is valid and can be converted to a comparable value
- If invalid, evaluates and returns the default value expression
- Handles type conversion for both operands

Best Practices
==============

- **Use for Optional Fields**: Apply the operator to handle optional properties that may not exist in all data
- **Provide Sensible Defaults**: Choose default values that make semantic sense for your application
- **Avoid Complex Defaults**: Keep default expressions simple for better readability
- **Consider Type Compatibility**: Ensure main and default values are of compatible types
- **Chain Carefully**: When chaining multiple defaults, ensure the logic is clear and maintainable

Notes
=====

- The default value operator does **not** cause compilation errors when the main value is missing
- Unlike the ``assert`` operator, it provides graceful fallback rather than failing fast
- The operator works with all JsonQL types including primitives, arrays, and objects
- Both operands are expressions and can include function calls, calculations, or complex paths

Examples
========

**Examples.json** file below demonstrate using `assert` operator

.. sourcecode:: json

    {
      "Object1": {
        "DateTime1": "2022-05-23T18:25:43.511Z",
        "Number1": 15.39,
        "TrueText": "true",
        "TrueValue": true,
        "FalseText": "false",
        "FalseValue": false

      },
      "Array1": [
        "x",
        "2022-05-23T18:25:43.511Z",
        5,
        [ 1, 2, 3 ],
        6,
        "2022-05-23T18:26:43.511Z",
        true,
        "true",
        false,
        "false"
      ],
      
      "DefaultValueOfValidValueIsOriginalValue_DateTime": "$value(ToDateTime(Object1.DateTime1):ToDateTime('2000-01-01T00:00:00.000Z') == ToDateTime(Object1.DateTime1))",
      "DefaultValueOfInvalidValueIsDefaultValue_DateTime1": "$value(ToDateTime(Object1.DateTime1_Invalid):ToDateTime(Object1.DateTime1) == ToDateTime(Object1.DateTime1))",
      "DefaultValueOfInvalidValueIsDefaultValue_DateTime2": "$value(Object1.DateTime1_Invalid:ToDateTime(Object1.DateTime1) == ToDateTime(Object1.DateTime1))",

      "DefaultValueOfValidValueIsOriginalValue_Double": "$value(Object1.Number1:17.26 == Object1.Number1)",
      "DefaultValueOfInvalidValueIsDefaultValue_Double1": "$value(ToDouble(Object1.Number1_Invalid):Object1.Number1 == Object1.Number1)",
      "DefaultValueOfInvalidValueIsDefaultValue_Double2": "$value(Object1.Number1_Invalid:Object1.Number1 == Object1.Number1)",

      "DefaultValueOfValidValueIsOriginalValue_Boolean": "$value(Object1.TrueValue:false == true)",
      "DefaultValueOfInvalidValueIsDefaultValue_Boolean1": "$value(ToBoolean(Object1.TrueValue_Invalid):ToBoolean(Object1.TrueText) == true)",
      "DefaultValueOfInvalidValueIsDefaultValue_Boolean2": "$value(Object1.TrueValue_Invalid:ToBoolean(Object1.TrueText) == true)",

      "DefaultValueOfValidValueIsOriginalValue_String": "$value(Array1[0]:'y' == 'x')",
      "DefaultValueOfInvalidValueIsDefaultValue_String": "$value(Array1_Invalid[0]:'y' == 'y')",

      "DefaultValueOfAnyType": "$value(Array1_Invalid[0]:Array1[0] == 'x')",

      "Comment_ArrayInitializedFromDefaultArray": "NonExistentArray:Array1[3] defaults to Array1[3] which is an array [1, 2, 3]",
      "ArrayInitializedFromDefaultArray": "$value(NonExistentArray:Array1[3])"
    }
    
The result (i.e., an instance of `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationResult.cs>`_) is serialized to a **Result.json** file below.

.. note::
    For brevity, the serialized result includes only serialized evaluated **Examples.json** and does not include parent JSON files in **JsonQL.Compilation.ICompilationResult.CompiledJsonFiles**

.. raw:: html

   <details>
   <summary>Click to expand the result in instance of <b>JsonQL.Compilation.ICompilationResult</b> serialized into <b>Result.json</b></summary>

.. code-block:: json

    {
      "CompiledJsonFiles":[
        {
          "TextIdentifier": "Examples",
          "CompiledParsedValue":
          {
            "Object1": {
              "DateTime1":  "2022-05-23T18:25:43.511Z",
              "Number1":  15.39,
              "TrueText":  "true",
              "TrueValue":  true,
              "FalseText":  "false",
              "FalseValue":  false
            },
            "Array1": [
              "x",
              "2022-05-23T18:25:43.511Z",
              5,
              [
                1,
                2,
                3
              ],
              6,
              "2022-05-23T18:26:43.511Z",
              true,
              "true",
              false,
              "false"
            ],
            "DefaultValueOfValidValueIsOriginalValue_DateTime":  true,
            "DefaultValueOfInvalidValueIsDefaultValue_DateTime1":  true,
            "DefaultValueOfInvalidValueIsDefaultValue_DateTime2":  true,
            "DefaultValueOfValidValueIsOriginalValue_Double":  true,
            "DefaultValueOfInvalidValueIsDefaultValue_Double1":  true,
            "DefaultValueOfInvalidValueIsDefaultValue_Double2":  true,
            "DefaultValueOfValidValueIsOriginalValue_Boolean":  true,
            "DefaultValueOfInvalidValueIsDefaultValue_Boolean1":  true,
            "DefaultValueOfInvalidValueIsDefaultValue_Boolean2":  true,
            "DefaultValueOfValidValueIsOriginalValue_String":  true,
            "DefaultValueOfInvalidValueIsDefaultValue_String":  true,
            "DefaultValueOfAnyType":  true,
            "Comment_ArrayInitializedFromDefaultArray":  "NonExistentArray:Array1[3] defaults to Array1[3] which is an array [1, 2, 3]",
            "ArrayInitializedFromDefaultArray": [
              1,
              2,
              3
            ]
          }
        }
      ],
      "CompilationErrors":
      {
        "$type": "System.Collections.Generic.List`1[[JsonQL.Compilation.ICompilationErrorItem, JsonQL]], System.Private.CoreLib",
        "$values": []
      }
    }

.. raw:: html

   </details><br/><br/>
   
   
The code snippet shows how the JSON file **Examples.json** was parsed using `JsonQL.Compilation.IJsonCompiler <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/IJsonCompiler.cs>`_

.. sourcecode:: csharp

    // Set the value of jsonCompiler to an instance of JsonQL.Compilation.IJsonCompiler here.
    // The value of JsonQL.Compilation.JsonCompiler is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.
    JsonQL.Compilation.IJsonCompiler jsonCompiler = null!;

    var result = jsonCompiler.Compile(new JsonTextData("Examples", this.LoadExampleJsonFile("Examples.json")));
