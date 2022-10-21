using System.Net.Http.Json;
using System.Text.Json;
using Domain.DTOs;
using Domain.Models;
using HttpClients.ClientInterfaces;

namespace HttpClients.Implementations;

public class TodoHttpClient:ITodoService
{
    private readonly HttpClient client;

    public TodoHttpClient(HttpClient client)
    {
        this.client = client;
    }

    public async Task CreateAsync(TodoCreationDto dto)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("/Todos", dto);
       

        if (!response.IsSuccessStatusCode)
        {
            String result = await response.Content.ReadAsStringAsync();
            throw new Exception(result);
        }
    }

    public async Task<ICollection<Todo>> GetAsync(string? userName, int? userId, bool? completedStatus, string? titleContains)
    {
        String query = ConstructQuery(userName, userId, completedStatus, titleContains);
        HttpResponseMessage response = await client.GetAsync("Todos"+query);
        String content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        ICollection<Todo> todos = JsonSerializer.Deserialize<ICollection<Todo>>(content, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        }) !;

        return todos;
    }

    private static String ConstructQuery(string? userName, int? userId, bool? completedStatus, string? titleContains)
    {
        String result = "";

        if (!String.IsNullOrEmpty(userName))
        {
            result += $"?username={userName}";
        }

        if (userId!=null)
        {
            result += String.IsNullOrEmpty(result) ? "?" : "&";
            result += $"userId={userId}";
        }

        if (completedStatus!=null)
        {
            result += String.IsNullOrEmpty(result) ? "?" : "&";
            result += $"completedStatus={completedStatus}";
        }

        if (!String.IsNullOrEmpty(titleContains))
        {
            result += String.IsNullOrEmpty(result) ? "?" : "&";
            result += $"titleContains={titleContains}";
        }

        return result;
    }
}