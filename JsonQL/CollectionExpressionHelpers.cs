// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL;

/// <summary>
/// Temporary helpers for collection expressions that will be removed once we move to .Net 8.
/// </summary>
internal static class CollectionExpressionHelpers
{
    /// <summary>
    /// A temporary helper for creating collections. 
    /// Remove and use collection expressions like [x, y, z] after moving to .Net 8.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    /// <param name="values"></param>
    /// <returns></returns>
    internal static IReadOnlyList<T> Create<T>(params T[] values)
    {
        return new List<T>(values);
    }
}