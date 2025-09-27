// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

namespace JsonQL.Compilation;

/// <summary>
/// Represents an interface for compiling JSON text into an executable or analyzable structure.
/// </summary>
public interface IJsonCompiler
{
    /// <summary>
    /// Compiles the JSON provided in <paramref name="jsonTextData"/> and produces a result that includes the compiled JSON and any errors encountered during the process.
    /// Compilation is performed hierarchically from parent JSON files to the specified JSON, resolving any references to parent JSON during the process.
    /// </summary>
    /// <remarks>Use this overload if JSON texts in <paramref name="jsonTextData"/> either have no parents,
    /// or if the parents in <see cref="IJsonTextData.ParentJsonTextData"/> are used only once.
    /// If JSON texts are used multiple times, use the over overloaded method instead.
    /// </remarks>
    /// <param name="jsonTextData">An object containing the JSON text and metadata for compilation, including references to parent JSON data.</param>
    /// <returns>
    /// An <see cref="ICompilationResult"/> containing the output of the compilation.<br/>
    /// The result includes the compiled JSON files in order, starting with parent JSON files followed by child JSON files,<br/>
    /// as well as any errors that occurred during the compilation.<br/>
    /// Since in the presence of compilation errors some files might not be in <see cref="ICompilationResult.CompiledJsonFiles"/>,<br/>
    /// a compiled file can be looked up by using <see cref="ICompiledJsonData.TextIdentifier"/>.<br/>
    /// Example: [var compiledFile=result.CompiledJsonFiles.FirstOrDefault(x=> x.TextIdentifier=="myJsonTextIdentifier")].
    /// </returns>
    ICompilationResult Compile(IJsonTextData jsonTextData);

    /// <summary>
    /// Compiles JSON with expressions in <paramref name="jsonText"/> and returns the results in <see cref="ICompilationResult"/>.
    /// JsonQL expressions in <paramref name="jsonText"/> are resolved by looking up objects in the following order:
    /// 1. Objects within <paramref name="jsonText"/> itself.
    /// 2. Objects in JSON files within <paramref name="compiledParents"/>, searched in the order they appear in the list.
    /// </summary>
    /// <remarks>
    /// Use this overload if the same JSON texts are compiled multiple times.
    /// In these scenarios it is more efficient to compile the JSON texts once using <see cref="Compile(JsonQL.Compilation.IJsonTextData)"/> method,
    /// and then re-use the compiled files.
    /// </remarks>
    /// <param name="jsonText">The JSON text to compile, which includes ma JsonQL expressions.</param>
    /// <param name="jsonTextIdentifier">A unique identifier for the JSON text. Used to tag errors in the compilation result.</param>
    /// <param name="compiledParents">
    /// A list of already compiled JSON data providing parent objects for resolving JsonQL expressions.
    /// Json objects will be looked up first in <paramref name="jsonText"/> with identifier <paramref name="jsonTextIdentifier"/>,
    /// and then in <paramref name="compiledParents"/> starting with parent at index 0, and moving up, if object is not found.
    /// In other words, compiled json that appear earlier in <paramref name="compiledParents"/> have precedence over
    ///  json that appear later in <paramref name="compiledParents"/>.
    /// </param>
    /// <returns>
    /// An <see cref="ICompilationResult"/> containing the output of the compilation.<br/>
    /// The result includes the compiled JSON files in order, starting with parent JSON files followed by child JSON files,<br/>
    /// as well as any errors that occurred during the compilation.<br/>
    /// Since in the presence of compilation errors some files might not be in <see cref="ICompilationResult.CompiledJsonFiles"/>,<br/>
    /// a compiled file can be looked up by using <see cref="ICompiledJsonData.TextIdentifier"/>.<br/>
    /// Example: [var compiledFile=result.CompiledJsonFiles.FirstOrDefault(x=> x.TextIdentifier=="myJsonTextIdentifier")].
    /// </returns>
    ICompilationResult Compile(
        string jsonText, string jsonTextIdentifier, IReadOnlyList<ICompiledJsonData> compiledParents);
}