using SocialSyncApp.Models;

namespace SocialSyncApp.Services
{
    public class FriendService : SupabaseService
    {
        public async Task<List<Friend>?> GetFriends(string userId)
        {
            string query = $"social_sync_friends?select=*,friend:social_sync_users!friend_id(fullname,email)&user_id=eq.{userId}";

            return await GetAsync<List<Friend>>(query);
        }

        public async Task<bool> AddFriend(string userId, string friendId)
        {
            return await PostAsync("social_sync_friends", new
            {
                user_id = userId,
                friend_id = friendId
            });
        }

        public async Task<bool> RemoveFriend(string userId, string friendId)
        {
            return await DeleteAsync(
                $"social_sync_friends?user_id=eq.{userId}&friend_id=eq.{friendId}"
            );
        }
    }
}