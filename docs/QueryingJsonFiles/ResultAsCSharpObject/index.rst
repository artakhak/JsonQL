===================
Result as C# Object
===================
.. contents::
   :local:
   :depth: 2

- The interface `JsonQL.Query.IQueryManager <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryManager.cs>`_ and its extensions are used to query one or more JSON files using a JsonQL query expression.
- The result is converted to `JsonQL.Query.IObjectQueryResult<T> <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IObjectQueryResult.cs>`_ a C# interface of class specified in generic parameter.
- The result stores the query result converted to type 'T' as well as data about errors encountered during execution of the query.
- The type parameter 'T' specified in query method specifies the return object type from query. It can be any class (value of reference type) including collection types.
- The type parameter 'T' specified in query is for a collection type, the collection item parameters can be interfaces or classes as well  (value of reference type). 
- Nullable syntax '?' can be specified for return type (including collection item types, if return type is a collection).
- One ore more JSON files can be specified as parameters to be used when looking up JSON values referenced by JsonQL expressions.
- If many JSON files are specified the following rules and techniques are used:
  - Parent/child relationships between JSON files is maintained and parent JSON files are evaluated before child JSON files are evaluated.
  - Lookup of JSON values specified in JsonQL expressions starts in JSON containing the expression first, and then in parent JSON files.

.. note::
    For more examples look at examples at these links:
        - `Successful query examples in JsonQL.Demos project <https://github.com/artakhak/JsonQL/tree/main/JsonQL.Demos/Examples/IQueryManagerExamples/SuccessExamples>`_
        - `Failed query examples in JsonQL.Demos project <https://github.com/artakhak/JsonQL/tree/main/JsonQL.Demos/Examples/IQueryManagerExamples/FailureExamples/ResultAsObject>`_
        - `Examples in JsonQL.Tests project <https://github.com/artakhak/JsonQL/tree/main/JsonQL.Tests/QueryManager/ResultAsObject>`_


.. toctree::

   ErrorDetails/index.rst
   NullableValueSupport/index.rst
   ConversionSettings/index.rst
   SampleFiles/index.rst