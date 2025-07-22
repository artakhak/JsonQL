// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Defines the settings used for JSON to object conversion.
/// </summary>
public interface IJsonConversionSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether the JSON conversion process should stop at the first error encountered.
    /// If set to true, the conversion will halt immediately upon encountering an error.
    /// If set to false, the conversion will continue, attempting to process as much as possible.
    /// </summary>
    bool FailOnFirstError { get; }

    /// <summary>
    /// Gets or sets the naming convention format applied to JSON property names during JSON to object conversion.
    /// This controls whether the property names are converted to camelCase or PascalCase.
    /// </summary>
    JsonPropertyFormat JsonPropertyFormat { get; }

    /// <summary>
    /// Gets or sets a delegate used to attempt mapping the default type to a specific type during the JSON conversion process.
    /// This delegate allows for custom logic to dynamically determine the appropriate type for converting parsed JSON data.
    /// </summary>
    TryMapTypeDelegate? TryMapJsonConversionType { get; }

    /// <summary>
    /// Gets the collection of configurations that define how different types of conversion errors
    /// should be handled during the JSON to object conversion process. Each configuration specifies
    /// the error type and its reporting behavior, allowing customization of error handling strategies.
    /// </summary>
    IReadOnlyList<IConversionErrorTypeConfiguration> ConversionErrorTypeConfigurations { get; }
}

/// <inheritdoc />
public class JsonConversionSettings : IJsonConversionSettings
{
    /// <inheritdoc />
    public JsonPropertyFormat JsonPropertyFormat { get; set; } = JsonPropertyFormat.PascalCase;

    /// <inheritdoc />
    public TryMapTypeDelegate? TryMapJsonConversionType { get; set; }

    /// <inheritdoc />
    public IReadOnlyList<IConversionErrorTypeConfiguration> ConversionErrorTypeConfigurations { get; set; } = Array.Empty<IConversionErrorTypeConfiguration>();

    /// <inheritdoc />
    public bool FailOnFirstError { get; set; } = true;
}
