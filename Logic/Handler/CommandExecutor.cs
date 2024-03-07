using System.Text.Json;
using Domain.Model;
using Domain.Services;
using Logic.Command;
using Logic.Command.Factory;
using Logic.Command.Response;
using Logic.Handler.Abstraction;

namespace Logic.Handler;

public class CommandExecutor : ICommandExecutor
{
    private readonly IUserService _userService;
    private readonly ICommandFactory _commandFactory;

    public CommandExecutor(IUserService userService, ICommandFactory commandFactory)
    {
        _userService = userService;
        _commandFactory = commandFactory;
    }

    public async Task<ResponseCommand> Execute(TextCommand textCommand, string platformId)
    {
        if (string.IsNullOrEmpty(platformId))
        {
            return await new ErrorCommand().Execute();
        }

        var userInfo = await _userService.GetOrCreate(platformId);
        var user = userInfo.user;
        var isNewUser = userInfo.isNewUser;
        var userState = JsonSerializer.Deserialize<UserState>(user.State);

        if (userState == null)
        {
            return await new ErrorCommand().Execute();
        }

        var command = await _commandFactory.Create(textCommand, userState, user, isNewUser);
        var responseCommand = await command.Execute();

        user.State = JsonSerializer.Serialize(userState);
        user.LastCommandId = (string)command.CommandId;
        await _userService.Update(user);

        return responseCommand;
    }
}