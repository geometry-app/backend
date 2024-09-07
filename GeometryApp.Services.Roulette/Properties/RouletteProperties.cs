using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Folleach.Properties;

namespace GeometryApp.Services.Roulette.Properties;

[DataContract]
public class RouletteProperties
{
    public static readonly string Scope = "roulette/roulette";

    [JsonPropertyName("Type")]
    public string Type { get; set; } = null!;
    [JsonPropertyName("OwnerSession")]
    public string OwnerSession { get; set; } = null!;
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("createDt")]
    private string? createDt { get; set; }
    [JsonPropertyName("id")]
    private string? id { get; set; }
    [JsonPropertyName("published")]
    private string? isPublished { get; set; }

    [IgnoreProperty]
    public DateTime CreateDt
    {
        get => createDt == null ? DateTime.MinValue : DateTime.Parse(createDt, null, System.Globalization.DateTimeStyles.RoundtripKind);
        set => createDt = value.ToString("O");
    }

    [IgnoreProperty]
    public Guid Id
    {
        get => id == null ? throw new InvalidOperationException("roulette id is not set. it's a wrong") : Guid.Parse(id);
        set => id = value.ToString();
    }

    [IgnoreProperty]
    public bool IsPublished
    {
        get => bool.TryParse(isPublished, out var value) && value;
        set => isPublished = value.ToString();
    }
}
