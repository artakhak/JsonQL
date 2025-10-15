========
Overview
========

.. contents::
   :local:
   :depth: 2
   
JsonQL can be extended in several ways:

1. **Custom Functions**: Add new functions like ``MyCustomFunction()``
2. **Custom Aggregate Functions**: Add new aggregate functions like: ``CountEvenIndexes(collection, predicate)``
3. **Custom Path Functions**: Create collection operators like ``Where`` or ``Select``
4. **Custom Operators**: Define new operators: binary, unary prefix and unary postfix (e.g., ``~=`` for regex matching)
5. **Custom Interface Implementations**: Override default C# type implementations for JSON-to-object conversion
6. **Custom Expression Language**: Modify the JsonQL language itself

Best Practices
==============

1. **Error Handling**: Always return proper ``IParseResult`` with errors when validation fails
2. **Line Information**: Pass ``IJsonLineInfo`` through for accurate error reporting
3. **Type Safety**: Use appropriate base classes (``StringJsonFunctionAbstr``, ``DateTimeJsonFunctionAbstr``, etc.)
4. **Null Handling**: Check for null values and handle appropriately
5. **Parameter Validation**: Validate function parameters before use
6. **Documentation**: Document your custom functions with XML comments
7. **Testing**: Write unit tests for custom extensions
8. **Performance**: Consider caching expensive operations
9. **Thread Safety**: Ensure custom implementations are thread-safe
10. **Naming**: Use clear, descriptive names that don't conflict with built-in functions

Common Patterns
===============

.. note::
    Couple of common patterns are demonstrated here, however this does not cover all patterns for extensions. Look at source code to see how functions, operators are implemented in JsonQL to understand how extensions can be done, as well as look at extension examples in namespace `JsonQL.Demos.CustomJsonQL.Compilation <https://github.com/artakhak/JsonQL/tree/main/JsonQL.Demos/CustomJsonQL/Compilation>`_.

**Pattern 1: Simple Value Function**

.. code-block:: csharp

    public class MyValueFunction : StringJsonFunctionAbstr
    {
        public override IParseResult<string?> EvaluateStringValue(...)
        {
            return new ParseResult<string?>("my value");
        }
    }

**Pattern 2: Function with Parameters**

.. code-block:: csharp

    public class MyParameterizedFunction : StringJsonFunctionAbstr
    {
        private readonly IJsonFunction _parameter1;
        private readonly IJsonFunction _parameter2;

        public MyParameterizedFunction(
            string functionName,
            IJsonFunction parameter1,
            IJsonFunction parameter2,
            IJsonFunctionValueEvaluationContext context,
            IJsonLineInfo? lineInfo)
            : base(functionName, context, lineInfo)
        {
            _parameter1 = parameter1;
            _parameter2 = parameter2;
        }

        public override IParseResult<string?> EvaluateStringValue(...)
        {
            var param1Result = _parameter1.EvaluateValue(...);
            var param2Result = _parameter2.EvaluateValue(...);
            
            // Process and return result
        }
    }

**Pattern 3: Collection Path Function**

.. code-block:: csharp

    public class MyCollectionFunction 
        : JsonValueCollectionItemsSelectorPathElementAbstr
    {
        protected override IParseResult<ICollectionJsonValuePathLookupResult>
            SelectCollectionItems(
                IReadOnlyList<IParsedValue> parentParsedValues,
                IRootParsedValue rootParsedValue,
                IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
        {
            var filteredValues = new List<IParsedValue>();
            
            // Process collection
            foreach (var value in parentParsedValues)
            {
                // Apply logic
                if (ShouldInclude(value))
                    filteredValues.Add(value);
            }

            return new ParseResult<ICollectionJsonValuePathLookupResult>(
                new CollectionJsonValuePathLookupResult(filteredValues));
        }
    }

Troubleshooting
===============

**Issue: Custom Function Not Recognized**

- Verify function name in factory's switch statement
- Ensure factory is registered in DI container
- Check that custom parameter resolver is registered

**Issue: Type Conversion Errors**

- Use ``JsonFunctionHelpers.TryConvertValueToJsonComparable``
- Check TypeCode parameter matches expected type
- Validate input before conversion

**Issue: Null Reference Exceptions**

- Check for null in ``EvaluateValue`` results
- Handle ``IJsonValuePathLookupResult`` with ``HasValue == false``
- Validate all function parameters

See Also
========

- :doc:`../DependencyInjectionSetup/index`: Setting up custom DI modules
- `JsonQL GitHub Repository <https://github.com/artakhak/JsonQL>`_: Source code and examples
- `Custom JsonQL Implementation Examples <https://github.com/artakhak/JsonQL/tree/main/JsonQL.Demos/CustomJsonQL/Compilation>`_