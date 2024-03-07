using Logic.Command.Id;
using Logic.Command.Response;
using Newtonsoft.Json;

namespace Logic.Command.Data;


[Serializable]
public class CommandData
{
    [System.Text.Json.Serialization.JsonIgnore] 
    public IReadOnlyDictionary<TextCommand, CommandId> CommandIds => _commandIds;

    [JsonProperty("commandIds")]
    private Dictionary<TextCommand, CommandId> _commandIds { get; set; }
    
    
    [System.Text.Json.Serialization.JsonIgnore] 
    public IReadOnlyDictionary<CommandId, ResponseCommand> Responses => _responses;
    
    [JsonProperty("responses")]
    private Dictionary<CommandId, ResponseCommand> _responses { get; set; }
}