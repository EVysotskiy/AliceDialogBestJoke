using Domain.Model;
using Logic.Command.Id;
using Logic.Command.Response;
using Logic.Provider.Command;

namespace Logic.Command;

public sealed class ListenJokeCommand : ICommand
{
    public CommandId CommandId => CommandId.Listen();

    
    private readonly ICommandDataProvider _commandDataProvider;
    private readonly User _user;

    public ListenJokeCommand(User user, ICommandDataProvider commandDataProvider)
    {
        _user = user;
        _commandDataProvider = commandDataProvider;
    }

    public async Task<ResponseCommand> Execute()
    {
        _user.LastCommandId = (string)CommandId.DefaultCommandId();
        return await _commandDataProvider.GetResponse(CommandId.Listen());
    }
}