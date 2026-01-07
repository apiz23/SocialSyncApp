using SocialSyncApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialSyncApp.Services
{
    public class EventService : SupabaseService
    {
        // Get all events
        public async Task<List<Event>?> GetEvents()
        {
            return await GetAsync<List<Event>>(
                "social_sync_events?select=*&order=event_date.asc"
            );
        }

        // Create event
        public async Task<bool> CreateEvent(Event evt, Guid creatorId, string email)
        {
            return await PostAsync("social_sync_events", new
            {
                title = evt.Title,
                description = evt.Description,
                event_date = evt.EventDate,
                location = evt.Location,
                max_participants = evt.MaxParticipants,
                created_by = email,
                creator_id = creatorId,
                image_url = evt.ImageUrl
            });
        }

        // Update event
        public async Task<bool> UpdateEvent(long id, Event evt)
        {
            return await PatchAsync($"social_sync_events?id=eq.{id}", new
            {
                title = evt.Title,
                description = evt.Description,
                event_date = evt.EventDate,
                location = evt.Location,
                max_participants = evt.MaxParticipants,
                image_url = evt.ImageUrl
            });
        }

        // Delete event
        public async Task<bool> DeleteEvent(long id)
        {
            return await DeleteAsync($"social_sync_events?id=eq.{id}");
        }
    }
}
