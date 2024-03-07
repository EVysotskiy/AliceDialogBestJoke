using System.Reflection;
using Newtonsoft.Json;

namespace Logic.Json.Converter.Abstraction;

abstract class ValueConverter<T>
{
    public abstract bool TryParse(string json, out T value);
}

abstract class TypeWrapperJsonConverter<T, K> : JsonConverter where T : struct
{
    private readonly ValueConverter<K> _converter;

    protected TypeWrapperJsonConverter(ValueConverter<K> converter)
    {
        _converter = converter;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(T) ||
               objectType == typeof(T?);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.Value == null)
        {
            return default;
        }

        var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        var ctor = typeof(T).GetConstructor(bindFlags, null, new Type[] { typeof(K) }, null);
        if (ctor == null)
        {
            string DescribeCtor(ConstructorInfo cI, string tab)
            {
                var res = $"{tab}{cI.Name}:";
                foreach (var item in cI.GetParameters())
                {
                    res += $"\n{tab}{item}";
                }

                return res;
            }

            var avaliableCtors = $"Avaliable ctors {objectType.GetConstructors().Length}:\n";
            foreach (var item in objectType.GetConstructors())
            {
                avaliableCtors += DescribeCtor(item, "\t");
            }

            throw new Exception($"Could not find constructor for {objectType} with parameter {typeof(K)}" +
                                $"\n {avaliableCtors} \n" +
                                $"Check that constructor exists and class is added to Assets/link.xml file");
        }

        if (!_converter.TryParse(reader.Value.ToString(), out var converted))
        {
            throw new Exception($"Cannot convert {reader.Value}(type:{reader.Value.GetType()}) to {typeof(K)}");
        }

        var value = ctor.Invoke(new object[] { converted });
        return (T)value;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        Write(writer, value);
    }

    protected abstract void Write(JsonWriter writer, object value);
}