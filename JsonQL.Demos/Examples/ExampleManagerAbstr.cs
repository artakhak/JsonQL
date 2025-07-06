using JsonQL.Diagnostics.ResultValidation;
using OROptimizer.Diagnostics.Log;
using System.Reflection;

namespace JsonQL.Demos.Examples;

public abstract class ExampleManagerAbstr : IExampleManager
{
    /// <inheritdoc />
    public abstract bool IsSuccessfulEvaluationExample { get; }

    /// <inheritdoc />
    public async Task ExecuteAsync()
    {
        LogHelper.Context.Log.InfoFormat("------EXECUTING EXAMPLE [{0}]---------------------------", this.GetType().FullName!);
        if (!this.IsSuccessfulEvaluationExample)
            LogHelper.Context.Log.InfoFormat("------------NOTE: THIS IS FAILURE TEST AND ERROR LOGS ARE EXPECTED!!!----------------------");

        try
        {
            await JsonQLResultValidator.ValidateResultAsync(new JsonQLResultValidationParameters
            {
                GetJsonQlResultAsync = this.GetJsonQlResultAsync,
                LoadExpectedResultJsonFileAsync = () => Task.FromResult(this.LoadExpectedResultJsonFile())
            });

        }
        catch (JsonQLResultValidationException)
        {
            LogHelper.Context.Log.ErrorFormat("FAILURE: Example [{0}]---------------------------", this.GetType().FullName!);
            throw;
        }

        LogHelper.Context.Log.InfoFormat("SUCCESS: Example [{0}]---------------------------", this.GetType().FullName!);

        LogHelper.Context.Log.InfoFormat("-----------------------------------------");
        LogHelper.Context.Log.InfoFormat("Generated output is in file [{0}].", GetOutputFilePath());
        LogHelper.Context.Log.InfoFormat("-----------------------------------------");
    }

    protected abstract Task<object> GetJsonQlResultAsync();

    public static string GetOutputFilePath() =>
        Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, JsonQLResultValidator.SerializedFileName);
}