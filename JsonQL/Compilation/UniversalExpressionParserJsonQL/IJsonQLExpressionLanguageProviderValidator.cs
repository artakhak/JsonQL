// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using UniversalExpressionParser;

namespace JsonQL.Compilation.UniversalExpressionParserJsonQL;

/// <summary>
/// Provides an interface to validate the implementation of <see cref="IExpressionLanguageProvider"/>
/// that defines the rules and characteristics of the JsonQL expression language.
/// This validator ensures that the provided JsonQL language definitions adhere to the rules expected by
/// the JsonQL-specific expression parser and the Universal Expression Parser infrastructure.
/// </summary>
public interface IJsonQLExpressionLanguageProviderValidator: IExpressionLanguageProviderValidator
{
}

public class JsonQLExpressionLanguageProviderValidator : DefaultExpressionLanguageProviderValidator, IJsonQLExpressionLanguageProviderValidator
{
    
}