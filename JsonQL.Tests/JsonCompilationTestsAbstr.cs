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
    public virtual void Setup()
    {
        var defaultJsonCompilerFactory = new DefaultJsonCompilerFactory(LogHelper.Context.Log);
        JsonCompiler = defaultJsonCompilerFactory.Create();
    }
   
    protected IJsonCompiler JsonCompiler { get; private set; } = null!;
}