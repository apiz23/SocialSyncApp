using SocialSyncApp.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace SocialSyncApp.Services;

public class AuthService : SupabaseService
{
    public async Task<User?> Login(string email, string password)
    {
        Debug.WriteLine("=== LOGIN DEBUG START ===");
        Debug.WriteLine($"Input Email: {email}");

        try
        {
            var encodedEmail = Uri.EscapeDataString(email);

            var url = $"social_sync_users?email=eq.{encodedEmail}&select=id,email,password,is_active,fullname,phone,bio,persona1,persona2,persona3";

            Debug.WriteLine($"Query URL: {url}");

            // Get raw JSON response
            var rawJson = await GetRawJsonAsync(url);
            Debug.WriteLine($"Raw JSON Response: {rawJson}");

            if (string.IsNullOrEmpty(rawJson) || rawJson == "[]")
            {
                Debug.WriteLine("❌ NO USER FOUND - Empty response");
                return null;
            }

            var users = JsonConvert.DeserializeObject<List<User>>(rawJson);

            if (users == null || users.Count == 0)
            {
                Debug.WriteLine("❌ NO USER FOUND");
                return null;
            }

            var user = users[0];
            Debug.WriteLine($"User ID: {user.Id}");
            Debug.WriteLine($"User IsActive: {user.IsActive}");

            if (string.IsNullOrEmpty(user.Password))
            {
                Debug.WriteLine("❌ PASSWORD FIELD IS NULL OR EMPTY IN DB");
                return null;
            }

            // Verify Password
            bool valid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            Debug.WriteLine($"BCrypt Verify Result: {valid}");

            if (!valid)
            {
                Debug.WriteLine("❌ PASSWORD MISMATCH");
                return null;
            }

            if (!user.IsActive)
            {
                Debug.WriteLine("❌ USER NOT ACTIVE");
                return null;
            }

            SaveSession(user);

            Debug.WriteLine("✅ LOGIN SUCCESS");
            return user;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ EXCEPTION: {ex.Message}");
            throw;
        }
    }

    public void SaveSession(User user)
    {
        Preferences.Set("IsLoggedIn", true);
        Preferences.Set("UserId", user.Id.ToString());
        Preferences.Set("UserEmail", user.Email);
        Preferences.Set("UserName", user.FullName ?? "");

        // Optional: Save other details if needed often
        if (!string.IsNullOrEmpty(user.Persona1))
            Preferences.Set("UserPersona1", user.Persona1);
    }

    public void ClearSession()
    {
        Preferences.Clear();
    }
}