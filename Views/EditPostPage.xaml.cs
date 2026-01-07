using SocialSyncApp.Models;
using SocialSyncApp.Services;

namespace SocialSyncApp.Views;

public partial class EditPostPage : ContentPage
{
    private readonly PostService _postService;
    private readonly Post _postToEdit;

    public EditPostPage(Post post)
    {
        InitializeComponent();
        _postService = new PostService();
        _postToEdit = post;

        // Pre-fill data
        TitleEntry.Text = post.Title;
        ContentEditor.Text = post.Content;
        CategoryPicker.SelectedItem = post.Category;

        // Update character count on load
        CharacterCountLabel.Text = $"{post.Content?.Length ?? 0}/500";
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(ContentEditor.Text))
        {
            await DisplayAlert("Validation Error", "Content cannot be empty.", "OK");
            return;
        }

        // ✅ FIX 1: Use LoadingOverlay (Grid) instead of LoadingSpinner
        LoadingOverlay.IsVisible = true;

        try
        {
            _postToEdit.Title = TitleEntry.Text?.Trim() ?? "";
            _postToEdit.Content = ContentEditor.Text.Trim();
            _postToEdit.Category = CategoryPicker.SelectedItem?.ToString() ?? "General";

            bool success = await _postService.UpdatePost(_postToEdit.Id, _postToEdit);

            if (success)
            {
                await DisplayAlert("Success", "Post updated successfully! ✅", "OK");
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error", "Failed to update post. Please try again.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
        finally
        {
            // ✅ FIX 1: Hide overlay
            LoadingOverlay.IsVisible = false;
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert(
            "Discard Changes?",
            "Are you sure you want to discard your changes?",
            "Discard",
            "Keep Editing"
        );

        if (confirm)
        {
            await Navigation.PopModalAsync();
        }
    }

    // ✅ FIX 2: Added missing handler referenced in XAML
    private void OnContentTextChanged(object sender, TextChangedEventArgs e)
    {
        int length = e.NewTextValue?.Length ?? 0;
        CharacterCountLabel.Text = $"{length}/500";

        // Optional: Change color if limit exceeded
        CharacterCountLabel.TextColor = length > 500 ? Colors.Red : Color.FromArgb("#888");
    }

    // ✅ FIX 3: Added missing handler referenced in XAML
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Delete Post", "Are you sure you want to delete this post?", "Delete", "Cancel");
        if (!confirm) return;

        LoadingOverlay.IsVisible = true;
        try
        {
            bool success = await _postService.DeletePost(_postToEdit.Id);
            if (success)
            {
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error", "Failed to delete post.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            LoadingOverlay.IsVisible = false;
        }
    }
}