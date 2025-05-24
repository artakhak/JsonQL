using JsonQL.Compilation;
using JsonQL.DependencyInjection;
using OROptimizer.Diagnostics.Log;
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
    public void Setup()
    {
        IDefaultStringFormatterFactory defaultStringFormatterFactory = new DefaultStringFormatterFactory(new DateTimeOperations());
        IDefaultJsonCompilerFactory defaultJsonCompilerFactory = new DefaultJsonCompilerFactory(
            LogHelper.Context.Log, defaultStringFormatterFactory.Create());

        JsonCompiler = defaultJsonCompilerFactory.Create();
    }
   
    protected IJsonCompiler JsonCompiler { get; private set; } = null!;
}