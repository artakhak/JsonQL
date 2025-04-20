using System;

namespace JsonQL.Extensions.JsonToObjectConversion;

public class JsonConversionException : ApplicationException
{
    public JsonConversionException(string message): base(message)
    {
        
    }

    public JsonConversionException()
    {
        
    }
}