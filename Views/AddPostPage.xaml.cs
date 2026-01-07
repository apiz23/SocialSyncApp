using SocialSyncApp.Models;
using SocialSyncApp.Services;

namespace SocialSyncApp.Views;

public partial class AddPostPage : ContentPage
{
    private readonly PostService _postService;

    public AddPostPage()
    {
        InitializeComponent();
        _postService = new PostService();
    }

    private async void OnPostClicked(object sender, EventArgs e)
    {
        // 1. Validation
        if (string.IsNullOrWhiteSpace(TitleEntry.Text) || string.IsNullOrWhiteSpace(ContentEditor.Text))
        {
            await DisplayAlert("Error", "Please enter a title and content.", "OK");
            return;
        }

        if (CategoryPicker.SelectedIndex == -1)
        {
            await DisplayAlert("Error", "Please select a category.", "OK");
            return;
        }

        // 2. Show Loading
        LoadingOverlay.IsVisible = true;

        try
        {
            // 3. Get Current User Info (Stored during Login)
            string userIdStr = Preferences.Get("UserId", "");
            string userFullName = Preferences.Get("UserFullName", "Anonymous");

            if (string.IsNullOrEmpty(userIdStr))
            {
                await DisplayAlert("Error", "You must be logged in to post.", "OK");
                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }

            // 4. Create Post Object
            var newPost = new Post
            {
                Title = TitleEntry.Text,
                Content = ContentEditor.Text,
                Category = CategoryPicker.SelectedItem.ToString(),
                Author = userFullName,
                ImageUrl = string.IsNullOrWhiteSpace(ImageEntry.Text) ? null : ImageEntry.Text,
                // Supabase will handle CreatedAt automatically if configured, or we send it here
            };

            // 5. Send to Service
            bool success = await _postService.CreatePost(newPost, Guid.Parse(userIdStr));

            if (success)
            {
                await DisplayAlert("Success", "Post created successfully!", "OK");
                await Navigation.PopModalAsync(); // Close the modal
            }
            else
            {
                await DisplayAlert("Error", "Failed to create post. Please try again.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
        }
        finally
        {
            LoadingOverlay.IsVisible = false;
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}