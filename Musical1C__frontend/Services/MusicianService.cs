using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Musical1C__frontend.Services.ResReq;

namespace Musical1C__frontend.Services
{
    public class MusicianService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5017/api/Musician";

        public MusicianService()
        {
            _httpClient = new HttpClient{BaseAddress = new Uri(BaseUrl)};
        }

        public async Task<MusicianResponse> AddMusicianAsync(AddMusicianRequest request, CancellationToken token = default)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, request, token);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<MusicianResponse>(cancellationToken: token);
        }

        public async Task<MusicianResponse> GetMusicianByIdAsync(Guid id, CancellationToken token = default)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}", token);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<MusicianResponse>(cancellationToken: token);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            return null;
        }

        public async Task<List<MusicianResponse>> GetMusiciansAsync(CancellationToken token = default)
        {
            var response = await _httpClient.GetAsync(BaseUrl, token);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<MusicianResponse>>(cancellationToken: token);
        }

        public async Task<List<MusicianResponse>> SearchMusiciansByLastNameAsync(string lastName, CancellationToken token = default)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/search?lastname={lastName}", token);
            response.EnsureSuccessStatusCode();
            
            Console.WriteLine(response.Content);
            
            return await response.Content.ReadFromJsonAsync<List<MusicianResponse>>(cancellationToken: token);
        }

        public async Task DeleteMusicianAsync(Guid id, CancellationToken token = default)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}", token);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException("Musician not found.");
            }

            response.EnsureSuccessStatusCode();
        }
    }
}
