using Logic.Command.Id;
using Logic.Command.Response;

namespace Logic.Command;

public interface ICommand
{
    Task<ResponseCommand> Execute();
    CommandId CommandId { get; }
}