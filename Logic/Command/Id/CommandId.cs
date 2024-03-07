using System.ComponentModel;
using System.Text.Json.Serialization;
using Logic.Converters;

namespace Logic.Command.Id;

[Serializable,TypeConverter(typeof (StringIdConverter<CommandId>))]
public struct CommandId
{
    private readonly string _value;
    
    [JsonConstructor]
    public CommandId(string value)
    {
        _value = value;
    }
    
    public static bool operator ==(CommandId a, CommandId b) => a._value == b._value;

    public static bool operator !=(CommandId a, CommandId b) => !(a == b);
    public static explicit operator string(CommandId commandId) => commandId._value;

    
    public override string ToString()
    {
        return _value;
    }

    public bool IsEmpty()
    {
        return string.IsNullOrEmpty(_value);
    }

    public bool Equals(CommandId other)
    {
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return obj is CommandId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }
    
    //CommandId
    public static CommandId DefaultCommandId() => new("default");
    public static CommandId StartCommand() => new("start");
    public static CommandId AllJokesAlreadySounded() => new("allJokesAlreadySounded");
    public static CommandId MenuCommand() => new("menu");
    public static CommandId Next() => new("next");
    public static CommandId Joke() => new("joke");
    public static CommandId Listen() => new("listen");
    public static CommandId Like() => new("like");
    public static CommandId TextEndJokes() => new("textEndJokes");
    public static CommandId Short() => new("shortJokes");
    public static CommandId ListenSuccess() => new("listenSuccess");
    public static CommandId Repeat() => new("repeat");
    public static CommandId DuckSound() => new("duckSound");
    public static CommandId Help() => new("help");

}