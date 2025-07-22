// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Represents an interface for overriding JSON conversion settings, allowing fine-grained control
/// over the behavior of JSON-to-object conversion processes.
/// </summary>
public interface IJsonConversionSettingsOverrides
{
    /// <summary>
    /// If the value is not null, will replace the default configurations in <see cref="IJsonConversionSettings.JsonPropertyFormat"/>
    /// </summary>
    JsonPropertyFormat? JsonPropertyFormat { get; }

    /// <summary>
    /// If the value is not null, will replace the default configurations in <see cref="IJsonConversionSettings.FailOnFirstError"/>
    /// </summary>
    bool? FailOnFirstError { get; }

    /// <summary>
    /// If the value is not null, it will be used first to try to map the default type used for json value conversion.
    /// Otherwise, if it is null, or it does not provide mapping, global mappings will be used (i.e., the value in <see cref="IJsonConversionSettings.TryMapType"/> and
    /// then  <see cref="IModelClassMapper"/>).
    /// </summary>
    TryMapTypeDelegate? TryMapJsonConversionType { get; }

    /// <summary>
    ///
    /// </summary>
    IReadOnlyList<IConversionErrorTypeConfiguration>? ConversionErrorTypeConfigurations { get; }
}

/// <inheritdoc />
public class JsonConversionSettingsOverrides : IJsonConversionSettingsOverrides
{
    /// <inheritdoc />
    public JsonPropertyFormat? JsonPropertyFormat { get; set; }

    /// <inheritdoc />
    public bool? FailOnFirstError { get; set; }

    /// <inheritdoc />
    public TryMapTypeDelegate? TryMapJsonConversionType { get; set; }

    /// <inheritdoc />
    public IReadOnlyList<IConversionErrorTypeConfiguration>? ConversionErrorTypeConfigurations { get; set; } = null;
}
