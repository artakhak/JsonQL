using Autofac;
using JsonQL.Compilation;
using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.Compilation.JsonValueTextGenerator.StringFormatters;
using JsonQL.Demos.AppSettings;
using JsonQL.Demos.CustomJsonQL.Compilation;
using JsonQL.JsonToObjectConversion;
using JsonQL.Query;
using Microsoft.Extensions.Configuration;
using OROptimizer.Diagnostics.Log;
using OROptimizer.ServiceResolver;

namespace JsonQL.Demos.Startup.DependencyInjection;

// ReSharper disable once InconsistentNaming
public class JsonQLClassRegistrationsModule: Module
{
    private readonly IConfigurationRoot _configurationRoot;
    private readonly ILog _logger;

    public JsonQLClassRegistrationsModule(IConfigurationRoot configurationRoot, ILog logger)
    {
        _configurationRoot = configurationRoot;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.Register(x => LogHelper.Context.Log).As<ILog>().SingleInstance();

        builder.Register(context => 
            new QueryManagerCompilationResultLogger(new QueryManagerCompilationResultLogger(new CompilationResultLogger())))
            .As<ICompilationResultLogger>().SingleInstance();

        builder.Register(context =>
        {
            var compilationResultLogger = context.Resolve<ICompilationResultLogger>();

            TryResolveConstructorParameterValueDelegate tryResolveConstructorParameterValue = (constructedObjectType, parameterInfo) =>
            {
                if (parameterInfo.ParameterType == typeof(ICompilationResultLogger))
                    return (true, compilationResultLogger);

                return (false, null);
            };

            if (context.Resolve<IAppSettings>().Settings.UseCustomJsonQL)
                return new CustomJsonCompilerFactory(tryResolveConstructorParameterValue, context.Resolve<IStringFormatter>(), null, _logger);

            return new JsonCompilerFactory(tryResolveConstructorParameterValue, context.Resolve<IStringFormatter>(), null, _logger);
        }).As<IJsonCompilerFactory>().SingleInstance();

        builder.Register(context => context.Resolve<IJsonCompilerFactory>().Create()).As<IJsonCompiler>().SingleInstance();

        builder.RegisterType<DateTimeOperations>().As<IDateTimeOperations>().SingleInstance();
        builder.Register(context =>
                new AggregatedStringFormatter(new List<IStringFormatter>
                {
                    new DateTimeToStringFormatter(context.Resolve<IDateTimeOperations>()),
                    new BooleanToStringFormatter(),
                    new DoubleToStringFormatter(),
                    new ObjectToStringFormatter()
                }))
            .As<IStringFormatter>().SingleInstance();
    }
}