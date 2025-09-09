===================
Querying JSON Files 
===================

.. contents::
   :local:
   :depth: 2

Section :doc:`../MutatingJsonFiles/index` demonstrated using interface `JsonQL.Compilation.IJsonCompiler <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/IJsonCompiler.cs>`_ to load one or more JSON files that have child/parent relationship with JsonQL expressions (the expressions mutate JSON files to new JSON structure).
JsonQL also has an interface `JsonQL.Query.IQueryManager <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryManager.cs>`_ which can be used to execute queries against one or more JSON files that have child/parent relationship with JsonQL expressions.
    .. note::
        Some of `JsonQL.Query.IQueryManager <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryManager.cs>`_ methods are extension methods in `JsonQL.Query.QueryManagerExtensions <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/QueryManagerExtensions.cs>`_ 

- Section :doc:`ResultAsCSharpObject/index` describes using `JsonQL.Query.IQueryManager <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryManager.cs>`_ to query JSON files and convert the query result to a C# object.
- Section :doc:`ResultAsJsonStructure/index` describes using `JsonQL.Query.IQueryManager <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryManager.cs>`_ to query JSON files and convert the query to JSON structure.


.. note::
    For more examples of querying JSON files look at demo and unit test examples at these links:
        - `Examples in JsonQL.Demos project <https://github.com/artakhak/JsonQL/tree/main/JsonQL.Demos/Examples/IQueryManagerExamples>`_
        - `Examples in JsonQL.Tests project <https://github.com/artakhak/JsonQL/tree/main/JsonQL.Tests/QueryManager>`_

.. toctree::

   ResultAsCSharpObject/index.rst
   ResultAsJsonStructure/index.rst
   ReusingCompiledJsonFiles/index.rst