using System;
using Newtonsoft.Json;

namespace Musical1C__frontend.Services.Requests;

public record SoundIdRequest
{
    [JsonProperty("id")] public Guid Id { get; set; }
}