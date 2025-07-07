// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using OROptimizer.Diagnostics.Log;
using TextParser;
using UniversalExpressionParser;

namespace JsonQL.Compilation.UniversalExpressionParserJsonQL;

/// <summary>
/// Represents a factory interface for creating instances of <see cref="IExpressionParser"/>
/// configured for parsing JsonQL-specific expressions and language constructs.
/// </summary>
public interface IJsonQLExpressionParserFactory
{
    /// <summary>
    /// Creates and returns an instance of <see cref="IExpressionParser"/> configured for parsing JsonQL-specific expressions.
    /// </summary>
    /// <returns>An instance of <see cref="IExpressionParser"/> for JsonQL.</returns>
    IExpressionParser Create();
}

/// <inheritdoc />
public class JsonQLExpressionParserFactory : IJsonQLExpressionParserFactory
{
    private readonly IJsonQLExpressionLanguageProvider _jsonQlExpressionLanguageProvider;
    private readonly IJsonQLExpressionLanguageProviderValidator _jsonQlExpressionLanguageProviderValidator;
    private readonly ILog _logger;

    /// <summary>
    /// Factory for creating instances of <see cref="IExpressionParser"/> that are configured to parse JsonQL-specific expressions and syntax.
    /// </summary>
    public JsonQLExpressionParserFactory(IJsonQLExpressionLanguageProvider jsonQLExpressionLanguageProvider,
        IJsonQLExpressionLanguageProviderValidator jsonQLExpressionLanguageProviderValidator,
        ILog logger)
    {
        _jsonQlExpressionLanguageProvider = jsonQLExpressionLanguageProvider;
        _jsonQlExpressionLanguageProviderValidator = jsonQLExpressionLanguageProviderValidator;
        _logger = logger;
    }

    /// <inheritdoc />
    public IExpressionParser Create()
    {
        var expressionLanguageProviderCache =
            new ExpressionLanguageProviderCache(_jsonQlExpressionLanguageProviderValidator);

        expressionLanguageProviderCache.RegisterExpressionLanguageProvider(_jsonQlExpressionLanguageProvider);
        return new ExpressionParser(new TextSymbolsParserFactory(), expressionLanguageProviderCache, _logger);
    }
}