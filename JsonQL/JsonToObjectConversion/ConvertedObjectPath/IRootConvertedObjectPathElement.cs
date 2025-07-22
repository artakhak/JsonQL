// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonToObjectConversion.ConvertedObjectPath;

/// <summary>
/// Defines the root element of a converted object path during the JSON-to-object conversion process.
/// This interface extends <see cref="IConvertedObjectPathElement"/> and provides additional
/// functionality specific to the root element in the object path structure.
/// </summary>
public interface IRootConvertedObjectPathElement: IConvertedObjectPathElement
{
    /// <summary>
    /// Creates a new instance of the root converted object path element with the same object type as the current instance.
    /// </summary>
    /// <returns>
    /// A new instance of <see cref="IRootConvertedObjectPathElement"/> that is a copy of the current instance.
    /// </returns>
    IRootConvertedObjectPathElement Clone();
}

/// <summary>
/// Represents the root element in a converted object path during the JSON-to-object conversion process.
/// Implements <see cref="IRootConvertedObjectPathElement"/> and provides functionality specific
/// to the root node in the object path structure, which serves as the starting point for traversal
/// or manipulation of the converted object path.
/// </summary>
public class RootConvertedObjectPathElement : ConvertedObjectPathElementAbstr, IRootConvertedObjectPathElement
{
    private const string RootObjectName = "Root";

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="objectType">
    /// Object type. See description of <see cref="IConvertedObjectPathElement.ObjectType"/>
    /// </param>
    public RootConvertedObjectPathElement(Type objectType) : base(RootObjectName, objectType)
    {
        
    }

    /// <inheritdoc />
    public IRootConvertedObjectPathElement Clone()
    {
        return new RootConvertedObjectPathElement(this.ObjectType);
    }
}
