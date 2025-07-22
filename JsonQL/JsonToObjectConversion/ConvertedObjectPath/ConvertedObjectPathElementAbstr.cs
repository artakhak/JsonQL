// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonToObjectConversion.ConvertedObjectPath;

/// <summary>
/// Represents an abstract base class for elements in a converted object path.
/// This class provides common properties and functionality for objects that are part
/// of a path structure used during the conversion process from JSON to objects.
/// </summary>
public abstract class ConvertedObjectPathElementAbstr : IConvertedObjectPathElement
{
    /// <summary>
    /// Represents an abstract base class for elements in a converted object path structure.
    /// This abstract class provides the foundational properties, such as a name and object type,
    /// required for building and representing an object path during the JSON-to-object conversion process.
    /// </summary>
    protected ConvertedObjectPathElementAbstr(string name, Type objectType)
    {
        Name = name;
        ObjectType = objectType;
    }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public Type ObjectType { get; }
}
