using Logic.Command;
using Logic.Command.Data;
using Logic.Command.Id;
using Logic.Command.Response;
using Logic.Json;

namespace Logic.Provider.Command;

public class CommandDataProvider : ICommandDataProvider
{
    private const string FileName = "../Logic/command.json";
    private CommandData? _commandData = null;
    
    public async Task<CommandId> GetCommandId(TextCommand textCommand)
    {
        var commandData = await GetCommandData();
        var commandIds = commandData.CommandIds;
        if (commandIds.TryGetValue(textCommand,out var commandId))
        {
            return commandId;
        }
        
        return CommandId.DefaultCommandId();
    }
    
    public async Task<ResponseCommand> GetResponse(CommandId commandId)
    {
        var commandData = await GetCommandData();
        var responses = commandData.Responses;
        if (responses.TryGetValue(commandId,out var responseCommand))
        {
            return responseCommand;
        }

        return new ResponseCommand("Упс...");
    }
    
    private async Task<CommandData> GetCommandData()
    {
        if (_commandData != null)
        {
            return _commandData;
        }

        var path = FileName;
        var fileText = await GetFile(path, string.Empty);

        if (string.IsNullOrEmpty(fileText))
        {
            return new CommandData();
        }

        var commandData = JsonParser.Deserialize<CommandData>(fileText);
        if (commandData == null)
        {
            return new CommandData();
        }
        
        _commandData = commandData;
        return _commandData;
    }

    private async Task<string> GetFile(string path, string defaultText)
    {
        var localPath = $"{path}";

        if (File.Exists(localPath))
        {
            return await File.ReadAllTextAsync(localPath);
        }

        return defaultText;
    }
}