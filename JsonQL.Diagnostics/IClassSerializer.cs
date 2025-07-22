using Newtonsoft.Json;

namespace JsonQL.Diagnostics;

public interface IClassSerializer
{
    string Serialize(object objectToSerialize);
}

public class ClassSerializer: IClassSerializer
{
    public string Serialize(object objectToSerialize)
    {
        var jsonSerializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All
        };
        return JsonConvert.SerializeObject(objectToSerialize, jsonSerializerSettings);
    }
}
