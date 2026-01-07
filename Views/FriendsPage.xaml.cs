using SocialSyncApp.Models;
using SocialSyncApp.Services;
using System.Collections.ObjectModel;

namespace SocialSyncApp.Views;

public partial class FriendsPage : ContentPage
{
    private readonly FriendService _friendService;
    public ObservableCollection<Friend> Friends { get; set; } = new ObservableCollection<Friend>();

    public FriendsPage()
    {
        InitializeComponent();
        _friendService = new FriendService();
        FriendsCollectionView.ItemsSource = Friends;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadFriends();
    }

    private async Task LoadFriends()
    {
        try
        {
            string currentUserId = Preferences.Get("UserId", "");
            if (string.IsNullOrEmpty(currentUserId)) return;

            var fetchedFriends = await _friendService.GetFriends(currentUserId);

            Friends.Clear();
            if (fetchedFriends != null)
            {
                foreach (var friend in fetchedFriends)
                {
                    Friends.Add(friend);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Could not load friends: {ex.Message}", "OK");
        }
        finally
        {
            LoadingSpinner.IsVisible = false;
            FriendsRefreshView.IsVisible = true;
            FriendsRefreshView.IsRefreshing = false;
        }
    }

    private async void OnRefreshing(object sender, EventArgs e) => await LoadFriends();

    private async void OnAddFriendClicked(object sender, EventArgs e)
    {
        // Navigate to the Add Friend Page
        await Navigation.PushModalAsync(new AddFriendPage());
    }

    // ✅ VIEW PROFILE HANDLER
    private async void OnViewProfileClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Friend friend)
        {
            await DisplayAlert("Profile", $"Viewing profile of: {friend.DisplayName}\nEmail: {friend.DisplayJob}", "OK");
        }
    }

    // ✅ UNFRIEND HANDLER
    private async void OnUnfriendClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Friend friend)
        {
            bool confirm = await DisplayAlert("Unfriend", $"Are you sure you want to remove {friend.DisplayName}?", "Yes", "No");

            if (!confirm) return;

            LoadingSpinner.IsVisible = true;
            try
            {
                string currentUserId = Preferences.Get("UserId", "");

                bool success = await _friendService.RemoveFriend(currentUserId, friend.FriendId.ToString());

                if (success)
                {
                    Friends.Remove(friend);
                    await DisplayAlert("Removed", $"{friend.DisplayName} has been removed.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Failed to remove friend.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                LoadingSpinner.IsVisible = false;
            }
        }
    }
}