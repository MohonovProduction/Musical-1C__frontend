using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Musical1C__frontend.Services;

public record MusicianResponse
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("lastName")]
    public string LastName { get; set; }

    [JsonProperty("surname")]
    public string Surname { get; set; }

    [JsonProperty("instruments")]
    public List<Instrument> Instruments { get; set; }

    [JsonProperty("concerts")]
    public List<Concert> Concerts { get; set; }
}