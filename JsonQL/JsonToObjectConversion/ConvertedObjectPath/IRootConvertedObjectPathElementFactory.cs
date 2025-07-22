// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonToObjectConversion.ConvertedObjectPath;

/// Provides a factory for creating instances of IRootConvertedObjectPathElement.
/// This is typically used to generate the root element of a converted object path,
/// based on the specified object type.
public interface IRootConvertedObjectPathElementFactory
{
    /// <summary>
    /// Creates an instance of <see cref="IRootConvertedObjectPathElement"/> for a specified object type.
    /// </summary>
    /// <param name="objectType">The <see cref="Type"/> of the object for which the root converted object path element is to be created.</param>
    /// <returns>An instance of <see cref="IRootConvertedObjectPathElement"/> representing the root element of the converted object path.</returns>
    IRootConvertedObjectPathElement Create(Type objectType);
}

/// <inheritdoc />
public class RootConvertedObjectPathElementFactory : IRootConvertedObjectPathElementFactory
{
    /// <inheritdoc />
    public IRootConvertedObjectPathElement Create(Type objectType)
    {
        return new RootConvertedObjectPathElement(objectType);
    }
}
