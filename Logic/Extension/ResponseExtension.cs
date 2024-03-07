using Logic.Command;
using Logic.Command.Response;

namespace Logic.Extension;

public static class ResponseExtension
{
    public static ResponseCommand ToResponse(this string text)
    {
        return new ResponseCommand(text);
    }
}