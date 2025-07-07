// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonToObjectConversion.ConvertedObjectPath;

/// Represents a factory responsible for creating instances of IConvertedObjectPath.
public interface IConvertedObjectPathFactory
{
    /// Creates an instance of IConvertedObjectPath using the specified root converted object path element.
    /// <param name="rootConvertedObjectPathElement">The root element that defines the starting point of the converted object path.</param>
    /// <returns>An instance of IConvertedObjectPath constructed based on the provided root element.</returns>
    IConvertedObjectPath Create(IRootConvertedObjectPathElement rootConvertedObjectPathElement);
}

/// <inheritdoc />
public class ConvertedObjectPathFactory : IConvertedObjectPathFactory
{
    /// <inheritdoc />
    public IConvertedObjectPath Create(IRootConvertedObjectPathElement rootConvertedObjectPathElement)
    {
        return new ConvertedObjectPath(rootConvertedObjectPathElement);
    }
}