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
    public class MusicianService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5017/api/Musician"; // Замените на ваш адрес API

        public MusicianService()
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

        // Получение всех музыкантов
        public async Task<List<MusicianResponse>> GetMusiciansAsync(CancellationToken token = default)
        {
            var response = await _httpClient.GetAsync(BaseUrl, token);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(token);
            
            Console.WriteLine(content);
            
            Console.WriteLine(JsonConvert.DeserializeObject<List<MusicianResponse>>(content, JsonSettings));
            
            return JsonConvert.DeserializeObject<List<MusicianResponse>>(content, JsonSettings);
        }

        // Получение музыканта по ID
        public async Task<MusicianResponse> GetMusicianByIdAsync(Guid id, CancellationToken token = default)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}", token);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error fetching musician with ID {id}: {response.ReasonPhrase}");

            var content = await response.Content.ReadAsStringAsync(token);
            return JsonConvert.DeserializeObject<MusicianResponse>(content, JsonSettings);
        }

        // Добавление музыканта
        public async Task<MusicianResponse> AddMusicianAsync(MusicianRequest request, CancellationToken token = default)
        {
            var jsonContent = JsonConvert.SerializeObject(request, JsonSettings);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(BaseUrl, content, token);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error adding musician: {response.ReasonPhrase}");

            var responseContent = await response.Content.ReadAsStringAsync(token);
            return JsonConvert.DeserializeObject<MusicianResponse>(responseContent, JsonSettings);
        }

        // Удаление музыканта по ID
        public async Task DeleteMusicianAsync(Guid id, CancellationToken token = default)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}", token);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error deleting musician with ID {id}: {response.ReasonPhrase}");
        }

        // Метод для поиска музыкантов по фамилии
        public async Task<List<MusicianResponse>?> SearchMusiciansByLastNameAsync(string lastName,
            CancellationToken token = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(lastName))
                    throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

                var response = await _httpClient.GetAsync($"{BaseUrl}/search?lastName={Uri.EscapeDataString(lastName)}",
                    token);

                if (!response.IsSuccessStatusCode)
                    throw new Exception(
                        $"Error searching musicians by last name \"{lastName}\": {response.ReasonPhrase}");

                var content = await response.Content.ReadAsStringAsync(token);

                return JsonConvert.DeserializeObject<List<MusicianResponse>>(content, JsonSettings);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    // Модели для десериализации данных

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
}