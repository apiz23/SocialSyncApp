namespace SocialSyncApp.Models;

public class EventParticipant
{
    public long Id { get; set; }
    public long EventId { get; set; }
    public Guid UserId { get; set; }
    public string UserEmail { get; set; } = "";
    public DateTime JoinedAt { get; set; }
}
