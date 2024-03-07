using Domain.Model;
using Domain.Services;
using Logic.Command.Id;
using Logic.Command.Response;
using Logic.Provider.Command;

namespace Logic.Command;

public sealed class ListenSuccessJokeCommand : ICommand
{
    public CommandId CommandId => CommandId.Like();

    
    private readonly TextCommand _textCommand;
    private readonly ICommandDataProvider _commandDataProvider;
    private readonly IJokeService _jokeService;
    private readonly User _user;

    public ListenSuccessJokeCommand(TextCommand textCommand, User user, IJokeService jokeService, ICommandDataProvider commandDataProvider)
    {
        _textCommand = textCommand;
        _user = user;
        _jokeService = jokeService;
        _commandDataProvider = commandDataProvider;
    }

    public async Task<ResponseCommand> Execute()
    {
        var userJoke = _textCommand;
        if (userJoke.CountWord() <= 5)
        {
            var responseCommand = await _commandDataProvider.GetResponse(CommandId.Short());
            return responseCommand;
        }
        
        var newJoke = new Joke(_user.Id, userJoke.ToString());
        await _jokeService.Add(newJoke);
        return await _commandDataProvider.GetResponse(CommandId.ListenSuccess());
    }
}