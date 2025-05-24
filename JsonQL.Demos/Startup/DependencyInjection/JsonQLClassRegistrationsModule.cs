using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Autofac;
using JsonQL.Compilation;
using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.Demos.CustomJsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.DependencyInjection;
using JsonQL.Query;
using OROptimizer.Diagnostics.Log;
using OROptimizer.ServiceResolver.DefaultImplementationBasedObjectFactory;
using UniversalExpressionParser;
using Module = Autofac.Module;

namespace JsonQL.Demos.Startup.DependencyInjection;

// ReSharper disable once InconsistentNaming
public class JsonQLClassRegistrationsModule : Module
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

        DefaultImplementationBasedObjectFactory? defaultImplementationBasedObjectFactory = null;
        IJsonFunctionFromExpressionParser? jsonFunctionFromExpressionParser = null;

        builder.Register(context =>
        {
            [SuppressMessage("ReSharper", "AccessToModifiedClosure")]
            (bool parameterValueWasResolved, object? resolvedValue) CustomDiResolver(Type type, ParameterInfo parameterInfo)
            {
                if (parameterInfo.ParameterType == typeof(ICompilationResultLogger))
                    return (true, context.Resolve<ICompilationResultLogger>());

                if (parameterInfo.ParameterType == typeof(IJsonQLExpressionLanguageProvider))
                {
                    return (true, new CustomJsonExpressionLanguageProvider(defaultImplementationBasedObjectFactory.GetOrCreateInstance<IExpressionLanguageProvider>()));
                }

                if (parameterInfo.ParameterType == typeof(IStringFormatter))
                {
                    defaultImplementationBasedObjectFactory.GetOrCreateInstance<IDefaultStringFormatterFactory>().Create();

                    /*return (true, new AggregatedStringFormatter(new List<IStringFormatter>
                    {
                        new DateTimeToStringFormatter(defaultImplementationBasedObjectFactory.GetOrCreateInstance<IDateTimeOperations>()),
                        new BooleanToStringFormatter(),
                        new DoubleToStringFormatter(),
                        new ObjectToStringFormatter()
                    }));*/
                }

                if (parameterInfo.ParameterType == typeof(IJsonFunctionFromExpressionParser))
                    return (true, GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));

                if (TryResolveJsonFunctionFactory(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser,
                        parameterInfo.ParameterType, out var jsonFunctionFactory))
                    return (true, jsonFunctionFactory);

                return (false, null);
            };

            defaultImplementationBasedObjectFactory = new DefaultImplementationBasedObjectFactory(_ => { },
                CustomDiResolver, type => true, _logger);

            return defaultImplementationBasedObjectFactory.CreateInstance<IJsonCompiler>();

        }).As<IJsonCompiler>().SingleInstance();
    }

    private static IJsonFunctionFromExpressionParser GetOrCreateJsonFunctionFromExpressionParser(DefaultImplementationBasedObjectFactory? defaultImplementationBasedObjectFactory,
        ref IJsonFunctionFromExpressionParser? jsonFunctionFromExpressionParser)
    {
        if (defaultImplementationBasedObjectFactory == null)
            throw new InvalidOperationException($"The value of [{nameof(defaultImplementationBasedObjectFactory)}] was not set.");

        return jsonFunctionFromExpressionParser ??= defaultImplementationBasedObjectFactory.GetOrCreateInstance<IJsonFunctionFromExpressionParser>();
    }
  
    private static bool TryResolveJsonFunctionFactory(DefaultImplementationBasedObjectFactory? defaultImplementationBasedObjectFactory,
        ref IJsonFunctionFromExpressionParser? jsonFunctionFromExpressionParser,
        Type parameterType, [NotNullWhen(true)] out object? jsonFunctionFactory)
    {
        if (parameterType == typeof(IBracesJsonFunctionFactory))
        {
            var defaultFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                defaultImplementationBasedObjectFactory.CreateInstance<IBracesJsonFunctionFactory>(),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));

            jsonFunctionFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                new CustomBracesJsonFunctionFactory(defaultFactory),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));
            return true;
        }

        if (parameterType == typeof(IBinaryOperatorJsonFunctionFactory))
        {
            var defaultFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                defaultImplementationBasedObjectFactory.CreateInstance<IBinaryOperatorJsonFunctionFactory>(),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));

            jsonFunctionFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                new CustomBinaryOperatorJsonFunctionFactory(defaultFactory),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));
            return true;
        }

        if (parameterType == typeof(IUnaryPrefixOperatorJsonFunctionFactory))
        {
            var defaultFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                defaultImplementationBasedObjectFactory.CreateInstance<IUnaryPrefixOperatorJsonFunctionFactory>(),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));

            jsonFunctionFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                new CustomUnaryPrefixOperatorJsonFunctionFactory(defaultFactory),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));
            return true;
        }

        if (parameterType == typeof(IUnaryPostfixOperatorJsonFunctionFactory))
        {
            var defaultFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                defaultImplementationBasedObjectFactory.CreateInstance<IUnaryPostfixOperatorJsonFunctionFactory>(),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));

            jsonFunctionFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                new CustomUnaryPostfixOperatorJsonFunctionFactory(defaultFactory),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));
            return true;
        }

        if (parameterType == typeof(IJsonValueCollectionItemsSelectorPathElementFactory))
        {
            var defaultFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                defaultImplementationBasedObjectFactory.CreateInstance<IJsonValueCollectionItemsSelectorPathElementFactory>(),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));

            jsonFunctionFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                new CustomJsonValueCollectionItemsSelectorPathElementFactory(defaultFactory),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));
            return true;
        }

        if (parameterType == typeof(ISpecialLiteralJsonFunctionFactory))
        {
            var defaultFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                defaultImplementationBasedObjectFactory.CreateInstance<ISpecialLiteralJsonFunctionFactory>(),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));

            jsonFunctionFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                new CustomSpecialLiteralJsonFunctionFactory(defaultFactory),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));
            return true;
        }

        if (parameterType == typeof(INumericValueJsonFunctionFactory))
        {
            var defaultFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                defaultImplementationBasedObjectFactory.CreateInstance<INumericValueJsonFunctionFactory>(),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));

            jsonFunctionFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                new CustomNumericValueJsonFunctionFactory(defaultFactory),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));
            return true;
        }

        if (parameterType == typeof(IConstantTextJsonFunctionFactory))
        {
            var defaultFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                defaultImplementationBasedObjectFactory.CreateInstance<IConstantTextJsonFunctionFactory>(),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));

            jsonFunctionFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                new CustomConstantTextJsonFunctionFactory(defaultFactory),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));
            return true;
        }

        jsonFunctionFactory = null;
        return false;
    }
}