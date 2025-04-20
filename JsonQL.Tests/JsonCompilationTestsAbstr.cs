using JsonQL.Compilation;
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
        JsonCompiler = new JsonCompilerFactory(
                (x, y) => (false, null),
                null, x => true,
                LogHelper.Context.Log)
            .Create();
    }
   
    protected IJsonCompiler JsonCompiler { get; private set; } = null!;
}