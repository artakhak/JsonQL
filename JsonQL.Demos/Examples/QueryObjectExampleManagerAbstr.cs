using JsonQL.Query;

namespace JsonQL.Demos.Examples;

public abstract class QueryObjectExampleManagerAbstr<T> : ExampleManagerAbstr
{
    /// <inheritdoc />
    protected override Task<object> GetJsonQlResultAsync()
    {
        return Task.FromResult<object>(this.QueryObject());
    }

    protected abstract IObjectQueryResult<T> QueryObject();
}