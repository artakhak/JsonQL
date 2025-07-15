// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.Compilation;

/// <summary>
/// Represents a single error encountered during the compilation of JSON data.
/// Provides contextual information about the error, including its location
/// in the JSON document and a descriptive error message.
/// </summary>
public interface ICompilationErrorItem
{
    /// <summary>
    /// Json text identifier.
    /// </summary>
    string JsonTextIdentifier { get; }

    /// <summary>
    /// Error line info.
    /// </summary>
    IJsonLineInfo? LineInfo { get; }

    /// <summary>
    /// Error message.
    /// </summary>
    string ErrorMessage { get; }
}

/// <inheritdoc />
public class CompilationErrorItem : ICompilationErrorItem
{
    /// <summary>
    /// Represents a compilation error item produced during the compilation process.
    /// </summary>
    public CompilationErrorItem(string jsonTextIdentifier, string errorMessage, IJsonLineInfo? lineInfo)
    {
        JsonTextIdentifier = jsonTextIdentifier;
        ErrorMessage = errorMessage;
        LineInfo = lineInfo;
    }

    /// <inheritdoc />
    public string JsonTextIdentifier { get; }

    /// <inheritdoc />
    public IJsonLineInfo? LineInfo { get; }
    
    /// <inheritdoc />
    public string ErrorMessage { get; }
}