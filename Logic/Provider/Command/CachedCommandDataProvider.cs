using Logic.Command;
using Logic.Command.Id;
using Logic.Command.Response;
using Logic.Extension;
using Microsoft.Extensions.Caching.Memory;

namespace Logic.Provider.Command;

public class CachedCommandDataProvider : ICommandDataProvider
{
    private readonly ICommandDataProvider _commandDataProvider;
    private readonly IMemoryCache _cache;

    private const string KeyPrefix = "Command";

    public CachedCommandDataProvider(IMemoryCache cache, ICommandDataProvider commandDataProvider)
    {
        _cache = cache;
        _commandDataProvider = commandDataProvider;
    }

    public async Task<CommandId> GetCommandId(TextCommand textCommand)
    {
        return await _cache.Remember($"{KeyPrefix}:textCommand:{textCommand}",
            async () => await _commandDataProvider.GetCommandId(textCommand));
    }

    public async Task<ResponseCommand> GetResponse(CommandId commandId)
    {
        return await _commandDataProvider.GetResponse(commandId);
    }
}