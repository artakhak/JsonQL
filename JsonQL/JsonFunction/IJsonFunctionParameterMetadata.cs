using JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;
using JsonQL.JsonFunction.JsonFunctions;
using JsonQL.JsonFunction.SimpleTypes;

namespace JsonQL.JsonFunction;

public interface IJsonFunctionParameterMetadata
{
    /// <summary>
    /// Name that will appear in logs. For example "operand1", etc. The name might tbe different from actual parameter value
    /// passed to classes that extend <see cref="JsonFunctionAbstr"/>.
    /// </summary>
    string Name { get; }

    ///// <summary>
    ///// Possible type identifiers for parameter types.
    ///// Example: [<see cref="FunctionReturnAndParameterTypes.Double"/>, <see cref="FunctionReturnAndParameterTypes.ArrayOfReferencedParsedJsonValues"/>]
    ///// </summary>
    //public IReadOnlyList<Guid> ValidTypes { get; }

    /// <summary>
    /// If the value is true, parameter will be validated to not be a json value path <see cref="IJsonValuePathJsonFunction"/>
    /// that evaluates to a path that has elements of that select multiple items (e.g., <see cref="WhereClauseArrayItemsSelectorPathElement"/>)
    /// </summary>
    bool ValidateIsNotMultipleValuesSelectorPath { get; }

    /// <summary>
    /// One of the types: <see cref="IBooleanJsonFunction"/>, <see cref="IStringJsonFunction"/>, <see cref="IDoubleJsonFunction"/>,
    /// <see cref="IDateTimeJsonFunction"/>, <see cref="IJsonFunction"/>, etc.
    /// </summary>
    Type ExpectedParameterFunctionType { get; }

    /// <summary>
    /// True, if parameter is required.
    /// </summary>
    bool IsRequired { get; }
}

public class JsonFunctionParameterMetadata: IJsonFunctionParameterMetadata
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">
    /// Name that will appear in logs. For example "operand1", etc. The name might tbe different from actual parameter value
    /// passed to classes that extend <see cref="JsonFunctionAbstr"/>.
    /// </param>
    /// <param name="expectedParameterFunctionType">
    /// One of the types: <see cref="IBooleanJsonFunction"/>, <see cref="IStringJsonFunction"/>, <see cref="IDoubleJsonFunction"/>,
    /// <see cref="IDateTimeJsonFunction"/>, <see cref="IJsonFunction"/>, etc.
    /// </param>
    /// <param name="isRequired">True, if parameter is required.</param>
    ///// <param name="validateParameterIsNotCollection">If the value is true, parameter will be validated to not be a </param>
    public JsonFunctionParameterMetadata(string name, Type expectedParameterFunctionType, bool isRequired)
    {
        Name = name;
        ExpectedParameterFunctionType = expectedParameterFunctionType;
        IsRequired = isRequired;
    }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public bool ValidateIsNotMultipleValuesSelectorPath { get; set; } = true;

    /// <inheritdoc />
    public Type ExpectedParameterFunctionType { get; }

    /// <inheritdoc />
    public bool IsRequired { get; }
}
