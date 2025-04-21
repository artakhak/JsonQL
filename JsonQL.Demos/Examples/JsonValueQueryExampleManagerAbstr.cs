using JsonQL.Query;

namespace JsonQL.Demos.Examples;

public abstract class QueryJsonValueExampleManagerAbstr : ExampleManagerAbstr
{
    /// <inheritdoc />
    protected override Task<object> GetJsonQlResultAsync()
    {
        return Task.FromResult<object>(this.QueryJsonValue());
    }

    protected abstract IJsonValueQueryResult QueryJsonValue();

    /// <inheritdoc />
    protected override string SerializeResult(object result)
    {
        if (result is not IJsonValueQueryResult jsonValueQueryResult)
            throw new ArgumentException($"The value is expected to be of type [{typeof(IJsonValueQueryResult)}]. Actual type was [{result.GetType()}].",
                nameof(result));
       
        return CompilationResultSerializerAmbientContext.Context.Serialize(jsonValueQueryResult);
    }
}