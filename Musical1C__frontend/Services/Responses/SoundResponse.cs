using System;
using Newtonsoft.Json;

namespace Musical1C__frontend.Services.Responses;

public class SoundResponse
{
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("author")] public string Author { get; set; }
}