using Logic.Command;
using Logic.Command.Response;

namespace Logic.Handler.Abstraction;

public interface ICommandExecutor
{
    Task<ResponseCommand> Execute(TextCommand textCommand, string platformId);
}