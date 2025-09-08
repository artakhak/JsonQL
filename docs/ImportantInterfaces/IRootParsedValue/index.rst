:orphan:

===================================
JsonQL.JsonObjects.IRootParsedValue
===================================

.. contents::
   :local:
   :depth: 2


- The interface `JsonQL.JsonObjects.IRootParsedValue <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IRootParsedValue.cs>`_ stores loaded parsed JSON file data and extends interface `JsonQL.JsonObjects.IParsedValue <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IParsedValue.cs>`_.
    .. note::
        See :doc:`../IParsedValue/index` to learn more about this interface.

- An instance of `JsonQL.JsonObjects.IRootParsedValue <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IRootParsedValue.cs>`_ is allways one of the following interfaces:
    - `JsonQL.JsonObjects.IRootParsedJson <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IRootParsedJson.cs>`_: Stores the entire parsed JSON file data if root object in JSON file is a JSON object enclosed that starts and ends with '{' and '}' braces.
    - `JsonQL.JsonObjects.IRootParsedArrayValue <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IRootParsedArrayValue.cs>`_: Stores the entire parsed JSON file data if root object in JSON file is a JSON array object that starts and ends with '[' and ']' braces.