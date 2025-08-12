namespace Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CategoryType
{
    Chore,
    Room,
    Intro,
    Outro,
    Completed
}
