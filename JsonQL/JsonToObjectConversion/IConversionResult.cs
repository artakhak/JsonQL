// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

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