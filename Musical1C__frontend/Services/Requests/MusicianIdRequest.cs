using System;
using Newtonsoft.Json;

namespace Musical1C__frontend.Services.Requests;

public record MusicianIdRequest
{
    public MusicianIdRequest(Guid id)
    {
        Id = id;
    }

    [JsonProperty("id")] public Guid Id { get; set; }
}