using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AvaloniaUI.Services
{
    public class ConcertService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5017/api/Concerts"; // Замените на ваш адрес API

        public ConcertService()
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

        // Получить список концертов
        public async Task<List<ConcertResponse>> GetConcertsAsync(CancellationToken token = default)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/all", token);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error fetching concerts: {response.ReasonPhrase}");
            }

            var content = await response.Content.ReadAsStringAsync(token);
            return JsonConvert.DeserializeObject<List<ConcertResponse>>(content, JsonSettings);
        }

        // Получить концерт по ID
        public async Task<ConcertResponse> GetConcertByIdAsync(Guid id, CancellationToken token = default)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}", token);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error fetching concert with ID {id}: {response.ReasonPhrase}");
            }

            var content = await response.Content.ReadAsStringAsync(token);
            return JsonConvert.DeserializeObject<ConcertResponse>(content, JsonSettings);
        }

        // Добавить концерт
        public async Task<ConcertResponse> AddConcertAsync(ConcertRequest request, CancellationToken token = default)
        {
            var jsonContent = JsonConvert.SerializeObject(request, JsonSettings);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(string.Empty, content, token);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error adding concert: {response.ReasonPhrase}");
            }

            var responseContent = await response.Content.ReadAsStringAsync(token);
            return JsonConvert.DeserializeObject<ConcertResponse>(responseContent, JsonSettings);
        }

        // Удалить концерт по ID
        public async Task DeleteConcertAsync(Guid id, CancellationToken token = default)
        {
            var response = await _httpClient.DeleteAsync($"{id}", token);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error deleting concert with ID {id}: {response.ReasonPhrase}");
            }
        }

        // Добавить музыканта к концерту
        public async Task AddMusicianToConcertAsync(Guid concertId, Guid musicianId, CancellationToken token = default)
        {
            var response = await _httpClient.PostAsync($"{concertId}/Musicians/{musicianId}", null, token);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error adding musician to concert: {response.ReasonPhrase}");
            }
        }

        // Удалить музыканта из концерта
        public async Task RemoveMusicianFromConcertAsync(Guid concertId, Guid musicianId, CancellationToken token = default)
        {
            var response = await _httpClient.DeleteAsync($"{concertId}/Musicians/{musicianId}", token);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error removing musician from concert: {response.ReasonPhrase}");
            }
        }

        // Добавить звук к концерту
        public async Task AddSoundToConcertAsync(Guid concertId, Guid soundId, CancellationToken token = default)
        {
            var response = await _httpClient.PostAsync($"{concertId}/Sounds/{soundId}", null, token);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error adding sound to concert: {response.ReasonPhrase}");
            }
        }

        // Удалить звук из концерта
        public async Task RemoveSoundFromConcertAsync(Guid concertId, Guid soundId, CancellationToken token = default)
        {
            var response = await _httpClient.DeleteAsync($"{concertId}/Sounds/{soundId}", token);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error removing sound from concert: {response.ReasonPhrase}");
            }
        }
    }

    // Модели данных
    public class ConcertResponse
    {
        [JsonProperty("id")] public Guid Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("type")] public string Type { get; set; }
        [JsonProperty("date")] public string Date { get; set; }
        [JsonProperty("musicians")] public List<MusicianResponse> Musicians { get; set; }
        [JsonProperty("sounds")] public List<SoundResponse> Sounds { get; set; }
    }

    public class MusicianResponse
    {
        [JsonProperty("id")] public Guid Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("lastName")] public string LastName { get; set; }
        [JsonProperty("surname")] public string Surname { get; set; }
    }

    public class SoundResponse
    {
        [JsonProperty("id")] public Guid Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("author")] public string Author { get; set; }
    }

    public class ConcertRequest
    {
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("type")] public string Type { get; set; }
        [JsonProperty("date")] public string Date { get; set; }
        [JsonProperty("musicians")] public List<MusicianIdRequest> Musicians { get; set; }
        [JsonProperty("sounds")] public List<SoundIdRequest> Sounds { get; set; }
    }

    public class MusicianIdRequest
    {
        [JsonProperty("id")] public Guid Id { get; set; }
    }

    public class SoundIdRequest
    {
        [JsonProperty("id")] public Guid Id { get; set; }
    }
}
