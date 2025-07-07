using JsonQL.Query;

namespace JsonQL.Tests;

public abstract class QueryResultAsObjectValidationTestsAbstr: JsonCompilationTestsAbstr
{
    [SetUp]
    public override void Setup()
    {
        //var defaultJsonCompilerFactory = new DefaultJsonCompilerFactory(LogHelper.Context.Log);
        //JsonCompiler = defaultJsonCompilerFactory.Create();

        //QueryManager = new Query.QueryManager(this.JsonCompiler, new JsonParsedValueConversionManager())
    }

    /*protected Task ValidateQueryResult<T>()
    {

    }*/

    protected IQueryManager QueryManager { get; private set; } = null!;
}
