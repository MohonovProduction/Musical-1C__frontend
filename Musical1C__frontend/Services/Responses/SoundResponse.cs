using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Musical1C__frontend.Services.Responses;

public record SoundResponse
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("author")]
    public string Author { get; set; }

    [JsonProperty("soundOnConcerts")]
    public List<SoundOnConcertResponse> SoundOnConcerts { get; set; }
}