using Domain.Model;
using Domain.Services;
using Logic.Command.Id;
using Logic.Command.Response;
using Logic.Extension;
using Logic.Provider.Command;

namespace Logic.Command;

public class LikeJokeCommand : ICommand
{
    public CommandId CommandId => CommandId.Joke();
    
    private readonly UserState _userState;
    private readonly IJokeService _jokeService;
    private readonly ICommandDataProvider _commandDataProvider;
    private readonly User _user;

    public LikeJokeCommand(ICommandDataProvider commandDataProvider, IJokeService jokeService, UserState userState, User user)
    {
        _commandDataProvider = commandDataProvider;
        _jokeService = jokeService;
        _userState = userState;
        _user = user;
    }

    public async Task<ResponseCommand> Execute()
    {
        var jokeAlreadyListened = _userState.GetJokeIdIdListened();
        var randomJokeTask = _jokeService.GetRandomByExcludeUserId(new[] { _user.Id }, jokeAlreadyListened);
        var likeJokeResponseTask = _commandDataProvider.GetResponse(CommandId.Like());
        await Task.WhenAll(randomJokeTask, likeJokeResponseTask);
        
        if (randomJokeTask.Result == null)
        {
            return likeJokeResponseTask.Result;
        }

        var responseCommand = new ResponseCommand($"{randomJokeTask.Result.Text}");
        _user.LastSoundJokeId = randomJokeTask.Id;
        _userState.AddListenedJoke(randomJokeTask.Id);
        
        var lastSoundJokeId = _user.LastSoundJokeId;
        var likeText = await _commandDataProvider.GetResponse(CommandId.Like());
         responseCommand = $"{likeText} {responseCommand}".ToResponse();
        await _jokeService.Like(lastSoundJokeId);
        return responseCommand;
    } 
}