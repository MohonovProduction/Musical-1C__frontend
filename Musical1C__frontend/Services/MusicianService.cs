using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Musical1C__frontend.Services
{
    public class MusicianService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5017/api/Musician"; // Замените на ваш адрес API

        public MusicianService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        // Получение всех музыкантов
        public async Task<List<MusicianResponse>> GetMusiciansAsync(CancellationToken token = default)
        {
            var response = await _httpClient.GetAsync(BaseUrl, token);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(token);
            return JsonConvert.DeserializeObject<List<MusicianResponse>>(content);
        }

        // Получение музыканта по ID
        public async Task<MusicianResponse> GetMusicianByIdAsync(Guid id, CancellationToken token = default)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}", token);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error fetching musician with ID {id}: {response.ReasonPhrase}");

            var content = await response.Content.ReadAsStringAsync(token);
            return JsonConvert.DeserializeObject<MusicianResponse>(content);
        }

        // Добавление музыканта
        public async Task<MusicianResponse> AddMusicianAsync(MusicianRequest request, CancellationToken token = default)
        {
            var jsonContent = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(BaseUrl, content, token);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error adding musician: {response.ReasonPhrase}");

            var responseContent = await response.Content.ReadAsStringAsync(token);
            return JsonConvert.DeserializeObject<MusicianResponse>(responseContent);
        }

        // Удаление музыканта по ID
        public async Task DeleteMusicianAsync(Guid id, CancellationToken token = default)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}", token);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error deleting musician with ID {id}: {response.ReasonPhrase}");
        }
    }

    // Модели для десериализации данных
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

    public record Instrument
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public record Concert
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }

    public record MusicianRequest
    {
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
}