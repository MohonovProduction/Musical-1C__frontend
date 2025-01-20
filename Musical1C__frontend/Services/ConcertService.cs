using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Musical1C__frontend.Services.ResReq;

namespace Musical1C__frontend.Services;

public class ConcertService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "http://localhost:5017/api/Concert";

    public ConcertService()
    {
        _httpClient = new HttpClient{BaseAddress = new Uri(_baseUrl)};
    }

    public async Task<ConcertResponse> AddConcertAsync(AddConcertRequest request, CancellationToken token)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_baseUrl, content, token);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(token);
        return JsonSerializer.Deserialize<ConcertResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<ConcertResponse> GetConcertByIdAsync(Guid id, CancellationToken token)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/{id}", token);
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync(token);
            return JsonSerializer.Deserialize<ConcertResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        return null;
    }

    public async Task<List<ConcertResponse>> GetConcertsAsync(CancellationToken token)
    {
        var response = await _httpClient.GetAsync(_baseUrl, token);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(token);
        return JsonSerializer.Deserialize<List<ConcertResponse>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task AddMusicianToConcertAsync(Guid concertId, AddMusicianToConcertRequest request, CancellationToken token)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"{_baseUrl}/{concertId}/musician", content, token);
        response.EnsureSuccessStatusCode();
    }

    public async Task AddSoundToConcertAsync(Guid concertId, AddSoundToConcertRequest request, CancellationToken token)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"{_baseUrl}/{concertId}/sound", content, token);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteConcertAsync(Guid id, CancellationToken token)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}", token);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveMusicianFromConcertAsync(Guid concertId, RemoveMusicianFromConcertRequest request, CancellationToken token)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"{_baseUrl}/{concertId}/musician"),
            Content = content
        }, token);

        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveSoundFromConcertAsync(Guid concertId, RemoveSoundFromConcertRequest request, CancellationToken token)
    {
        var jsonContent = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"{_baseUrl}/{concertId}/sound"),
            Content = content
        }, token);

        response.EnsureSuccessStatusCode();
    }
}