using System.Collections.Generic;
using Newtonsoft.Json;

namespace Musical1C__frontend.Services.Requests;

public record ConcertRequest
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("date")] public string Date { get; set; }
    [JsonProperty("musicians")] public List<MusicianIdRequest> Musicians { get; set; }
    [JsonProperty("sounds")] public List<SoundIdRequest> Sounds { get; set; }
}