namespace JsonQL.Demos.Examples;

public abstract class QueryObjectExampleManagerForSuccessAbstr<T> : QueryObjectExampleManagerAbstr<T>
{
    public override bool IsSuccessfulEvaluationExample => true;
}
