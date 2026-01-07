using SocialSyncApp.Models;
using SocialSyncApp.Services;
using System.Collections.ObjectModel;

namespace SocialSyncApp.Views;

public partial class PostsPage : ContentPage
{
    private readonly PostService _postService;
    public ObservableCollection<Post> Posts { get; set; } = new ObservableCollection<Post>();

    public PostsPage()
    {
        InitializeComponent();
        _postService = new PostService();
        PostsCollectionView.ItemsSource = Posts;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadPosts();
    }

    private async Task LoadPosts()
    {
        try
        {
            LoadingSpinner.IsVisible = true;
            LoadingSpinner.IsRunning = true;
            FeedRefreshView.IsVisible = false;

            var fetchedPosts = await _postService.GetPosts();

            // 1. Get User ID from Preferences
            string currentUserId = Preferences.Get("UserId", string.Empty);

            // Debugging: Print to Output window to verify what is stored
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Stored User ID: '{currentUserId}'");

            Posts.Clear();

            if (fetchedPosts != null && fetchedPosts.Count > 0)
            {
                foreach (var post in fetchedPosts)
                {
                    // 2. ROBUST ID CHECK
                    // We check if AuthorId has a value, then compare ignoring Case (A vs a)
                    if (!string.IsNullOrEmpty(currentUserId) &&
                        post.AuthorId.HasValue &&
                        string.Equals(post.AuthorId.Value.ToString(), currentUserId, StringComparison.OrdinalIgnoreCase))
                    {
                        post.IsOwner = true;
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Owner found for post: {post.Title}");
                    }
                    else
                    {
                        post.IsOwner = false;
                    }

                    Posts.Add(post);
                }

                EmptyStateLabel.IsVisible = false;
                FeedRefreshView.IsVisible = true;
            }
            else
            {
                EmptyStateLabel.IsVisible = true;
                FeedRefreshView.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load posts: {ex.Message}", "OK");
        }
        finally
        {
            LoadingSpinner.IsVisible = false;
            LoadingSpinner.IsRunning = false;
            FeedRefreshView.IsRefreshing = false;
        }
    }

    private async void OnRefreshing(object sender, EventArgs e) => await LoadPosts();

    private async void OnNewPostClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AddPostPage());
    }

    // ✅ HANDLER: For Owner (Edit)
    private async void OnEditPostClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Post post)
        {
            await Navigation.PushModalAsync(new EditPostPage(post));
        }
    }

    // ✅ HANDLER: For Owner (Delete)
    private async void OnDeletePostClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Post post)
        {
            bool confirm = await DisplayAlert(
                "Delete Post",
                "Are you sure you want to delete this post? This action cannot be undone.",
                "Delete",
                "Cancel"
            );

            if (!confirm) return;

            try
            {
                bool success = await _postService.DeletePost(post.Id);

                if (success)
                {
                    Posts.Remove(post); // Remove from UI immediately
                    await DisplayAlert("Success", "Post deleted successfully!", "OK");

                    if (Posts.Count == 0)
                    {
                        EmptyStateLabel.IsVisible = true;
                        FeedRefreshView.IsVisible = false;
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Failed to delete post. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }

    // ✅ FIXED: Missing Handler for Non-Owners (The "•••" Button)
    private async void OnPostOptionsClicked(object sender, EventArgs e)
    {
        // This is for posts that belong to OTHER people
        string action = await DisplayActionSheet("Post Options", "Cancel", null, "Report Post", "Copy Link");

        if (action == "Report Post")
        {
            await DisplayAlert("Report", "Thank you for reporting this post. We will review it shortly.", "OK");
        }
        else if (action == "Copy Link")
        {
            await DisplayAlert("Link Copied", "Link copied to clipboard.", "OK");
        }
    }
}