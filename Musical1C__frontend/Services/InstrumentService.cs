using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Musical1C__frontend.Services
{
    public class InstrumentsService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:5001/api/Instruments"; // Замените на ваш адрес API

        public InstrumentsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Получение всех инструментов
        public async Task<List<InstrumentResponse>> GetInstrumentsAsync(CancellationToken token)
        {
            var response = await _httpClient.GetAsync(BaseUrl, token);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(token);
            return JsonSerializer.Deserialize<List<InstrumentResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // Получение инструмента по ID
        public async Task<InstrumentResponse> GetInstrumentByIdAsync(Guid id, CancellationToken token)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}", token);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(token);
                return JsonSerializer.Deserialize<InstrumentResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            return null;
        }

        // Добавление инструмента
        public async Task AddInstrumentAsync(Guid id, string name, CancellationToken token)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { id, name }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(BaseUrl, content, token);
            response.EnsureSuccessStatusCode();
        }

        // Удаление инструмента
        public async Task DeleteInstrumentAsync(Guid id, CancellationToken token)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}", token);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception($"Instrument with ID {id} not found.");
            }

            response.EnsureSuccessStatusCode();
        }
    }

    // DTO классы для сериализации/десериализации
    public partial class InstrumentResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
