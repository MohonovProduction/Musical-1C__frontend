using System;
using System.Collections.Generic;
using Musical1C__frontend.Services.Responses;
using Newtonsoft.Json;

namespace Musical1C__frontend.Services;

public class ConcertResponse
{
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("date")] public string Date { get; set; }
    [JsonProperty("musicians")] public List<MusicianResponse> Musicians { get; set; }
    [JsonProperty("sounds")] public List<SoundResponse> Sounds { get; set; }
}