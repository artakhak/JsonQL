using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.Compilation.JsonValueTextGenerator.StringFormatters;
using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using OROptimizer.Diagnostics.Log;
using OROptimizer.ServiceResolver;
using OROptimizer.ServiceResolver.DefaultImplementationBasedObjectFactory;
using TextParser;
using UniversalExpressionParser;

namespace JsonQL.Compilation;

public interface IJsonCompilerFactory
{
    IJsonCompiler Create(Func<IJsonCompilerParameters, IJsonCompilerParameters>? mutateJsonCompilerParameters = null);
}

public class JsonCompilerFactory: IJsonCompilerFactory
{
    private readonly IStringFormatter _stringFormatter;
    private readonly ILog _logger;
    private readonly DefaultImplementationBasedObjectFactory _defaultImplementationBasedObjectFactory;

    /// <summary>
    /// Constructor.<br/>
    /// The method <see cref="Create"/> of this class creates an instance of default implementation of <see cref="IJsonCompiler"/> with possibility of
    /// replacing dependencies using service resolver <param name="tryResolveConstructorParameterValueDelegate"></param>.
    /// All dependencies are resolved using <param name="tryResolveConstructorParameterValueDelegate"></param> first
    /// and then default instance are created, if no custom resolver does not resolve the type. 
    /// </summary>
    /// <remarks>
    /// -Dependencies are resolved recursively using <param name="tryResolveConstructorParameterValueDelegate"></param>.
    /// Therefore, custom implementations
    /// can be provided for any of the dependencies.<br/>
    /// -If a dependency is not resolved using the custom service provider <param name="tryResolveConstructorParameterValueDelegate"></param>
    /// then the dependencies resolved using default implementation are cached and re/used.
    /// User of this method is responsible for caching custom dependency resolutions (for example by configuring
    /// bindings in Autofac). 
    /// </remarks>
    /// <param name="tryResolveConstructorParameterValueDelegate">Service resolver used to provide custom implementations of
    /// interfaces used for parameter types. If type is not resoled, the default implementation will be used.
    /// </param>
    /// <param name="stringFormatter">String formatter. If the value is null, a default formatter <see cref="AggregatedStringFormatter"/>
    /// formatter will be used that uses known formatters, such as <see cref="DoubleToStringFormatter"/>, <see cref="BooleanToStringFormatter"/>, etc.
    /// Otherwise, the value provided in this parameter will be used.
    /// </param>
    /// <param name="resolvedTypeInstanceCanBeCached">
    /// A function that returns true if type can be cached and re-used, and false, otherwise.
    /// The function should return true for most cases, and only for some special types false can be returned, to ensure
    /// new instance of a type is created each time. 
    /// </param>
    /// <param name="logger">Logger. If the value is null, <see cref="LogToConsole"/> will be used.</param>
    /// <exception cref="InvalidOperationException"></exception>
    public JsonCompilerFactory(TryResolveConstructorParameterValueDelegate tryResolveConstructorParameterValueDelegate,
        IStringFormatter? stringFormatter = null,
        Func<Type, bool>? resolvedTypeInstanceCanBeCached = null, ILog? logger = null)
    {
        logger ??= new LogToConsole(LogLevel.Debug);
        _logger = logger;

        IStringFormatter CreateStringFormatter()
        {
            return _stringFormatter ?? throw new InvalidOperationException($"Failed to initialize the value of [{nameof(_stringFormatter)}] of type [{typeof(IStringFormatter).FullName}]");
        }

        (bool parameterValueWasResolved, object resolvedValue) TryResolveConstructorParameterValue(Type type, System.Reflection.ParameterInfo parameterInfo)
        {
            var resolvedValue = tryResolveConstructorParameterValueDelegate(type, parameterInfo);

            if (resolvedValue.parameterValueWasResolved)
                return resolvedValue;

            if (parameterInfo.ParameterType == typeof(ILog))
                return (true, _logger);

            if (parameterInfo.ParameterType == typeof(IExpressionParser))
            {
                return (true, CreateExpressionParser());
            }

            if (parameterInfo.ParameterType == typeof(IStringFormatter))
            {
                return (true, CreateStringFormatter());
            }

            return resolvedValue;
        }

        _defaultImplementationBasedObjectFactory = new DefaultImplementationBasedObjectFactory(resolvedTypeInstanceWasCreated =>
        {
        }, TryResolveConstructorParameterValue,
        (type) =>
        {
            if (resolvedTypeInstanceCanBeCached != null)
                return resolvedTypeInstanceCanBeCached(type);

            //if (type == typeof(IJsonCompilerParameters))
            //    return false;

            return true;
        }, logger);

        _stringFormatter = stringFormatter ?? new AggregatedStringFormatter(CollectionExpressionHelpers.Create<IStringFormatter>(
            new DateTimeToStringFormatter(_defaultImplementationBasedObjectFactory.GetOrCreateInstance<IDateTimeOperations>()),
            new BooleanToStringFormatter(),
            new DoubleToStringFormatter(),
            new ObjectToStringFormatter()
        ));
    }

