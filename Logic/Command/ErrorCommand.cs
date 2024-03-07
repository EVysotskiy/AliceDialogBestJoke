using Logic.Command.Id;
using Logic.Command.Response;

namespace Logic.Command;

public sealed class ErrorCommand : ICommand
{
    public CommandId CommandId => CommandId.DefaultCommandId();
    
    public Task<ResponseCommand> Execute()
    {
        return Task.FromResult(ResponseCommand.ERROR_RESPONSE);
    }
}