using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SocialSyncApp.Models;

public class Post : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; } = "";

    [JsonProperty("content")]
    public string Content { get; set; } = "";

    [JsonProperty("author")]
    public string Author { get; set; } = "";

    [JsonProperty("category")]
    public string Category { get; set; } = "";

    [JsonProperty("image_url")]
    public string? ImageUrl { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("author_id")]
    public Guid? AuthorId { get; set; }

    // --- UI Helpers ---

    [JsonIgnore]
    public bool HasImage => !string.IsNullOrEmpty(ImageUrl);

    [JsonIgnore]
    public string AvatarUrl => $"https://ui-avatars.com/api/?name={Uri.EscapeDataString(Author)}&background=random&color=fff&size=128";

    private bool _isOwner;

    [JsonIgnore]
    public bool IsOwner
    {
        get => _isOwner;
        set
        {
            if (_isOwner != value)
            {
                _isOwner = value;
                OnPropertyChanged();
            }
        }
    }

    [JsonIgnore]
    public string TimeAgo
    {
        get
        {
            var span = DateTime.UtcNow - CreatedAt.ToUniversalTime();
            if (span.TotalHours < 1) return $"{Math.Max(1, span.Minutes)}m ago";
            if (span.TotalHours < 24) return $"{span.Hours}h ago";
            if (span.TotalDays < 7) return $"{span.Days}d ago";
            return CreatedAt.ToString("MMM dd");
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}