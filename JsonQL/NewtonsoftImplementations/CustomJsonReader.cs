using Newtonsoft.Json;

namespace JsonQL.NewtonsoftImplementations;

public class CustomJsonReader: JsonTextReader
{
    public CustomJsonReader(TextReader reader) : base(reader)
    {
        //this.DateParseHandling = DateParseHandling.None;
    }

    //public override DateTime? ReadAsDateTime()
    //{
    //    return null;
    //}

    //public override JsonToken TokenType
    //{
    //    get
    //    {
    //        var tokenType = base.TokenType;

    //        switch (tokenType)
    //        {
    //            case JsonToken.Date:
    //                return JsonToken.String;

    //            default:
    //                return tokenType;
    //        }
    //    }
    //}

    //public override bool Read()
    //{
    //    if (!base.Read())
    //        return false;
        
    //    return true;
    //}

    
}