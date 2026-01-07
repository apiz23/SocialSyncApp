using SocialSyncApp.Models;
using System.Linq;

namespace SocialSyncApp.Services
{
    public class UserService : SupabaseService
    {
        public async Task<List<User>?> GetUsers()
            => await GetAsync<List<User>>("social_sync_users?select=*");

        public async Task<bool> CreateUser(User user)
        {
            return await PostAsync("social_sync_users", new
            {
                fullname = user.FullName,
                email = user.Email,
                phone = user.Phone,
                bio = user.Bio
            });
        }

        public async Task<bool> UpdateUser(Guid id, User user)
        {
            return await PatchAsync($"social_sync_users?id=eq.{id}", new
            {
                fullname = user.FullName,
                phone = user.Phone,
                bio = user.Bio,
                persona1 = user.Persona1,
                persona2 = user.Persona2,
                persona3 = user.Persona3
            });
        }

        public async Task<bool> DeleteUser(Guid id)
            => await DeleteAsync($"social_sync_users?id=eq.{id}");

        public async Task<User?> GetUserById(Guid id)
        {
            var users = await GetAsync<List<User>>(
                $"social_sync_users?id=eq.{id}&select=*"
            );

            return users?.FirstOrDefault();
        }
    }
}