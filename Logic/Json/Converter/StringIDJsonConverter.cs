using Logic.Json.Converter.Abstraction;
using Newtonsoft.Json;

namespace Logic.Json.Converter;

internal class StringIdJsonConverter<T> : TypeWrapperJsonConverter<T, string> where T : struct
{
    public StringIdJsonConverter() : base(new StringValueConverter()){}

    protected override void Write(JsonWriter writer, object value)
    {
        writer.WriteValue(value.ToString());
    }
}