// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonObjects.JsonPath;

/// <summary>
/// Represents an abstract base interface for elements in a JSON path structure.
/// Provides common functionality and contracts for different types of JSON path elements.
/// </summary>
public interface IJsonPathElement
{
    /// <summary>
    /// Determines whether the current object is equal to another object of the same type,
    /// specifically an implementation of <see cref="IJsonPathElement"/>.
    /// </summary>
    /// <param name="jsonPathElement">The <see cref="IJsonPathElement"/> to compare with the current object.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="IJsonPathElement"/> is equal to the current object;
    /// otherwise, <c>false</c>.
    /// </returns>
    bool Equals(IJsonPathElement jsonPathElement);
}