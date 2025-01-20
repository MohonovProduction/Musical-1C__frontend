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

            var response = await _httpClient.PostAsync($"{BaseUrl}", content, token);

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
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}", token);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error deleting concert with ID {id}: {response.ReasonPhrase}");
            }
        }

        // Добавить музыканта к концерту
        public async Task AddMusicianToConcertAsync(Guid concertId, Guid musicianId, CancellationToken token = default)
        {
            var response = await _httpClient.PostAsync($"{BaseUrl}/{concertId}/Musicians/{musicianId}", null, token);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error adding musician to concert: {response.ReasonPhrase}");
            }
        }

        // Удалить музыканта из концерта
        public async Task RemoveMusicianFromConcertAsync(Guid concertId, Guid musicianId, CancellationToken token = default)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{concertId}/Musicians/{musicianId}", token);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error removing musician from concert: {response.ReasonPhrase}");
            }
        }

        // Добавить звук к концерту
        public async Task AddSoundToConcertAsync(Guid concertId, Guid soundId, CancellationToken token = default)
        {
            var response = await _httpClient.PostAsync($"{BaseUrl}/{concertId}/Sounds/{soundId}", null, token);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error adding sound to concert: {response.ReasonPhrase}");
            }
        }

        // Удалить звук из концерта
        public async Task RemoveSoundFromConcertAsync(Guid concertId, Guid soundId, CancellationToken token = default)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{concertId}/Sounds/{soundId}", token);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error removing sound from concert: {response.ReasonPhrase}");
            }
        }

    }
}
