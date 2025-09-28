=============================
Optional and Named Parameters
=============================

.. contents::
   :local:
   :depth: 2

- JsonQL functions (See :doc:`../JsonPathFunctions/index`, :doc:`../AggregateFunctions/index`,  :doc:`../Functions/index`) support both required and optional parameters.
- Optional parameters appear after required parameters (if there are any), and can be omitted, as long as we don't have to pass other optional parameters that follow them.
- If a function has one or more optional parameters, and we want to specify only some of the optional parameter, we can either specify the values of other optional parameters that appear before the optional parameters of interest, or we can use named parameters, and only specify values of optional parameters we care about.
- To use named parameters, operator "->" is used between parameter name and its value (see examples below).
- Using named parameters we can change the order in which arguments are passed.

    - For example, in JsonQL expression in value of "AllParametersSpecifiedAsNamedParameters" in JSON file **Example** below we use named parameters for all three parameters. We pass a value for optional parameter 'value' first (this parameter is the the third parameter in function 'Average'), then we pass a value for optional parameter 'criteria' (this parameter is the the second parameter in function 'Average'), and finally we pass a value for required parameter 'collection' (this parameter is the the first parameter in function 'Average').

Example
=======

.. note:: The following JSON files are referenced in JsonQL expressions in **Example.json** in example below:  :doc:`../../MutatingJsonFiles/SampleFiles/employees`.

**Example.json** below demonstrates using optional and named parameters with JsonQL.


.. sourcecode:: json

    {
      "Comment1": "In example below we pass only a value for required parameter 'collection' ",
      "Comment2": "to function 'Average' as unnamed parameter.",
      "OptionalParametersNotSpecified": "$value(Average(Employees.Select(e => e.Salary)))",

      "Comment3": "In example below we pass only a value for required parameter 'collection' ",
      "Comment4": "and a value for optional parameter 'criteria' to function 'Average' as unnamed parameters.",
      "OneOptionalParameterSpecified": "$value(Average(Employees.Select(e => e.Salary), s => s > 100000))",
      "SomeOptionalParametersSpecified": "$value(Average(Employees, e => e.Salary > 100000))",

      "Comment5": "In example below, a value for required parameter 'collection' is passed as unnamed parameter,",
      "Comment6": "and values for optional parameters 'criteria' and 'value' are passed in order different from default order",
      "Comment7": " as named parameters. The default order is 'collection', 'criteria', and 'value'",
      "MixOfNamedAndUnnamedParameters": "$value(Average(Employees, value -> e => e.Salary, criteria -> e => e.Salary > 100000))",

      "Comment8": "In example below, the values for required parameter 'collection' and optional parameters 'criteria' and 'value'",
      "Comment9": "are passed are passed in order different from default order as named parameters.",
      "Comment10": "The default order is 'collection', 'criteria', and 'value'",
      "AllParametersSpecifiedAsNamedParameters": "$value(Average(value -> e => e.Salary, criteria -> e => e.Salary > 100000, collection -> Employees))"
    }

    
The result (i.e., an instance of `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationResult.cs>`_) is serialized to a **Result.json** file below.

.. note::
    For brevity, the serialized result includes only serialized evaluated **Example.json** and does not include parent JSON files in **JsonQL.Compilation.ICompilationResult.CompiledJsonFiles**
 
.. sourcecode:: json

    {
      "CompiledJsonFiles":[
        {
          "TextIdentifier": "Example",
          "CompiledParsedValue":
          {
            "Comment1":  "In example below we pass only a value for required parameter 'collection' ",
            "Comment2":  "to function 'Average' as unnamed parameter.",
            "OptionalParametersNotSpecified":  96375,
            "Comment3":  "In example below we pass only a value for required parameter 'collection' ",
            "Comment4":  "and a value for optional parameter 'criteria' to function 'Average' as unnamed parameters.",
            "OneOptionalParameterSpecified":  105000,
            "SomeOptionalParametersSpecified":  0,
            "Comment5":  "In example below, a value for required parameter 'collection' is passed as unnamed parameter,",
            "Comment6":  "and values for optional parameters 'criteria' and 'value' are passed in order different from default order",
            "Comment7":  " as named parameters. The default order is 'collection', 'criteria', and 'value'",
            "MixOfNamedAndUnnamedParameters":  105000,
            "Comment8":  "In example below, the values for required parameter 'collection' and optional parameters 'criteria' and 'value'",
            "Comment9":  "are passed are passed in order different from default order as named parameters.",
            "Comment10":  "The default order is 'collection', 'criteria', and 'value'",
            "AllParametersSpecifiedAsNamedParameters":  105000
          }
        }
      ],
      "CompilationErrors":
      {
        "$type": "System.Collections.Generic.List`1[[JsonQL.Compilation.ICompilationErrorItem, JsonQL]], System.Private.CoreLib",
        "$values": []
      }
    }
   
The code snippet shows how the JSON file **Example.json** was parsed using `JsonQL.Compilation.IJsonCompiler <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/IJsonCompiler.cs>`_

.. sourcecode:: csharp

    // Set the value of jsonCompiler to an instance of JsonQL.Compilation.IJsonCompiler here.
    // The value of JsonQL.Compilation.JsonCompiler is normally created by Dependency Injection container 
    // and it is normally configured as a singleton.
    JsonQL.Compilation.IJsonCompiler jsonCompiler = null!;

    var sharedExamplesFolderPath = new []
    {
        "DocFiles", "MutatingJsonFiles", "Examples"
    };

    var companiesJsonTextData = new JsonTextData("Companies",
        LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath));

    var result = jsonCompiler.Compile(new JsonTextData("Example",
        this.LoadExampleJsonFile("Example.json"), companiesJsonTextData));