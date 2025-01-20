using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Musical1C__frontend.Services.Requests;
using Musical1C__frontend.Services.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Musical1C__frontend.Services
{
    public class SoundService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5017/api/Sounds"; // Replace with your API address

        public SoundService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        private static JsonSerializerSettings JsonSettings => new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        // Get all sounds
        public async Task<List<SoundResponse>> GetSoundsAsync(CancellationToken token = default)
        {
            var response = await _httpClient.GetAsync(BaseUrl, token);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(token);
            return JsonConvert.DeserializeObject<List<SoundResponse>>(content, JsonSettings);
        }

        // Get sound by ID
        public async Task<SoundResponse> GetSoundByIdAsync(Guid id, CancellationToken token = default)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}", token);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error fetching sound with ID {id}: {response.ReasonPhrase}");

            var content = await response.Content.ReadAsStringAsync(token);
            return JsonConvert.DeserializeObject<SoundResponse>(content, JsonSettings);
        }

        // Add a sound
        public async Task<SoundResponse> AddSoundAsync(SoundRequest request, CancellationToken token = default)
        {
            var jsonContent = JsonConvert.SerializeObject(request, JsonSettings);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(BaseUrl, content, token);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error adding sound: {response.ReasonPhrase}");

            var responseContent = await response.Content.ReadAsStringAsync(token);
            return JsonConvert.DeserializeObject<SoundResponse>(responseContent, JsonSettings);
        }

        // Delete a sound by ID
        public async Task DeleteSoundAsync(Guid id, CancellationToken token = default)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}", token);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error deleting sound with ID {id}: {response.ReasonPhrase}");
        }
    }

    // Models for deserialization

    public record SoundOnConcertResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("soundId")]
        public Guid SoundId { get; set; }

        [JsonProperty("concertId")]
        public Guid ConcertId { get; set; }
    }
}
