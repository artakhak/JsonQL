:orphan:

===============================
JsonQL.JsonObjects.IParsedValue
===============================

.. contents::
   :local:
   :depth: 2
   
- The interface `JsonQL.JsonObjects.IParsedValue <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IParsedValue.cs>`_ is a base interface for all interfaces used to store parsed JSON values. Here is a list of all interfaces that extend `JsonQL.JsonObjects.IParsedValue <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IParsedValue.cs>`_:
    - `JsonQL.JsonObjects.IRootParsedValue <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IRootParsedValue.cs>`_ - Stores entire parsed JSON file data.
    - `JsonQL.JsonObjects.IRootParsedJson <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IRootParsedJson.cs>`_ - A subclass of `JsonQL.JsonObjects.IRootParsedValue <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IRootParsedValue.cs>`_. Stores entire parsed JSON file data if root object in JSON file is a JSON object enclosed that starts and ends with '{' and '}' braces.
    - `JsonQL.JsonObjects.IRootParsedArrayValue <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IRootParsedArrayValue.cs>`_ - A subclass of `JsonQL.JsonObjects.IRootParsedValue <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IRootParsedValue.cs>`_. Stores entire parsed JSON file data if root object in JSON file is a JSON array object that starts and ends with '[' and ']' braces.
    - `JsonQL.JsonObjects.IParsedJson <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IParsedJson.cs>`_ - Stores parsed JSON object data (i.e., JSON object that starts and ends with '{' and '}' braces).
    - `JsonQL.JsonObjects.IParsedArrayValue <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IParsedArrayValue.cs>`_ - Stores parsed JSON array data (i.e., JSON object that starts and ends with '[' and ']' braces).
    - `JsonQL.JsonObjects.IParsedSimpleValue <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IParsedSimpleValue.cs>`_ - Stores parsed JSON simple value, such a string value, numeric value, boolean value.
    - `JsonQL.JsonObjects.IParsedCalculatedValue <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IParsedCalculatedValue.cs>`_ - Stores calculated value for which a JSON object is created during JsonQL expression evaluation, but for which there is no corresponding JSON object in JSON files loaded. 
        - Any instance of `JsonQL.JsonObjects.IParsedCalculatedValue <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IParsedCalculatedValue.cs>`_ also implements one of the following interfaces:
            - `JsonQL.JsonObjects.IParsedSimpleValue <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IParsedSimpleValue.cs>`_
            - `JsonQL.JsonObjects.IParsedJson <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IParsedJson.cs>`_
            - `JsonQL.JsonObjects.IParsedArrayValue <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IParsedArrayValue.cs>`_
                            
        - For example consider the following two examples demonstrating `JsonQL.JsonObjects.IParsedCalculatedValue <https://github.com/artakhak/JsonQL/blob/main/JsonQL/JsonObjects/IParsedCalculatedValue.cs>`_:
            - Example with **JsonQL.JsonObjects.IParsedCalculatedValue**: {"MyCalculatedValue": "$value(Employees.Select(e => 2 * e.Salary))"}. In this case an instance of **JsonQL.JsonObjects.IParsedCalculatedValue** is created to store the numeric value **2 * e.Salary**. 
            - Example with **JsonQL.JsonObjects.IParsedSimpleValue**: {"MySimpleJsonValue": "$value(Employees.Select(e => e.Salary))"}. In this case an instance of **JsonQL.JsonObjects.IParsedSimpleValue** is created to store the numeric value **e.Salary** which represents a JSON value in one of the loaded JSON files.