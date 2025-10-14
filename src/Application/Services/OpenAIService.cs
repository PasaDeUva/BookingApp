using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BotWhatsapp.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BotWhatsapp.Application.Services;

public class OpenAiService : IOpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OpenAiService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _apiKey = configuration["OpenAi:ApiKey"] ?? throw new Exception("Falta configurar OpenAi:ApiKey en appsettings.json");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<DateTime?> ExtractDateFromText(string message)
    {
        var prompt = $@"
            Sos un asistente que extrae fechas y horarios de mensajes humanos.
            Dado este mensaje:
            ""{message}""
            
            Respondé **solo** con la fecha y hora en formato ISO-8601: AAAA-MM-DD HH:mm
            (no agregues texto adicional, ni comillas, ni comas).
            
            Si no hay fecha clara, respondé solo con: null.
            
            Fecha actual: {DateTime.Now:yyyy-MM-dd HH:mm}
            ";

        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
            new { role = "user", content = prompt }
        }
        };

        var json = JsonSerializer.Serialize(requestBody);

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var response = await _httpClient.SendAsync(httpRequest);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseString);

        var content = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString()
            ?.Trim()
            .ToLower();

        if (string.IsNullOrWhiteSpace(content) || content == "null")
            return null;

        // Intentamos parsear formato estricto "yyyy-MM-dd HH:mm"
        if (DateTime.TryParseExact(content, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
            return parsed;

        // Fallback a parseo flexible
        if (DateTime.TryParse(content, out parsed))
            return parsed;

        return null;
    }

    public async Task<string> SendToOpenAIAsync(string prompt)
    {
        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            temperature = 0,
            messages = new[]
            {
            new { role = "user", content = prompt }
        }
        };

        var json = JsonSerializer.Serialize(requestBody);

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var response = await _httpClient.SendAsync(httpRequest);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseString);

        return doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? "";
    }

}
