using System;
using Newtonsoft.Json;

namespace SocialSyncApp.Models;

public class Friend
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("user_id")]
    public Guid UserId { get; set; }

    [JsonProperty("friend_id")]
    public Guid FriendId { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("friend")]
    public FriendProfile? Profile { get; set; }

    // --- Helpers ---
    [JsonIgnore]
    public string DisplayName => !string.IsNullOrEmpty(Profile?.FullName) ? Profile.FullName : "Unknown User";

    [JsonIgnore]
    public string DisplayJob => !string.IsNullOrEmpty(Profile?.Email) ? Profile.Email : "No contact info";

    [JsonIgnore]
    public string SinceLabel => $"✅ Friends since {CreatedAt.Year}";

    [JsonIgnore]
    public string AvatarUrl => $"https://ui-avatars.com/api/?name={Uri.EscapeDataString(DisplayName)}&background=random&color=fff&size=128";
}

public class FriendProfile
{
    // ✅ FIX: Changed property name to "fullname"
    [JsonProperty("fullname")]
    public string FullName { get; set; } = "";

    [JsonProperty("email")]
    public string Email { get; set; } = "";
}