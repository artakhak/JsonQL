// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.Compilation;

/// <summary>
/// Defines the contract for mapping an implementation of <see cref="ICompilationResult"/>.
/// Provides functionality to transform or adapt a given <see cref="ICompilationResult"/> instance
/// to another <see cref="ICompilationResult"/> instance.
/// </summary>
public interface ICompilationResultMapper
{
    /// <summary>
    /// Maps an instance of <see cref="ICompilationResult"/> to another <see cref="ICompilationResult"/> instance.
    /// This method is intended to provide transformation or adaptation of the provided
    /// <see cref="ICompilationResult"/> as required by the implementation.
    /// </summary>
    /// <param name="compiledFileIdentifiers">Compiled file identifiers.</param>
    /// <param name="compilationResult">The input <see cref="ICompilationResult"/> to be mapped or transformed.</param>
    /// <returns>An instance of <see cref="ICompilationResult"/> that represents the mapped or transformed result.</returns>
    ICompilationResult Map(IReadOnlyList<string> compiledFileIdentifiers, ICompilationResult compilationResult);
}

/// <inheritdoc />
public class CompilationResultMapper: ICompilationResultMapper
{
    /// <inheritdoc />
    public ICompilationResult Map(IReadOnlyList<string> compiledFileIdentifiers, ICompilationResult compilationResult)
    {
        return compilationResult;
    }
}
