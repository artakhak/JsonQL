namespace JsonQL.Compilation.JsonFunction;

public interface IVariablesManagerFactory
{
    IVariablesManager Create();
}

/// <inheritdoc />
public class VariablesManagerFactory : IVariablesManagerFactory
{
    /// <inheritdoc />
    public IVariablesManager Create()
    {
        return new VariablesManager();
    }
}