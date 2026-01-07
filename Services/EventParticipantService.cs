using SocialSyncApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialSyncApp.Services
{
    public class EventParticipantService : SupabaseService
    {
        // Join event
        public async Task<bool> JoinEvent(long eventId, Guid userId, string email)
        {
            return await PostAsync("social_sync_event_participants", new
            {
                event_id = eventId,
                user_id = userId,
                user_email = email
            });
        }

        // Leave event
        public async Task<bool> LeaveEvent(long eventId, Guid userId)
        {
            return await DeleteAsync(
                $"social_sync_event_participants?event_id=eq.{eventId}&user_id=eq.{userId}"
            );
        }

        // Get participants by event
        public async Task<List<EventParticipant>?> GetParticipants(long eventId)
        {
            return await GetAsync<List<EventParticipant>>(
                $"social_sync_event_participants?event_id=eq.{eventId}"
            );
        }
    }
}
