using Autofac;
using JsonQL.Extensions.JsonToObjectConversion;
using JsonQL.Extensions.JsonToObjectConversion.NullabilityCheck;
using JsonQL.Extensions.JsonToObjectConversion.Serializers;
using JsonQL.Extensions.Query;

namespace JsonQL.Demos.Startup.DependencyInjection;

public class JsonQLExtensionsClassesRegistrationsModule: Module
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        //builder.RegisterType<TypeHelpers>().As<ITypeHelpers>().SingleInstance();
        builder.RegisterType<CollectionTypeHelpers>().As<ICollectionTypeHelpers>().SingleInstance();

        builder.RegisterType<QueryManager>().As<IQueryManager>().SingleInstance();
        builder.RegisterType<JsonParsedValueConversionManager>().As<IJsonParsedValueConversionManager>().SingleInstance();

        builder.RegisterType<ValueNullabilityHelpers>().As<IValueNullabilityHelpers>().SingleInstance();

        builder.RegisterType<JsonConversionSettingsWrapperFactory>().As<IJsonConversionSettingsWrapperFactory>().SingleInstance();
        builder.RegisterType<MicrosoftInternalApiBasedNullabilityCheck>().As<IMicrosoftInternalApiBasedNullabilityCheck>().SingleInstance();
        
        SetupSimpleJsonValueSerializer(builder);
        SetupJsonConversionSettings(builder);
    }

    private void SetupSimpleJsonValueSerializer(ContainerBuilder builder)
    {
        builder.RegisterType<TypedDoubleSimpleJsonValueSerializer>().AsSelf().SingleInstance();
        builder.RegisterType<TypedFloatSimpleJsonValueSerializer>().AsSelf().SingleInstance();
        builder.RegisterType<TypedInt16SimpleJsonValueSerializer>().AsSelf().SingleInstance();
        builder.RegisterType<TypedInt32SimpleJsonValueSerializer>().AsSelf().SingleInstance();
        builder.RegisterType<TypedInt64SimpleJsonValueSerializer>().AsSelf().SingleInstance();

        builder.RegisterType<TypedDateTimeSimpleJsonValueSerializer>().AsSelf().SingleInstance();
        builder.RegisterType<TypedBooleanSimpleJsonValueSerializer>().AsSelf().SingleInstance();
        builder.RegisterType<TypedStringSimpleJsonValueSerializer>().AsSelf().SingleInstance();

        builder.Register(context =>
            new AggregateSimpleJsonValueSerializer(new List<ITypedSimpleJsonValueSerializer>
            {
                context.Resolve<TypedDoubleSimpleJsonValueSerializer>(),
                context.Resolve<TypedFloatSimpleJsonValueSerializer>(),
                context.Resolve<TypedInt16SimpleJsonValueSerializer>(),
                context.Resolve<TypedInt32SimpleJsonValueSerializer>(),
                context.Resolve<TypedInt64SimpleJsonValueSerializer>(),
                context.Resolve<TypedDateTimeSimpleJsonValueSerializer>(),
                context.Resolve<TypedBooleanSimpleJsonValueSerializer>(),
                context.Resolve<TypedStringSimpleJsonValueSerializer>()
            })).As<ISimpleJsonValueSerializer>().SingleInstance();
    }

    private void SetupJsonConversionSettings(ContainerBuilder builder)
    {
        List<ConversionErrorTypeConfiguration> conversionErrorTypeConfigurations = new List<ConversionErrorTypeConfiguration>();

        builder.RegisterType<ModelClassMapper>().As<IModelClassMapper>().SingleInstance();
        builder.RegisterType<ModelClassInstanceCreator>().As<IModelClassInstanceCreator>().SingleInstance();

        foreach (var conversionErrorType in Enum.GetValues<ConversionErrorType>())
        {
            // Set custom ErrorReportingType for ConversionErrorType here.
            // We report all errors as ErrorReportingType.ReportAsError by default.
            conversionErrorTypeConfigurations.Add(new ConversionErrorTypeConfiguration(conversionErrorType, ErrorReportingType.ReportAsError));
        }
        
        builder.Register(x => new JsonConversionSettings
        {
            JsonPropertyFormat = JsonPropertyFormat.PascalCase,
            FailOnFirstError = true,
            ConversionErrorTypeConfigurations = conversionErrorTypeConfigurations,

            
            // Set custom interface to implementation mappings here. Default mappings (i.e., IModelClassMapper) will 
            // use try to find an implementation that has the same name space and class name that matches interface name
            // without I. For example for interface JsonQL.Demos.Examples.DataModels.IEmployee implementation  
            // JsonQL.Demos.Examples.DataModels.Employee will be used if it exists.
            TryMapJsonConversionType = null,
        }).As<IJsonConversionSettings>().SingleInstance();
    }
}