using System.Reflection;
using OROptimizer.Diagnostics.Log;
using OROptimizer.ServiceResolver;
using OROptimizer.ServiceResolver.DefaultImplementationBasedObjectFactory;

namespace JsonQL.Compilation;

public class JsonParserFactory
{
    private readonly DefaultImplementationBasedObjectFactory _defaultImplementationBasedObjectFactory;

    public JsonParserFactory(TryResolveConstructorParameterValueDelegate tryResolveConstructorParameterValueDelegate,
        Func<Type, bool> resolvedTypeInstanceCanBeCached, ILog? logger = null)
    {
        logger ??= new LogToConsole(LogLevel.Debug);

        (bool parameterValueWasResolved, object resolvedValue) TryResolveConstructorParameterValue(Type type, ParameterInfo parameterInfo)
        {
            var resolvedValue = tryResolveConstructorParameterValueDelegate(type, parameterInfo);

            if (resolvedValue.parameterValueWasResolved)
                return resolvedValue;

            if (parameterInfo.ParameterType == typeof(ILog))
                return (true, logger);

            return resolvedValue;
        }

        _defaultImplementationBasedObjectFactory = new DefaultImplementationBasedObjectFactory(resolvedObject =>
        {
        },
        TryResolveConstructorParameterValue, resolvedTypeInstanceCanBeCached, logger);
    }

    public JsonParser Create()
    {
        return _defaultImplementationBasedObjectFactory.CreateInstance<JsonParser>();
    }
}