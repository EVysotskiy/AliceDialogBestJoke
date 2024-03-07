using System.ComponentModel;
using System.Text.Json.Serialization;
using Logic.Converters;

namespace Logic.Command;

[Serializable,TypeConverter(typeof (StringIdConverter<TextCommand>))]
public readonly struct TextCommand
{
    private readonly string _value;

    [JsonConstructor]
    public TextCommand(string value)
    {
        _value = value;
    }
    
    public static explicit operator string(TextCommand responseCommand)
    {
        return responseCommand._value;
    }
    
    public static bool operator ==(TextCommand a, TextCommand b) => a._value == b._value;

    public static bool operator !=(TextCommand a, TextCommand b) => !(a == b);
    
    public override string ToString()
    {
        return _value;
    }

    public int CountWord()
    {
        var countWord = _value.Split(' ');
        return countWord.Length;
    }
    
    public bool IsEmpty()
    {
        return string.IsNullOrEmpty(_value);
    }

    public bool Equals(TextCommand other)
    {
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return obj is TextCommand other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }
}