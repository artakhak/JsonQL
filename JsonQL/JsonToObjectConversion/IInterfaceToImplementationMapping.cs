// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Mapping used when de-serializing json objects to model classes.
/// </summary>
public interface IInterfaceToImplementationMapping
{
    /// <summary>
    /// Interface to map.The type is normally expected to be an interface type, but can be abstract or non-abstract class as well.
    /// </summary>
    Type Interface { get; }

    /// <summary>
    /// Mapped implementation.
    /// </summary>
    Type Implementation { get; }
}

/// <inheritdoc />
public class InterfaceToImplementationMapping : IInterfaceToImplementationMapping
{
    public InterfaceToImplementationMapping(Type @interface, Type implementation)
    {
        Interface = @interface;
        Implementation = implementation;
    }

    /// <inheritdoc />
    public Type Interface { get; }

    /// <inheritdoc />
    public Type Implementation { get; }
}
