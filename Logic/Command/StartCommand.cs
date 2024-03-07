using Domain.Model;
using Logic.Command.Id;
using Logic.Command.Response;
using Logic.Provider.Command;

namespace Logic.Command;

public sealed class StartCommand : ICommand
{
    public CommandId CommandId => CommandId.StartCommand();
    
    private readonly ICommandDataProvider _commandDataProvider;
    private readonly User _user;

    public StartCommand(ICommandDataProvider commandDataProvider,User user)
    {
        _commandDataProvider = commandDataProvider;
        _user = user;
    }

    public async Task<ResponseCommand> Execute()
    {
        var startText = _commandDataProvider.GetResponse(CommandId.StartCommand());
        var menuText = _commandDataProvider.GetResponse(CommandId.MenuCommand());
        await Task.WhenAll(startText, menuText);
        
        var responseText = $"{startText.Result} {menuText.Result}";
        var responseCommand = new ResponseCommand(responseText);
        _user.LastCommandId = (string)CommandId.MenuCommand();
        
        return responseCommand;
    }
}