using SocialSyncApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialSyncApp.Services
{
    public class PostService : SupabaseService
    {
        // Get all posts
        public async Task<List<Post>?> GetPosts()
        {
            return await GetAsync<List<Post>>(
                "social_sync_posts?select=*&order=created_at.desc"
            );
        }

        // Create post
        public async Task<bool> CreatePost(Post post, Guid authorId)
        {
            return await PostAsync("social_sync_posts", new
            {
                title = post.Title,
                content = post.Content,
                author = post.Author,
                category = post.Category,
                image_url = post.ImageUrl,
                author_id = authorId
            });
        }

        // Update post
        public async Task<bool> UpdatePost(long id, Post post)
        {
            return await PatchAsync($"social_sync_posts?id=eq.{id}", new
            {
                title = post.Title,
                content = post.Content,
                category = post.Category,
                image_url = post.ImageUrl
            });
        }

        // Delete post
        public async Task<bool> DeletePost(long id)
        {
            return await DeleteAsync($"social_sync_posts?id=eq.{id}");
        }
    }
}
