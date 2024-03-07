using Domain.Model;
using Domain.Services;
using Logic.Command.Id;
using Logic.Provider.Command;

namespace Logic.Command.Factory;

public interface ICommandFactory
{
    Task<ICommand> Create(TextCommand textCommand, UserState userState, User user, bool isNewUser);
}

public class CommandFactory : ICommandFactory
{
    private readonly ICommandDataProvider _commandDataProvider;
    private readonly IJokeService _jokeService;

    public CommandFactory(ICommandDataProvider commandDataProvider, IJokeService jokeService)
    {
        _commandDataProvider = commandDataProvider;
        _jokeService = jokeService;
    }

    public async Task<ICommand> Create(TextCommand textCommand, UserState userState, User user, bool isNewUser)
    {
        var commandId = await _commandDataProvider.GetCommandId(textCommand);
        var lastCommandId = new CommandId(user.LastCommandId);

        if (isNewUser || commandId == CommandId.StartCommand())
        {
            return new StartCommand(_commandDataProvider, user);
        }

        if ((commandId == CommandId.Joke() || commandId == CommandId.Next())  && userState != null)
        {
            return new SayJokeCommand(_commandDataProvider, _jokeService, userState, user);
        }

        if (lastCommandId == CommandId.Listen() && commandId != CommandId.Help())
        {
            return new ListenSuccessJokeCommand(textCommand, user, _jokeService, _commandDataProvider);
        }

        if (commandId == CommandId.Listen())
        {
            return new ListenJokeCommand(user, _commandDataProvider);
        }

        if (lastCommandId == CommandId.Joke() && commandId != CommandId.Help())
        {
            var jokeAlreadyListened = userState.GetJokeIdIdListened();
            var randomJoke = await _jokeService.GetRandomByExcludeUserId(new[] { user.Id }, jokeAlreadyListened);

            if (randomJoke == null)
            {
                return new AllJokesAlreadySoundedCommand(_commandDataProvider);
            }

            if (commandId == CommandId.Like())
            {
                return new LikeJokeCommand(_commandDataProvider, _jokeService, userState, user);
            }

            if (commandId == CommandId.Repeat())
            {
                return new RepeatJokeCommand(_commandDataProvider, _jokeService, user);
            }
        }

        return new ErrorCommand();
    }
}