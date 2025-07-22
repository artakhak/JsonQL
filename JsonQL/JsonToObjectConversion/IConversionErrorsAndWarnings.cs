// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Provides access to errors and warnings encountered during a conversion operation.
/// </summary>
public interface IConversionErrorsAndWarnings
{
    /// <summary>
    /// Gets the collection of errors encountered during the conversion process.
    /// </summary>
    IConversionErrors ConversionErrors { get; }

    /// <summary>
    /// Gets the collection of warnings encountered during the conversion process.
    /// </summary>
    IConversionErrors ConversionWarnings { get; }
}

/// <inheritdoc />
public class ConversionErrorsAndWarnings: IConversionErrorsAndWarnings
{
    /// <summary>
    /// Represents a class designed to hold instances of errors and warnings
    /// encountered during JSON to object conversion processes.
    /// </summary>
    public ConversionErrorsAndWarnings(IConversionErrors conversionErrors, IConversionErrors conversionWarnings)
    {
        ConversionErrors = conversionErrors;
        ConversionWarnings = conversionWarnings;
    }

    /// <inheritdoc />
    public IConversionErrors ConversionErrors { get; }

    /// <inheritdoc />
    public IConversionErrors ConversionWarnings { get; }
}
