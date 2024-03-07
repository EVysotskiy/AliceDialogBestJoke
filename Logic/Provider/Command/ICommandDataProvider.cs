using Logic.Command;
using Logic.Command.Id;
using Logic.Command.Response;

namespace Logic.Provider.Command;

public interface ICommandDataProvider
{
    Task<CommandId> GetCommandId(TextCommand textCommand);
    Task<ResponseCommand> GetResponse(CommandId commandId);
}