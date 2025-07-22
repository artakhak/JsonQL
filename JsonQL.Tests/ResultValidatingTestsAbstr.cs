using JsonQL.Compilation;
using JsonQL.Diagnostics;
using JsonQL.Diagnostics.ResultValidation;
using JsonQL.Query;
using OROptimizer.ServiceResolver.DefaultImplementationBasedObjectFactory;

namespace JsonQL.Tests;

public abstract class ResultValidatingTestsAbstr : JsonCompilationTestsAbstr
{
    public override void Setup()
    {
        base.Setup();
        CompilationResultSerializerAmbientContext.Context = this.JsonQLDefaultImplementationBasedObjectFactory.CreateInstance<ICompilationResultSerializer>();
        QueryManager = this.JsonQLDefaultImplementationBasedObjectFactory.CreateInstance<IQueryManager>();
    }

    protected IQueryManager QueryManager { get; private set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="testJsonTextDataPath">Json text path for the JSON to compile.</param>
    /// <param name="getCompilationResult">
    /// A function that returns <see cref="ICompilationResult"/> using a parameter of
    /// <see cref="ICompilationResult"/> created using <paramref name="testJsonTextDataPath"/>. 
    /// </param>
    /// <param name="expectedResultJsonFilePath">A function that returns <see cref="JsonFilePath"/> referencing expected result file path.
    /// </param>
    protected Task ValidateCompilationResultAsync(TestJsonTextDataPath testJsonTextDataPath,
        Func<IJsonTextData, ICompilationResult> getCompilationResult, JsonFilePath expectedResultJsonFilePath)
    {
        return JsonQLResultValidator.ValidateResultAsync(new JsonQLResultValidationParameters
        {
            GetJsonQlResultAsync = () => Task.FromResult<object>(getCompilationResult(LoadJsonTextData(testJsonTextDataPath))),
            LoadExpectedResultJsonFileAsync = () => Task.FromResult(LoadJsonFile(expectedResultJsonFilePath))
        });
    }

    public delegate IJsonValueQueryResult GetQueryResultDelegate(string query, IJsonTextData jsonTextData);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="query">Query expression.</param>
    /// <param name="testJsonTextDataPath">Json text path for the JSON to compile.</param>
    /// <param name="getQueryResult">
    /// A function that returns <see cref="IJsonValueQueryResult"/> using parameters of types <see cref="string"/> for query expression
    /// and <see cref="IJsonTextData"/> for JSON file the query will be executed against.
    /// </param>
    /// <param name="expectedResultJsonFilePath">A function that returns <see cref="JsonFilePath"/> referencing expected result file path.
    /// </param>
    protected Task ValidateQueryResultAsync(string query, TestJsonTextDataPath testJsonTextDataPath,
        GetQueryResultDelegate getQueryResult, JsonFilePath expectedResultJsonFilePath)
    {
        return JsonQLResultValidator.ValidateResultAsync(new JsonQLResultValidationParameters
        {
            GetJsonQlResultAsync = () => Task.FromResult<object>(getQueryResult(query, LoadJsonTextData(testJsonTextDataPath))),
            LoadExpectedResultJsonFileAsync = () => Task.FromResult(LoadJsonFile(expectedResultJsonFilePath))
        });
    }
    
    public delegate IObjectQueryResult<T> GetQueryObjectResultDelegate<T>(string query, IJsonTextData jsonTextData);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Type of object returned by query.</typeparam>
    /// <param name="query">Query expression.</param>
    /// <param name="testJsonTextDataPath">Json text path for the JSON to compile.</param>
    /// <param name="getQueryObjectResult">
    /// A function that returns <see cref="IObjectQueryResult{T}"/> using parameters of types <see cref="string"/> for query expression
    /// and <see cref="IJsonTextData"/> for JSON file the query will be executed against.
    /// </param>
    /// <param name="expectedResultJsonFilePath">A function that returns <see cref="JsonFilePath"/> referencing expected result file path.
    /// </param>
    protected Task ValidateQueryResultAsync<T>(string query, TestJsonTextDataPath testJsonTextDataPath,
        GetQueryObjectResultDelegate<T> getQueryObjectResult, JsonFilePath expectedResultJsonFilePath)
    {
        return JsonQLResultValidator.ValidateResultAsync(new JsonQLResultValidationParameters
        {
            GetJsonQlResultAsync = () => Task.FromResult<object>(getQueryObjectResult(query, LoadJsonTextData(testJsonTextDataPath))),
            LoadExpectedResultJsonFileAsync = () => Task.FromResult(LoadJsonFile(expectedResultJsonFilePath))
        });
    }

    private IJsonTextData LoadJsonTextData(TestJsonTextDataPath testJsonTextDataPath)
    {
        var jsonTextDataLoader = new JsonTextDataLoader(testJsonTextDataPath.RelativeFolderPath);

        return jsonTextDataLoader
            .GetJsonTextData(testJsonTextDataPath.CompiledFileName, testJsonTextDataPath.ParentFileName, testJsonTextDataPath.ParentParentFileName);
    }

    private string LoadJsonFile(JsonFilePath jsonFilePath)
    {
        return ResourceFileLoader.LoadJsonFile(
            new ResourcePath(jsonFilePath.JsonFileName, jsonFilePath.RelativeFolderPath.ToList()));
    }
}
