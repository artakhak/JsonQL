namespace JsonQL.Diagnostics.ResultValidation;

public class JsonQLResultValidationException: ApplicationException
{
    public JsonQLResultValidationException(string errorMessage): base(errorMessage)
    {
        
    }
}