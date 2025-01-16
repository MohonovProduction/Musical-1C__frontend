using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Musical1C__frontend.Services
{
    public class SoundsService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:5001/api/Sounds"; // Замените на ваш адрес API

        public SoundsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Получение всех звуков
        public async Task<List<SoundResponse>> GetSoundsAsync(CancellationToken token)
        {
            var response = await _httpClient.GetAsync(BaseUrl, token);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(token);
            return JsonSerializer.Deserialize<List<SoundResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // Получение звука по ID
        public async Task<SoundResponse> GetSoundByIdAsync(Guid id, CancellationToken token)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}", token);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(token);
                return JsonSerializer.Deserialize<SoundResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            return null;
        }

        // Добавление нового звука
        public async Task AddSoundAsync(string name, string author, CancellationToken token)
        {
            var requestBody = new
            {
                Name = name,
                Author = author
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(BaseUrl, content, token);
            response.EnsureSuccessStatusCode();
        }

        // Удаление звука
        public async Task DeleteSoundAsync(Guid id, CancellationToken token)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}", token);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception($"Sound with ID {id} not found.");
            }

            response.EnsureSuccessStatusCode();
        }
    }

    // DTO классы для сериализации/десериализации
    public class SoundResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public List<SoundOnConcertResponse> SoundOnConcerts { get; set; }
    }

    public class SoundOnConcertResponse
    {
        public Guid SoundId { get; set; }
        public Guid ConcertId { get; set; }
    }
}
