using JsonQL.Demos.Examples.DataModels;
using JsonQL.JsonToObjectConversion;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.ConversionSettings.SummaryExample;

/// <summary>
/// This is just a C# code to make sure that the code used in setup-conversion-settings.cs.snippet
/// compiles. This is not an example how the <see cref="IJsonConversionSettings"/>.
/// For examples of configuring <see cref="IJsonConversionSettings"/> see examples of how an instance of <see cref="JsonConversionSettings"/>
/// is created in <see cref="DependencyInjection.JsonQLDefaultImplementationBasedObjectFactory"/>.
/// </summary>
public static class ConversionSettingsExample
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

        return new JsonConversionSettings
        {
            JsonPropertyFormat = JsonPropertyFormat.PascalCase,
            FailOnFirstError = true,
            ConversionErrorTypeConfigurations = conversionErrorTypeConfigurations,

            // Set custom interface to implementation mappings here.
            // Default mapping mechanism (i.e., IModelClassMapper) will 
            // try to find an implementation that has the same name space and class
            // name that matches interface name without "I" prefix (if the mapped
            // type defaultTypeToConvertParsedJsonTo is an interface).
            // For example for interface JsonQL.Demos.Examples.DataModels.IEmployee class
            // JsonQL.Demos.Examples.DataModels.Employee will be used if it exists and it
            // implements JsonQL.Demos.Examples.DataModels.IEmployee.
            // If defaultTypeToConvertParsedJsonTo is a class, the default mapping mechanism will
            // use the class itself.
            TryMapJsonConversionType =
                (defaultTypeToConvertParsedJsonTo, convertedParsedJson) =>
            {
                if (defaultTypeToConvertParsedJsonTo == typeof(Employee))
                {
                    if (convertedParsedJson.HasKey(nameof(IManager.Employees)))
                        return typeof(IManager);

                    if (!convertedParsedJson.HasKey(nameof(Employee.Id)))
                        return typeof(EmployeeWithoutId);
                }

                // Returning null will result default mapping mechanism picking a type to use.
                return null;
            }
        };
    }
}