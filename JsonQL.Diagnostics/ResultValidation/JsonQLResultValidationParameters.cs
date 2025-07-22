namespace JsonQL.Diagnostics.ResultValidation;

public class JsonQLResultValidationParameters
{
    public Func<Task<object>> GetJsonQlResultAsync { get; set; } = null!;
    public Func<Task<string>> LoadExpectedResultJsonFileAsync { get; set; } = null!;
}
