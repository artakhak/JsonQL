using JsonQL.JsonToObjectConversion;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.ConversionSettings.JsonPropertyFormat.Example;

/// <summary>
/// This is just a C# code to make sure that the code used in setup-conversion-settings.cs.snippet
/// compiles. This is not an example how the <see cref="IJsonConversionSettings"/>.
/// For examples of configuring <see cref="IJsonConversionSettings"/> see examples of how an instance of <see cref="JsonConversionSettings"/>
/// is created in <see cref="DependencyInjection.JsonQLDefaultImplementationBasedObjectFactory"/>.
/// </summary>
public static class ConversionSettingsSetup
{
    private static IJsonConversionSettings CreateConversionSettings()
    {
        var conversionErrorTypeConfigurations = new List<ConversionErrorTypeConfiguration>();
        
        foreach (var conversionErrorType in Enum.GetValues<ConversionErrorType>())
        {
            // Set custom ErrorReportingType for ConversionErrorType here.
            // We report all errors as ErrorReportingType.ReportAsError by default.
            conversionErrorTypeConfigurations.Add(new ConversionErrorTypeConfiguration(conversionErrorType, ErrorReportingType.ReportAsError));
        }
        
        var jsonConversionSettings = new JsonConversionSettings
        {
            JsonPropertyFormat = JsonQL.JsonToObjectConversion.JsonPropertyFormat.PascalCase,
            FailOnFirstError = true,

            // conversionErrorTypeConfigurations was setup above to report all error types as errors.
            ConversionErrorTypeConfigurations = conversionErrorTypeConfigurations,

            TryMapJsonConversionType = null
        };
        
        return jsonConversionSettings;
    }
}