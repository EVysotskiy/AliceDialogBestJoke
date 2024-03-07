using Logic.Command;
using Logic.Command.Id;
using Logic.Command.Response;
using Logic.Json.Converter;
using Newtonsoft.Json;

namespace Logic.Json;

public class JsonParser
{
    public static readonly JsonSerializerSettings Settings = new()
    {
        Converters = new List<JsonConverter>
        {
            new StringIdJsonConverter<CommandId>(),
            new StringIdJsonConverter<ResponseCommand>(),
            new StringIdJsonConverter<TextCommand>(),
        },

        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.None
    };
    
    public static T Deserialize<T>(string str)
    {
        return JsonConvert.DeserializeObject<T>(str, Settings);
    }
}