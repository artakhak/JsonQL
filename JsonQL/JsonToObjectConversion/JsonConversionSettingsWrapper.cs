// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Provides a wrapper for JSON conversion settings and functionality to retrieve
/// configurations for specific conversion error types.
/// </summary>
public interface IJsonConversionSettingsWrapper
{
    /// <summary>
    /// Represents configuration settings for JSON conversion, including options for handling
    /// parsing errors, property formatting, type mapping delegates, and error type configurations.
    /// </summary>
    IJsonConversionSettings JsonConversionSettings { get; }

    /// <summary>
    /// Attempts to retrieve the configuration for a specified conversion error type.
    /// </summary>
    /// <param name="conversionErrorType">The type of conversion error for which the configuration is being requested.</param>
    /// <param name="conversionErrorTypeConfiguration">
    /// When this method returns, contains the configuration for the specified conversion error type if found,
    /// or null if not found. This parameter is passed uninitialized.
    /// </param>
    /// <returns>
    /// <c>true</c> if a configuration for the specified conversion error type was found; otherwise, <c>false</c>.
    /// </returns>
    bool TryGetConversionErrorTypeConfiguration(ConversionErrorType conversionErrorType, [NotNullWhen(true)]out IConversionErrorTypeConfiguration? conversionErrorTypeConfiguration);
}

/// <inheritdoc />
public class JsonConversionSettingsWrapper : IJsonConversionSettingsWrapper
{
    private readonly Dictionary<ConversionErrorType, IConversionErrorTypeConfiguration> _conversionErrorTypeToConfigurationMap = new();

    /// <summary>
    /// Provides a wrapper for <see cref="IJsonConversionSettings"/> to facilitate additional functionality,
    /// such as mapping conversion error types to their respective configurations.
    /// </summary>
    public JsonConversionSettingsWrapper(IJsonConversionSettings jsonConversionSettings)
    {
        JsonConversionSettings = jsonConversionSettings;

        foreach (var conversionErrorTypeConfiguration in jsonConversionSettings.ConversionErrorTypeConfigurations)
        {
            _conversionErrorTypeToConfigurationMap[conversionErrorTypeConfiguration.ConversionErrorType] = conversionErrorTypeConfiguration;
        }
    }

    /// <inheritdoc />
    public IJsonConversionSettings JsonConversionSettings { get; }

    /// <inheritdoc />
    public bool TryGetConversionErrorTypeConfiguration(ConversionErrorType conversionErrorType, [NotNullWhen(true)] out IConversionErrorTypeConfiguration? conversionErrorTypeConfiguration)
    {
        return _conversionErrorTypeToConfigurationMap.TryGetValue(conversionErrorType, out conversionErrorTypeConfiguration);
    }
}

