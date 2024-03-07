using System.Web.Http;
using Logic.Command;
using Logic.Handler.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Yandex.Alice.Sdk.Models;

namespace Server.Controllers;

[ApiController]
[Route("verification")]
[Produces("application/json")]
public class VerificationController : ApiController
{
    private readonly ILogger<AlicaController> _logger;

    public VerificationController(ILogger<AlicaController> logger)
    {
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(string access_token, CancellationToken cancellationToken)
    {
        return Ok();
    }
}