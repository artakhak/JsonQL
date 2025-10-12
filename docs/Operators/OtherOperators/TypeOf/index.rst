========
'typeof'
========

.. contents::
   :local:
   :depth: 2
   
The `typeof` unary operator is a prefix operator used in JsonQL to determine the type of a value or expression result. It returns a string representation of the type, which can be used for type checking, validation, or conditional logic.

**Operator Priority**: 100

Syntax
======

typeof <expression>

The `typeof` operator is placed before the expression whose type should be determined.

Operator Operands
=================

- **Operand 1**:    
    - Type: any valid JsonQL expression (including invalid path).
    - Description: The expression whose result type should be evaluated

Return Values
=============

The `typeof` operator returns one of the following string values representing the type:

.. list-table::
   :header-rows: 1
   :widths: 20 80

   * - Type String
     - Description
   * - ``"Undefined"``
     - Value is missing or undefined (e.g., non-existent JSON path)
   * - ``"JsonNull"``
     - Value is JSON ``null``
   * - ``"Number"``
     - Numeric value (integer or floating-point)
   * - ``"String"``
     - String value
   * - ``"Boolean"``
     - Boolean value (``true`` or ``false``)
   * - ``"DateTime"``
     - DateTime value (result of DateTime functions)
   * - ``"JsonObject"``
     - JSON object
   * - ``"JsonArray"``
     - JSON array
   * - ``"Collection"``
     - Collection result from functions like ``Where``, ``Select``, etc.

Evaluation Rules
================

The `typeof` operator follows these evaluation rules:

1. **Missing Values**: Returns ``"Undefined"`` for non-existent properties or paths
2. **Null Values**: Returns ``"JsonNull"`` for explicit JSON ``null`` values
3. **Primitive Types**: Returns the appropriate type string for numbers, strings, and booleans
4. **Complex Types**: Distinguishes between ``"JsonArray"`` (arrays in JSON) and ``"Collection"`` (query results)
5. **Function Results**: Returns the type of the evaluated function result, not the function itself
6. **Error Propagation**: If the operand expression has errors, those errors are propagated
7. **Empty Collections**: Returns ``"Undefined"`` for empty collections, not ``"Collection"``

Use Cases
=========

The `typeof` operator is useful for:

- **Type Validation**: Ensuring values have expected types before processing
- **Conditional Logic**: Branching based on value types
- **Debugging**: Understanding what type an expression evaluates to
- **Type Guards**: Checking if a value exists before using it (checking for ``"Undefined"``)
- **Polymorphic Processing**: Handling different value types appropriately

Implementation Details
======================

The `typeof` operator is implemented through the `TypeOfJsonFunctionResultFunction <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/JsonFunction/JsonFunctions/TypeOfJsonFunctionResultFunction.cs>`_ class, which:

- Evaluates the operand expression
- Determines the result type based on the value's characteristics
- Returns the appropriate type string
- Handles special cases like collections, parsed values, and undefined values

Best Practices
==============

- **Use for Type Safety**: Check types before performing type-specific operations
- **Distinguish Null from Undefined**: Use `typeof` to differentiate between ``"JsonNull"`` and ``"Undefined"``
- **Understand Array vs Collection**: Be aware that JSON arrays and query collections have different type strings
- **Combine with Conditionals**: Use `typeof` in conditional expressions to handle different types appropriately
- **Check Function Results**: Use `typeof` to verify that functions return expected types

Notes
=====

- The `typeof` operator always returns a string, never ``null`` or undefined
- String comparisons with type names are case-sensitive (use ``'Number'``, not ``'number'``)
- The operator evaluates the expression first, then determines its type
- For empty collections, `typeof` returns ``"Undefined"``, not ``"Collection"``


Examples
========

**Examples.json** file below demonstrate using `assert` operator

.. note:: The following JSON files are referenced in JsonQL expressions in **Examples.json** in example below:
    
    - :doc:`Examples/data`


.. sourcecode:: json

    {
      "TypeOf_MissingValue": "$value(typeof parent.MissingValue == 'Undefined')",
      "TypeOf_DoubleJsonValue": "$value(typeof parent.Double1 == 'Number')",
      "TypeOf_DoubleFunction": "$value(typeof (parent.Double1 + 10) == 'Number')",
      "TypeOf_NullValueInJsonObject": "$value(typeof parent.NullValue1 == 'JsonNull')",
      "TypeOf_NullValueInArray": "$value(typeof parent.Array1[1] == 'JsonNull')",
      "TypeOf_Array1": "$value(typeof parent.Array1 == 'JsonArray')",
      "TypeOf_CollectionType": "$value(typeof parent.Array1.Where(x => x > 0) == 'Collection')",
      "TypeOf_DateTime1": "$value(typeof parent.DateTime1 == 'String')",
      "TypeOf_DateTimeFunction": "$value(typeof ToDateTime(parent.DateTime1) == 'DateTime')",
      "TypeOf_Boolean_True": "$value(typeof parent.Boolean_True == 'Boolean')",
      "TypeOf_Boolean_False": "$value(typeof parent.Boolean_False == 'Boolean')",
      "TypeOf_BooleanFunction": "$value(typeof (parent.Double1 > 1) == 'Boolean')",
      "TypeOf_String1": "$value(typeof parent.String1 == 'String')",
      "TypeOf_StringFunction": "$value(typeof Concatenate(parent.String1, '_', parent.Array1[0]) == 'String')"
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
            "TypeOf_MissingValue":  true,
            "TypeOf_DoubleJsonValue":  true,
            "TypeOf_DoubleFunction":  true,
            "TypeOf_NullValueInJsonObject":  true,
            "TypeOf_NullValueInArray":  true,
            "TypeOf_Array1":  true,
            "TypeOf_CollectionType":  true,
            "TypeOf_DateTime1":  true,
            "TypeOf_DateTimeFunction":  true,
            "TypeOf_Boolean_True":  true,
            "TypeOf_Boolean_False":  true,
            "TypeOf_BooleanFunction":  true,
            "TypeOf_String1":  true,
            "TypeOf_StringFunction":  true
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

    var dataJsonTextData = new JsonTextData("Data", this.LoadExampleJsonFile("Data.json"));
             
    var result = _jsonCompiler.Compile(new JsonTextData("Examples",
    this.LoadExampleJsonFile("Examples.json"), dataJsonTextData));
