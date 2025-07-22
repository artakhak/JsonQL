// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Represents a contract for a JSON serializer that converts simple JSON values
/// to a specific .NET <see cref="Type"/> and vice versa.
/// </summary>
public interface ITypedSimpleJsonValueSerializer
{
    /// <summary>
    /// Gets the .NET type that the serializer is capable of serializing and deserializing.
    /// </summary>
    /// <remarks>
    /// This property returns the specific type associated with the implementation of the
    /// <see cref="ITypedSimpleJsonValueSerializer"/> interface. Each implementation targets a
    /// specific .NET type, such as <see cref="bool"/>, <see cref="int"/>, <see cref="DateTime"/>, etc.
    /// </remarks>
    Type SerializedType { get; }

    /// <summary>
    /// Attempts to serialize the specified value into a JSON-compatible format, based on the associated type.
    /// </summary>
    /// <param name="value">The value to be serialized.</param>
    /// <param name="serializedValue">
    /// When this method returns, contains the serialized JSON-compatible value if the serialization was successful;
    /// otherwise, <c>null</c>. This parameter is passed uninitialized.
    /// </param>
    /// <returns>
    /// <c>true</c> if the value was successfully serialized; otherwise, <c>false</c>.
    /// </returns>
    bool TrySerialize(object? value, [NotNullWhen(true)] out object? serializedValue);
}
