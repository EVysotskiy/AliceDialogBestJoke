using Logic.Json.Converter.Abstraction;

namespace Logic.Json.Converter;

class StringValueConverter : ValueConverter<string>
{
    public override bool TryParse(string json, out string value)
    {
        value = json;
        return true;
    }
}