using Domain.Model;
using Domain.Services;
using Logic.Command.Id;
using Logic.Command.Response;
using Logic.Provider.Command;

namespace Logic.Command;

public class SayJokeCommand : ICommand
{
    public CommandId CommandId => CommandId.Joke();
    
    private readonly UserState _userState;
    private readonly IJokeService _jokeService;
    private readonly ICommandDataProvider _commandDataProvider;
    private readonly User _user;

    public SayJokeCommand(ICommandDataProvider commandDataProvider, IJokeService jokeService, UserState userState, User user)
    {
        _commandDataProvider = commandDataProvider;
        _jokeService = jokeService;
        _userState = userState;
        _user = user;
    }

    public async Task<ResponseCommand> Execute()
    {
        var jokeAlreadyListened = _userState.GetJokeIdIdListened();
        
        var randomJoke = await _jokeService.GetRandomByExcludeUserId(new[] { _user.Id }, jokeAlreadyListened);
        if (randomJoke == null)
        {
            var responseCommand = await _commandDataProvider.GetResponse(CommandId.AllJokesAlreadySounded());
            return responseCommand;
        }

        var textEndJoke = await _commandDataProvider.GetResponse(CommandId.TextEndJokes());
        _user.LastSoundJokeId = randomJoke.Id;
        _userState.AddListenedJoke(randomJoke.Id);
        return new ResponseCommand($"{randomJoke.Text} {textEndJoke}");
    }
}