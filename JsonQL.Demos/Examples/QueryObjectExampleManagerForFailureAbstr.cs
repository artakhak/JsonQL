namespace JsonQL.Demos.Examples;

public abstract class QueryObjectExampleManagerForFailureAbstr<T> : QueryObjectExampleManagerAbstr<T>
{
    public override bool IsSuccessfulEvaluationExample => false;
}
