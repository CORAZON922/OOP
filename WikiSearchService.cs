using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


public class WikiSearchService : IDisposable
{
    private readonly HttpClient _httpClient;

    public WikiSearchService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "WikiSearcher/1.0 (https://myapp.com; contact@myapp.com)");
    }

    /// <summary>
    /// Выполняет поиск в Википедии по заданному запросу
    /// </summary>
    /// <param name="searchQuery">Поисковый запрос</param>
    /// <returns>Результаты поиска</returns>
    public async Task<WikiSearchResult> SearchAsync(string searchQuery)
    {
        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            throw new ArgumentException("Запрос не может быть пустым.");
        }

        string encodedQuery = Uri.EscapeDataString(searchQuery);
        string requestUrl = $"https://ru.wikipedia.org/w/api.php?action=query&list=search&utf8=&format=json&srsearch={encodedQuery}";

        Console.WriteLine("Идет поиск...");
        HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
        response.EnsureSuccessStatusCode();

        string jsonResponse = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        return JsonSerializer.Deserialize<WikiSearchResult>(jsonResponse, options);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}