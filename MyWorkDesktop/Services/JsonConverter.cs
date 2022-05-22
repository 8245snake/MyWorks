using Newtonsoft.Json;
using System;
using System.Globalization;


namespace MyWorkDesktop.Services;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string DateFormat = "yyyy/MM/dd";

    public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return DateOnly.Parse((string)reader.Value);
    }

    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(DateFormat));
    }
}
public class NullableDateOnlyJsonConverter : JsonConverter<Nullable<DateOnly>>
{
    private const string DateFormat = "yyyy/MM/dd";

    public override DateOnly? ReadJson(JsonReader reader, Type objectType, DateOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (DateOnly.TryParse((string)reader.Value, out var d) == true)
        {
            return d;
        }
        return null;
    }

    public override void WriteJson(JsonWriter writer, DateOnly? value, JsonSerializer serializer)
    {
        if (value.HasValue)
        {
            writer.WriteValue(value.Value.ToString(DateFormat));
        }
        else
        {
        }
    }
}

public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private const string TimeFormat = "HH:mm:ss.FFFFFFF";

    public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return TimeOnly.ParseExact((string)reader.Value, TimeFormat, CultureInfo.InvariantCulture);
    }

    public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(TimeFormat, CultureInfo.InvariantCulture));
    }
}
public class NullableTimeOnlyJsonConverter : JsonConverter<Nullable<TimeOnly>>
{
    private const string TimeFormat = "HH:mm:ss.FFFFFFF";

    public override TimeOnly? ReadJson(JsonReader reader, Type objectType, TimeOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (TimeOnly.TryParseExact((string)reader.Value, TimeFormat, out var t) == true)
        {
            return t;
        }
        return null;
    }

    public override void WriteJson(JsonWriter writer, TimeOnly? value, JsonSerializer serializer)
    {
        if (value.HasValue)
        {
            writer.WriteValue(value.Value.ToString(TimeFormat, CultureInfo.InvariantCulture));
        }
    }
}