namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Represents a factory for creating instances of <see cref="IVariablesManager"/>.
/// </summary>
public interface IVariablesManagerFactory
{
    /// <summary>
    /// Creates an instance of <see cref="IVariablesManager"/>.
    /// </summary>
    /// <returns></returns>
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