using Domain.Model;
using Domain.Services;
using Logic.Command.Id;
using Logic.Command.Response;
using Logic.Provider.Command;

namespace Logic.Command;

public class AllJokesAlreadySoundedCommand : ICommand
{
    public CommandId CommandId => CommandId.AllJokesAlreadySounded();
    
    private readonly ICommandDataProvider _commandDataProvider;

    public AllJokesAlreadySoundedCommand(ICommandDataProvider commandDataProvider)
    {
        _commandDataProvider = commandDataProvider;
    }

    public async Task<ResponseCommand> Execute()
    {
        return await _commandDataProvider.GetResponse(CommandId.AllJokesAlreadySounded());
    }
}