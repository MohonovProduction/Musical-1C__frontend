using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Musical1C__frontend.Services.ResReq;
using WebStation.Trains;

namespace Musical1C__frontend.Services;

public class SoundService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://localhost:5017/api/Sound";

    public SoundService()
    {
        _httpClient = new HttpClient{BaseAddress = new Uri(BaseUrl)};
    }

    // Добавление музыкального произведения
    public async Task<SoundResponse> AddMusicAsync(AddSoundRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseUrl, request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<SoundResponse>(cancellationToken: cancellationToken);
        }

        var error = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new HttpRequestException($"Error adding music: {response.StatusCode}, {error}");
    }

    // Удаление музыкального произведения
    public async Task DeleteMusicAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"Error deleting music: {response.StatusCode}, {error}");
        }
    }

    // Получение списка всех музыкальных произведений
    public async Task<List<SoundResponse>> GetAllMusicAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(BaseUrl, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<SoundResponse>>(cancellationToken: cancellationToken);
        }

        var error = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new HttpRequestException($"Error fetching music list: {response.StatusCode}, {error}");
    }

    // Получение музыкального произведения по ID
    public async Task<SoundResponse> GetMusicByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}/{id}", cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<SoundResponse>(cancellationToken: cancellationToken);
        }

        var error = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new HttpRequestException($"Error fetching music by ID: {response.StatusCode}, {error}");
    }
}