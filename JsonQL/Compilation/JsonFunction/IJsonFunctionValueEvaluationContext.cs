namespace JsonQL.Compilation.JsonFunction;

public interface IJsonFunctionValueEvaluationContext
{
    /// <summary>
    /// Note, tne value is set later. Therefore, do not use this value in constructors of classes where
    /// this class instances are injected.
    /// The value can be used when evaluating function value. For example in <see cref="IJsonFunction.EvaluateValue"/>.
    /// </summary>
    IJsonFunction? ParentJsonFunction { get; }

    /// <summary>
    /// An instance of <see cref="IVariablesManager"/> for registering or resolving variables.
    /// </summary>
    IVariablesManager VariablesManager { get; }
}

/// <inheritdoc />
public class JsonFunctionValueEvaluationContext : IJsonFunctionValueEvaluationContext
{
    //private IJsonFunction? _parentJsonFunction;

    public JsonFunctionValueEvaluationContext(IVariablesManager variablesManager)
    {
        VariablesManager = variablesManager;
    }

    /// <inheritdoc />
    public IJsonFunction? ParentJsonFunction { get; set; }

    /// <inheritdoc />
    public IVariablesManager VariablesManager { get; }
}