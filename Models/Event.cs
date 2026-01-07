using Newtonsoft.Json;
using System;

namespace SocialSyncApp.Models;

public class Event
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; } = "";

    [JsonProperty("description")]
    public string Description { get; set; } = "";

    [JsonProperty("event_date")]
    public DateTime EventDate { get; set; }

    [JsonProperty("location")]
    public string Location { get; set; } = "";

    [JsonProperty("max_participants")]
    public int? MaxParticipants { get; set; }

    [JsonProperty("created_by")]
    public string CreatedBy { get; set; } = "";

    // ✅ NEW: ID of the user who created the event (Matches DB column)
    [JsonProperty("creator_id")]
    public Guid CreatorId { get; set; }

    [JsonProperty("image_url")]
    public string? ImageUrl { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; } = "Upcoming";

    // --- UI Helpers (Not saved to DB) ---

    // ✅ NEW: Determines if the logged-in user owns this event
    [JsonIgnore]
    public bool IsOwner { get; set; }

    [JsonIgnore]
    public string FormattedDate => EventDate.ToString("MMM dd, yyyy");

    [JsonIgnore]
    public string FormattedTime => EventDate.ToString("h:mm tt");

    [JsonIgnore]
    public string DisplayImage => !string.IsNullOrEmpty(ImageUrl)
        ? ImageUrl
        : $"https://picsum.photos/seed/{Id}/400/200";

    [JsonIgnore]
    public string ParticipantsLabel => MaxParticipants.HasValue
        ? $"{MaxParticipants} Seats"
        : "Open";
}