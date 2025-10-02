========
HasField
========

.. contents::
   :local:
   :depth: 2
   
- The function 'HasField' is used to check if a JsonPath expression evaluates to a JSON object that has a given field.

Function Parameters
===================

- **path**:
    - Required: Yes
    - Type: JSON path.
    - Description:  valid json path. Examples: "e.Salary", "e.Address.Street"

- **key**:
    - Required: Yes
    - Type: string
    - Description: JSON field name to check for existence in the object referenced by JSON path in parameter **path**.

Examples
========
    
.. sourcecode:: json

    {
      "HasField_1": "$value(HasFieldData.Array1.Where(x => HasField(x, 'Id')))",
      "HasField_2": "$value(HasFieldData.Array1.Where(x => HasField(x.Address, 'Street')))",
      "HasField_3": "$(HasField(HasFieldData.InvalidPath, 'Street')) is false"
    }