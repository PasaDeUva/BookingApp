using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BotWhatsapp.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using BotWhatsapp.Application.DTOs;

namespace BotWhatsapp.Application.Services;

public class EvolutionService : IEvolutionService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _instance;
    private readonly string _apiKey;

    public EvolutionService(IConfiguration config)
    {
        _httpClient = new HttpClient();
        _baseUrl = config["EvolutionApi:BaseUrl"] ?? throw new Exception("Falta EvolutionApi:BaseUrl");
        _instance = config["EvolutionApi:Instance"] ?? throw new Exception("Falta EvolutionApi:Instance");
        _apiKey = config["EvolutionApi:ApiKey"] ?? throw new Exception("Falta EvolutionApi:ApiKey");
    }

    public async Task SendMessageAsync(string number, string text)
    {
        try
        {
            var url = $"{_baseUrl}/message/sendText/{_instance}";
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("apikey", _apiKey);

            var body = new
            {
                number,
                options = new
                {
                    delay = 500,
                    presence = "composing",
                    linkPreview = false
                },
                textMessage = new { text }
            };

            request.Content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al enviar mensaje: {response.StatusCode} – {error}");
            }

        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public async Task<Dictionary<string, object>> RestartInstanceAsync()
    {
        var url = $"{_baseUrl}/instance/restart/{_instance}";
        var request = new HttpRequestMessage(HttpMethod.Put, url);
        request.Headers.Add("apikey", _apiKey);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error al reiniciar instancia: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Dictionary<string, object>>(content);
        return result ?? throw new Exception("No se pudo obtener la respuesta del reinicio");
    }

    public async Task<WhatsAppConnectionDto> GetInstancesAsync()
    {
        var url = $"{_baseUrl}/instance/connect/{_instance}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("apikey", _apiKey);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error al obtener QR: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var connectionDto = JsonSerializer.Deserialize<WhatsAppConnectionDto>(content);

        if (connectionDto == null)
        {
            throw new Exception("No se pudo deserializar la respuesta");
        }

        return connectionDto;
    }


    public async Task<(string Status, string ExternalReference)> ObtenerEstadoPagoAsync(long paymentId)
    {
        var url = $"https://api.mercadopago.com/v1/payments/{paymentId}";

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "TU_ACCESS_TOKEN");
        var response = await client.GetAsync(url);

        var result = await response.Content.ReadFromJsonAsync<JsonElement>();

        string status = result.GetProperty("status").GetString();
        string reference = result.GetProperty("external_reference").GetString();

        return (status, reference);
    }


    public async Task<WhatsAppDisconnectionDto> DisconectInstanceAsync()
    {
        var url = $"{_baseUrl}/instance/logout/{_instance}";
        var request = new HttpRequestMessage(HttpMethod.Delete, url);
        request.Headers.Add("apikey", _apiKey);

        var response = await _httpClient.SendAsync(request);

        var readed = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("La instancia ya esta desvinculada");
        }

        return JsonSerializer.Deserialize<WhatsAppDisconnectionDto>(await response.Content.ReadAsStringAsync());
    }

}
