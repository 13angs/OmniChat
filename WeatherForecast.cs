using Newtonsoft.Json;

namespace OmniChat;

public class WeatherForecast
{
    [JsonProperty("date")]
    public DateOnly Date { get; set; }

    [JsonProperty("temperaturec")]
    public int TemperatureC { get; set; }

    [JsonProperty("temperaturef")]
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    [JsonProperty("summary")]
    public string? Summary { get; set; }
}
