:orphan:

====================================
JsonQL.Compilation.ICompiledJsonData
====================================

.. contents::
   :local:
   :depth: 2
   
- `JsonQL.Compilation.ICompiledJsonData <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompiledJsonData.cs>`_ stores data on single evaluated JSON file that uses JsonQL expressions (file might not have any JsonQL expressions as well).
- `JsonQL.Compilation.ICompiledJsonData <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompiledJsonData.cs>`_ has properties **TextIdentifier** and **CompiledParsedValue** (among others).
- Property **TextIdentifier** in stores parsed file identifier.
- Property **CompiledParsedValue** is of type `JsonQL.JsonObjects.IRootParsedValue <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IRootParsedValue.cs>`_ and stores the compiled JSON structure after applying all JsonQL expressions.
- See :doc:`../IRootParsedValue/index` to learn more about this interface.