using System.Web.Http;
using Domain.Model;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("joke")]
[Produces("application/json")]
public class JokeController : ApiController
{
    private readonly ILogger<JokeController> _logger;
    private readonly IJokeService _jokeService;

    public JokeController(ILogger<JokeController> logger, IJokeService jokeService)
    {
        _logger = logger;
        _jokeService = jokeService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] IReadOnlyList<JokeModel> jokes, CancellationToken cancellationToken)
    {
        foreach (var jokeModel in jokes)
        {
            var joke = new Joke(0, jokeModel.Text);
            joke = await _jokeService.Add(joke);
            _logger.Log(LogLevel.Debug, $"{joke.Id}");
        }
        
        return Ok();
    }
}