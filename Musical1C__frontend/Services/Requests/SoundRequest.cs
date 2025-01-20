using Newtonsoft.Json;

namespace Musical1C__frontend.Services.Requests;

public record SoundRequest
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("author")]
    public string Author { get; set; }
}