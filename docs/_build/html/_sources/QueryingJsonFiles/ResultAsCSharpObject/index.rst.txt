===================
Result as C# Object
===================
.. contents::
   :local:
   :depth: 2

- The overloaded methods **QueryObject** in interface `JsonQL.Query.IQueryManager <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryManager.cs>`_ and similarly named overloaded extension methods with generic parameter **QueryObject<T>** in `JsonQL.Query.QueryManagerExtensions <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/QueryManagerExtensions.cs>`_ can be used to to query one or more JSON files using a JsonQL query expressions.

.. note::
    - The extension methods with generic parameter **T** are easier to use. The methods in `JsonQL.Query.IQueryManager <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryManager.cs>`_ might be easier to use with reflection.
    - Moving forward the extension methods will be discussed.

- The result is converted to `JsonQL.Query.IObjectQueryResult<T> <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IObjectQueryResult.cs>`_ a C# interface of class specified in generic parameter where **T** is the generic type argument, which implement used in call to **IJsonQL.Query.QueryManager.QueryObject<T>** method.
- The result stores the query result converted to type **T** as well as data about errors encountered during execution of the query.
- The type parameter **T** specified in query method specifies the return object type from query. It can be any class (value of reference type) including collection types.
- If collection type is used for type parameter **T** in in call to **IJsonQL.Query.QueryManager.QueryObject<T>** method, the collection item parameters can be interfaces or classes as well  (value of reference type). 
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

   Examples/index.rst
   ErrorDetails/index.rst
   TypeBinding/index.rst
   NullableValueSupport/index.rst
   ConversionSettings/index.rst
   SampleFiles/index.rst