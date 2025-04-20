using TestsSharedLibrary.Diagnostics.Log;

namespace JsonQL.Tests;

public class NUnitTestContext: ITestContext
{
    public string GetExecutingTestName()
    {
        return TestContext.CurrentContext.Test.FullName;
    }
}