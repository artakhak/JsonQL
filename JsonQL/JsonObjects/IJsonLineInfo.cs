// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

namespace JsonQL.JsonObjects;

/// <summary>
/// Provides line and position information for a JSON element within a JSON document.
/// </summary>
public interface IJsonLineInfo
{
    /// <summary>
    /// Gets the current line number.
    /// </summary>
    /// <value>The current line number or 0 if no line information is available (for example, when <see cref="HasLineInfo"/> returns <c>false</c>).</value>
    int LineNumber { get; }

    /// <summary>
    /// Gets the current line position.
    /// </summary>
    /// <value>The current line position or 0 if no line information is available (for example, when <see cref="HasLineInfo"/> returns <c>false</c>).</value>
    int LinePosition { get; }
}