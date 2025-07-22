using JsonQL.Compilation;
using JsonQL.DependencyInjection;
using OROptimizer.Diagnostics.Log;
using OROptimizer.ServiceResolver;
using OROptimizer.ServiceResolver.DefaultImplementationBasedObjectFactory;
using TestsSharedLibrary;
using TestsSharedLibrary.Diagnostics.Log;

namespace JsonQL.Tests;

public abstract class JsonCompilationTestsAbstr
{
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        TestsHelper.SetupLogger(new Log4TestsParameters
        {
            TestContext = new NUnitTestContext()
        });
        Log4Tests.LogLevel = LogLevel.Debug;
    }

   
    [SetUp]
    public virtual void Setup()
    {
        DiBasedObjectFactoryParametersContext.Context = new DiBasedObjectFactoryParameters
        {
            LogDiagnosticsData = true
        };

        JsonQLDefaultImplementationBasedObjectFactory = new JsonQLDefaultImplementationBasedObjectFactory(
            type => true, LogHelper.Context.Log);

        JsonCompiler = JsonQLDefaultImplementationBasedObjectFactory.CreateInstance<IJsonCompiler>();
    }
   
    protected IJsonCompiler JsonCompiler { get; private set; } = null!;
    protected IJsonQLDefaultImplementationBasedObjectFactory JsonQLDefaultImplementationBasedObjectFactory { get; private set; } = null!;
}
