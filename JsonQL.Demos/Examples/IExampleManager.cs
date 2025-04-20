using JsonQL.Demos.Examples.QueryExamples;
using JsonQL.Diagnostics;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Demos.Examples;

public interface IExampleManager
{
    Task ExecuteAsync();
}

public abstract class ExampleManagerAbstr : IExampleManager
{
    /// <inheritdoc />
    public async Task ExecuteAsync()
    {
        LogHelper.Context.Log.InfoFormat("------EXECUTING EXAMPLE [{0}]---------------------------", this.GetType().FullName!);
        var result = await GetJsonQlResultAsync();
        
        var serializedResult = SerializeResult(result);

        await ExampleManagerHelpers.SaveResultToApplicationOutputFolderAsync(this, serializedResult);
        var expectedJsonFile = ExampleManagerHelpers.LoadExpectedResultJsonFile(this);

        if (!string.Equals(serializedResult, expectedJsonFile, StringComparison.Ordinal))
            throw new ApplicationException($"The contents of expected result and actual result do not match. Example type: [{GetType()}].");

        LogHelper.Context.Log.InfoFormat("-----------------------------------------");
        LogHelper.Context.Log.InfoFormat("Generated output is in file [{0}].", ExampleManagerHelpers.GetOutputFilePath(this));
        LogHelper.Context.Log.InfoFormat("-----------------------------------------");
    }

    protected abstract Task<object> GetJsonQlResultAsync();

    protected virtual string SerializeResult(object result)
    {
        return ClassSerializerAmbientContext.Context.Serialize(result);
    }
}

