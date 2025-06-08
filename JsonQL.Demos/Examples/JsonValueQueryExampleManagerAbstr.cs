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
}