namespace Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DirtinessFactor
{
    Negligible = 0,
    VeryLow = 1,
    Low = 2,
    Medium = 3,
    VeryHigh = 4,
    High = 5,
    Extreme = 6
}
