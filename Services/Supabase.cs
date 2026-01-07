using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Diagnostics;

public class SupabaseService
{
    protected readonly HttpClient _http;

    public SupabaseService()
    {
        _http = new HttpClient
        {
            // KEEPING YOUR URL
            BaseAddress = new Uri("https://otmlfmgyscrohtbpqqoi.supabase.co/rest/v1/")
        };

        // KEEPING YOUR KEYS
        string apiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im90bWxmbWd5c2Nyb2h0YnBxcW9pIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MTc4MjkxNjUsImV4cCI6MjAzMzQwNTE2NX0.LkJePEXbi7jvDWzopTOdPmuh-UCTmY83NQYpvxaxuTE";

        _http.DefaultRequestHeaders.Add("apikey", apiKey);
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    // Helper to log errors safely
    protected async Task LogErrorIfFailed(HttpResponseMessage res, string operation)
    {
        if (!res.IsSuccessStatusCode)
        {
            var content = await res.Content.ReadAsStringAsync();
            Debug.WriteLine($"🔴 [Supabase {operation} Error] Status: {res.StatusCode}");
            Debug.WriteLine($"🔴 Body: {content}");
        }
    }

    protected async Task<string> GetRawJsonAsync(string url)
    {
        var res = await _http.GetAsync(url);
        var json = await res.Content.ReadAsStringAsync();
        Debug.WriteLine($"HTTP Status: {res.StatusCode}");

        if (!res.IsSuccessStatusCode)
        {
            Debug.WriteLine($"[Error Body]: {json}");
        }

        return json;
    }

    // ✅ CRITICAL FIX HERE
    protected async Task<T?> GetAsync<T>(string url)
    {
        try
        {
            var res = await _http.GetAsync(url);
            var json = await res.Content.ReadAsStringAsync();

            // 1. Check if the request actually worked
            if (!res.IsSuccessStatusCode)
            {
                Debug.WriteLine($"🔴 [GetAsync Error] URL: {url}");
                Debug.WriteLine($"🔴 Status: {res.StatusCode}");
                Debug.WriteLine($"🔴 Response: {json}");

                // Return default (null) instead of crashing the JSON parser
                return default;
            }

            // 2. Only deserialize if successful
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"🔴 [Exception] {ex.Message}");
            return default;
        }
    }

    protected async Task<bool> PostAsync(string url, object data)
    {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await _http.PostAsync(url, content);

        await LogErrorIfFailed(res, "POST");
        return res.IsSuccessStatusCode;
    }

    protected async Task<bool> PatchAsync(string url, object data)
    {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await _http.PatchAsync(url, content);

        await LogErrorIfFailed(res, "PATCH");
        return res.IsSuccessStatusCode;
    }

    protected async Task<bool> DeleteAsync(string url)
    {
        var res = await _http.DeleteAsync(url);
        await LogErrorIfFailed(res, "DELETE");
        return res.IsSuccessStatusCode;
    }
}