    private void InitFunctionFactories()
    {
        var jsonFunctionFromExpressionParser = _defaultImplementationBasedObjectFactory.GetOrCreateInstance<IJsonFunctionFromExpressionParser>();
        
        SetJsonFunctionFromExpressionParser(_defaultImplementationBasedObjectFactory.GetOrCreateInstance<IJsonValueCollectionItemsSelectorPathElementFactory>(), jsonFunctionFromExpressionParser);
        SetJsonFunctionFromExpressionParser(_defaultImplementationBasedObjectFactory.GetOrCreateInstance<INumericValueJsonFunctionFactory>(), jsonFunctionFromExpressionParser);
        SetJsonFunctionFromExpressionParser(_defaultImplementationBasedObjectFactory.GetOrCreateInstance<IConstantTextJsonFunctionFactory>(), jsonFunctionFromExpressionParser);
        SetJsonFunctionFromExpressionParser(_defaultImplementationBasedObjectFactory.GetOrCreateInstance<ISpecialLiteralJsonFunctionFactory>(), jsonFunctionFromExpressionParser);
        SetJsonFunctionFromExpressionParser(_defaultImplementationBasedObjectFactory.GetOrCreateInstance<IBracesJsonFunctionFactory>(), jsonFunctionFromExpressionParser);
        SetJsonFunctionFromExpressionParser(_defaultImplementationBasedObjectFactory.GetOrCreateInstance<IOperatorJsonFunctionFactory>(), jsonFunctionFromExpressionParser);
        SetJsonFunctionFromExpressionParser(_defaultImplementationBasedObjectFactory.GetOrCreateInstance<IBinaryOperatorJsonFunctionFactory>(), jsonFunctionFromExpressionParser);
        SetJsonFunctionFromExpressionParser(_defaultImplementationBasedObjectFactory.GetOrCreateInstance<IUnaryPrefixOperatorJsonFunctionFactory>(), jsonFunctionFromExpressionParser);
        SetJsonFunctionFromExpressionParser(_defaultImplementationBasedObjectFactory.GetOrCreateInstance<IUnaryPostfixOperatorJsonFunctionFactory>(), jsonFunctionFromExpressionParser);
    }

    private void SetJsonFunctionFromExpressionParser(object hasJsonFunctionFromExpressionParser, IJsonFunctionFromExpressionParser jsonFunctionFromExpressionParser)
    {
        var propertyInfo = hasJsonFunctionFromExpressionParser.GetType().GetProperties().FirstOrDefault(x => x.CanWrite &&
                                                                                                             x.PropertyType == typeof(IJsonFunctionFromExpressionParser));

        if (propertyInfo == null)
            return;

        propertyInfo.SetValue(hasJsonFunctionFromExpressionParser, jsonFunctionFromExpressionParser);
    }

    /// <summary>
    /// Creates an instance of default implementation of <see cref="IJsonCompiler"/> with possibility of
    /// replacing dependencies 
    /// </summary>
    public IJsonCompiler Create(Func<IJsonCompilerParameters, IJsonCompilerParameters>? mutateJsonCompilerParameters = null)
    {
        // This should not be necessary.
        var jsonCompilerParameters = _defaultImplementationBasedObjectFactory.CreateInstance<IJsonCompilerParameters>();

        var jsonCompiler = CreateJsonCompiler(mutateJsonCompilerParameters?.Invoke(jsonCompilerParameters)?? jsonCompilerParameters);

        InitFunctionFactories();
        return jsonCompiler;
    }

    protected virtual IJsonCompiler CreateJsonCompiler(IJsonCompilerParameters jsonCompilerParameters)
    {
        return new JsonCompiler(jsonCompilerParameters);
    }

    protected virtual (IExpressionLanguageProviderValidator expressionLanguageProviderValidator, IExpressionLanguageProvider expressionLanguageProvider) CreateExpressionParserDependencies()
    {
        return (new DefaultExpressionLanguageProviderValidator(), new JsonExpressionLanguageProvider());
    }

    private IExpressionParser CreateExpressionParser()
    {
        var expressionParserDependencies = CreateExpressionParserDependencies();

        var expressionLanguageProviderCache =
            new ExpressionLanguageProviderCache(expressionParserDependencies.expressionLanguageProviderValidator);

        expressionLanguageProviderCache.RegisterExpressionLanguageProvider(expressionParserDependencies.expressionLanguageProvider);

        return new ExpressionParser(new TextSymbolsParserFactory(), expressionLanguageProviderCache);
    }

    //protected virtual IStringFormatter CreateStringFormatter()
    //{
    //    return _stringFormatter ?? new AggregatedStringFormatter(CollectionExpressionHelpers.Create<IStringFormatter>(
    //        new DateTimeToStringFormatter(_defaultImplementationBasedObjectFactory.GetOrCreateInstance<IDateTimeOperations>()),
    //        new BooleanToStringFormatter(),
    //        new DoubleToStringFormatter(),
    //        new ObjectToStringFormatter()
    //    ));
    //}
}