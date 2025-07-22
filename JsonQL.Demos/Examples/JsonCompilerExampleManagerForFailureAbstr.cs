namespace JsonQL.Demos.Examples;

public abstract class JsonCompilerExampleManagerForFailureAbstr : JsonCompilerExampleManagerAbstr
{
    public override bool IsSuccessfulEvaluationExample => false;
}
