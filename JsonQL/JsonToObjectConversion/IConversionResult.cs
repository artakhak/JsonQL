// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Query;

namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Represents the result of a conversion operation, providing access to the converted value
/// and any errors or warnings encountered during the conversion process.
/// </summary>
public interface IConversionResult<out TValue>
{
    /// <summary>
    /// Gets the value produced by the conversion operation.
    /// </summary>
    /// <remarks>
    /// The value may be null if the conversion was unsuccessful or incomplete, depending on the specific context and implementation.
    /// </remarks>
    TValue? Value { get; }

    /// <summary>
    /// Gets the errors and warnings associated with the conversion process.
    /// </summary>
    /// <remarks>
    /// This property provides detailed information about issues encountered during the conversion, including both errors and warnings.
    /// It can be used to diagnose and handle cases where the conversion process produced unexpected or incomplete results.
    /// </remarks>
    IConversionErrorsAndWarnings ConversionErrorsAndWarnings { get; }
}

/// <inheritdoc />
public class ConversionResult<TValue> : IConversionResult<TValue>
{
    /// <summary>
    /// Represents the result of a conversion operation from JSON to an object of type <typeparamref name="TValue"/>.
    /// This class holds the converted value, if present, along with any associated
    /// conversion errors or warnings. It is typically used to standardize the output
    /// of JSON-to-object conversion processes.
    /// </summary>
    /// <typeparam name="TValue">
    /// The type of the value resulting from the conversion operation.
    /// </typeparam>
    public ConversionResult(TValue? value)
    {
        Value = value;
        ConversionErrorsAndWarnings = EmptyErrors.EmptyConversionErrorsAndWarnings;
    }

    /// <summary>
    /// Represents the result of a conversion operation that produces a value of type <typeparamref name="TValue"/>.
    /// This class encapsulates the converted value and any errors or warnings encountered
    /// during the conversion process.
    /// </summary>
    /// <typeparam name="TValue">
    /// The type of the value to which the conversion operation pertains.
    /// </typeparam>
    public ConversionResult(IConversionErrorsAndWarnings conversionErrorsAndWarnings)
    {
        Value = default;
        ConversionErrorsAndWarnings = conversionErrorsAndWarnings;
    }

    /// <summary>
    /// Represents the result of an attempt to convert a parsed value into a strongly typed object.
    /// This class encapsulates both the converted object and any associated errors or warnings encountered during the conversion process.
    /// </summary>
    /// <typeparam name="TValue">
    /// The type of the converted object resulting from the conversion operation.
    /// </typeparam>
    public ConversionResult(TValue? value, IConversionErrorsAndWarnings conversionErrorsAndWarnings)
    {
        Value = value;
        ConversionErrorsAndWarnings = conversionErrorsAndWarnings;
    }

    /// <inheritdoc />
    public TValue? Value { get; }

    /// <inheritdoc />
    public IConversionErrorsAndWarnings ConversionErrorsAndWarnings { get; }
}