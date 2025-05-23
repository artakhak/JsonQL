using Autofac;
using JsonQL.Compilation;
using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.Compilation.JsonValueTextGenerator.StringFormatters;
using JsonQL.Demos.AppSettings;
using JsonQL.Demos.CustomJsonQL.Compilation;
using JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.Query;
using OROptimizer.Diagnostics.Log;
using OROptimizer.ServiceResolver;

namespace JsonQL.Demos.Startup.DependencyInjection;

// ReSharper disable once InconsistentNaming
public class JsonQLClassRegistrationsModule: Module
{
    
    private readonly ILog _logger;

    public JsonQLClassRegistrationsModule(ILog logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.Register(x => LogHelper.Context.Log).As<ILog>().SingleInstance();

        builder.Register(context => 
            new QueryManagerCompilationResultLogger(new CompilationResultLogger()))
            .As<ICompilationResultLogger>().SingleInstance();

        RegisterCustomOperatorsAndFunctions(builder);

        builder.Register(context =>
        {
            var compilationResultLogger = context.Resolve<ICompilationResultLogger>();

            TryResolveConstructorParameterValueDelegate tryResolveConstructorParameterValue = (constructedObjectType, parameterInfo) =>
            {
                if (parameterInfo.ParameterType == typeof(ICompilationResultLogger))
                    return (true, compilationResultLogger);

                if (parameterInfo.ParameterType == typeof(IBinaryOperatorJsonFunctionFactory))
                    return (true, context.Resolve<IBinaryOperatorJsonFunctionFactory>());

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

    private void RegisterCustomOperatorsAndFunctions(ContainerBuilder builder)
    {
        builder.RegisterType<CustomBinaryOperatorJsonFunctionFactory>().As<IBinaryOperatorJsonFunctionFactory>().SingleInstance();
    }

}