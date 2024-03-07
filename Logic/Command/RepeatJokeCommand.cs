using Domain.Model;
using Domain.Services;
using Logic.Command.Id;
using Logic.Command.Response;
using Logic.Extension;
using Logic.Provider.Command;

namespace Logic.Command;

public class RepeatJokeCommand : ICommand
{
    public CommandId CommandId => CommandId.Joke();

    
    private readonly IJokeService _jokeService;
    private readonly ICommandDataProvider _commandDataProvider;
    private readonly User _user;

    public RepeatJokeCommand(ICommandDataProvider commandDataProvider, IJokeService jokeService, User user)
    {
        _commandDataProvider = commandDataProvider;
        _jokeService = jokeService;
        _user = user;
    }

    public async Task<ResponseCommand> Execute()
    {
        var textEndJoke = await _commandDataProvider.GetResponse(CommandId.TextEndJokes());
        var lastSoundJokeId = _user.LastSoundJokeId;
        var joke = await _jokeService.Get(lastSoundJokeId);
        _user.LastSoundJokeId = lastSoundJokeId;
        return joke != null ? $"{joke.Text}".ToResponse() : textEndJoke;
    }
}