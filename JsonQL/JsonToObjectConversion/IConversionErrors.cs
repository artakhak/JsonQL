using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion;

public interface IConversionErrors
{
    IReadOnlyList<IConversionError> Errors { get; }
    void AddError(IConversionError conversionError);
    bool TryGetErrorsOfType(ConversionErrorType conversionErrorType, [NotNullWhen(true)] out IReadOnlyList<IConversionError>? errors);
}

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