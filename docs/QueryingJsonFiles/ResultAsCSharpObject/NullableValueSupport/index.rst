======================
Nullable Value Support
======================

.. contents::
   :local:
   :depth: 2

- JsonQL supports using nullable syntax using '?' with types returned by overloaded methods **JsonQL.Query.IQueryManager.QueryObject<TQueriedObject>(...)** (e.g., **queryManager.QueryObject<IEmployee?[]>(...)**, **queryManager.QueryObject<IReadOnlyList<double?[]>>(...)**) as well as nullable syntax with properties in returned type.
- Nullable syntax can be used with both C# reference types as well as value types (e.g., **System.Double**, **System.Boolean**).
- If the returned type is specified to have non-nullable, and the value is missing, there will be conversion errors of type `JsonQL.JsonToObjectConversion.IConversionError <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IConversionError.cs>`_ in result with value of **ErrorType** equal to one of the following enum values: **ValueNotSet**, **NonNullableCollectionItemValueNotSet**.
- If the property is non-nullable in returned type (or in one of the classes used by return type class), there will be conversion errors of type `JsonQL.JsonToObjectConversion.IConversionError <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonToObjectConversion/IConversionError.cs>`_ in result with value of **ErrorType** equal to one of the following enum values: **ValueNotSet**, **NonNullablePropertyNotSet**.
- Errors related to non-nullable values being null in result are reported as errors by default, however specific errors can be configured to be reported as warnings, or to be ignored (not reported at all) in conversion settings configuration or in setting overrides in calls to **JsonQL.Query.IQueryManager.QueryObject<T>(...)**. See :doc:`../ConversionSettings/index` for more configuring conversion rules.

.. toctree::

   NullableSyntaxInQueryResultType/index.rst
   NullableSyntaxInQueriedObjectProperties/index.rst