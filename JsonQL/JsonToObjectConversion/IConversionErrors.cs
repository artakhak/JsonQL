using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Represents a collection of conversion errors encountered during a JSON to object conversion process.
/// </summary>
public interface IConversionErrors
{
    /// <summary>
    /// Gets the collection of conversion errors encountered during the JSON to object conversion process.
    /// </summary>
    IReadOnlyList<IConversionError> Errors { get; }

    /// <summary>
    /// Adds a conversion error to the collection of errors.
    /// </summary>
    /// <param name="conversionError">The conversion error to add to the collection.</param>
    void AddError(IConversionError conversionError);

    /// <summary>
    /// Attempts to retrieve conversion errors of a specific type.
    /// </summary>
    /// <param name="conversionErrorType">The type of conversion errors to retrieve.</param>
    /// <param name="errors">When this method returns, contains the list of conversion errors of the specified type, if found; otherwise, null.</param>
    /// <returns>True if errors of the specified type are found; otherwise, false.</returns>
    bool TryGetErrorsOfType(ConversionErrorType conversionErrorType, [NotNullWhen(true)] out IReadOnlyList<IConversionError>? errors);
}

/// <inheritdoc />
internal class ConversionErrors : IConversionErrors
{
    private readonly List<IConversionError> _errors = new();
    private readonly Dictionary<ConversionErrorType, List<IConversionError>> _errorsTypeToConversionsErrors = new();
  
    /// <inheritdoc />
    public IReadOnlyList<IConversionError> Errors => _errors;
 
    /// <inheritdoc />
    public void AddError(IConversionError conversionError)
    {
        _errors.Add(conversionError);

        if (!_errorsTypeToConversionsErrors.TryGetValue(conversionError.ErrorType, out var errorsOfErrorType))
        {
            errorsOfErrorType = new List<IConversionError>();
            _errorsTypeToConversionsErrors[conversionError.ErrorType] = errorsOfErrorType;
        }

        errorsOfErrorType.Add(conversionError);
    }

    /// <inheritdoc />
    public bool TryGetErrorsOfType(ConversionErrorType conversionErrorType, [NotNullWhen(true)] out IReadOnlyList<IConversionError>? errors)
    {
        if (!_errorsTypeToConversionsErrors.TryGetValue(conversionErrorType, out var errorsList))
        {
            errors = null;
            return false;
        }

        errors = errorsList;
        return true;
    }
}