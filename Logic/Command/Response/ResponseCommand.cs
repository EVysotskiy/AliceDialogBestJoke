using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Logic.Converters;

namespace Logic.Command.Response;

[Serializable,TypeConverter(typeof (StringIdConverter<ResponseCommand>))]
public readonly struct ResponseCommand
{

    public static ResponseCommand ERROR_RESPONSE = new ResponseCommand("Что - то пошло не так.");
    private const string DUCK_SOUND = "<speaker audio=\"dialogs-upload/eae2f2d1-6cd9-4885-b21d-997afd2c5d1f/9a0561b3-0b40-4f34-b003-099765aa9b3b.opus\">";
    
    private readonly string _value;
    private readonly string _tts;

    [JsonConstructor]
    public ResponseCommand(string value)
    {
        _value = value;
        _tts = ProfanityFilterTts(_value);
    }

    private string ProfanityFilterTts(string value)
    {
        var regex = new Regex(@"\*+");
        var result = regex.Replace(value, DUCK_SOUND);
        return result;
    }

    public static explicit operator string(ResponseCommand responseCommand)
    {
        return responseCommand._value;
    }
    
    public static bool operator ==(ResponseCommand a, ResponseCommand b) => a._value == b._value;

    public static bool operator !=(ResponseCommand a, ResponseCommand b) => !(a == b);
    
    public override string ToString()
    {
        return _value;
    }

    public bool IsEmpty()
    {
        return string.IsNullOrEmpty(_value);
    }

    public bool Equals(ResponseCommand other)
    {
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return obj is ResponseCommand other && Equals(other);
    }

    public string GetTts()
    {
        return _tts;
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }
}