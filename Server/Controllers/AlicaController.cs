using System.Web.Http;
using Logic.Command;
using Logic.Handler.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Yandex.Alice.Sdk.Models;

namespace Server.Controllers;

[ApiController]
[Route("alica")]
[Produces("application/json")]
public class AlicaController : ApiController
{
    private readonly ILogger<AlicaController> _logger;
    private readonly ICommandExecutor _commandExecutor;

    public AlicaController(ILogger<AlicaController> logger, ICommandExecutor commandExecutor)
    {
        _logger = logger;
        _commandExecutor = commandExecutor;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AliceRequest aliceRequest, CancellationToken cancellationToken)
    {
        _logger.Log(LogLevel.Debug, $"aliceRequest: {aliceRequest.Version}");
        var command = new TextCommand(aliceRequest.Request.Command);
        var platformId = aliceRequest.Session.UserId;
        var responseCommand = await _commandExecutor.Execute(command, platformId);
        var tts = responseCommand.GetTts();
        var aliceResponse = new AliceResponse(aliceRequest, responseCommand.ToString(), tts: tts);
        
        return Ok(aliceResponse);
    }
}