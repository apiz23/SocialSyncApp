using SocialSyncApp.Models;
using SocialSyncApp.Services;
using System.Collections.ObjectModel;

namespace SocialSyncApp.Views;

public partial class AddFriendPage : ContentPage
{
    private readonly UserService _userService;
    private readonly FriendService _friendService;
    public ObservableCollection<User> PotentialFriends { get; set; } = new ObservableCollection<User>();

    public AddFriendPage()
    {
        InitializeComponent();
        _userService = new UserService();
        _friendService = new FriendService();
        UsersCollectionView.ItemsSource = PotentialFriends;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadPotentialFriends();
    }

    private async Task LoadPotentialFriends()
    {
        try
        {
            string currentUserId = Preferences.Get("UserId", "");
            if (string.IsNullOrEmpty(currentUserId)) return;

            LoadingSpinner.IsVisible = true;
            UsersCollectionView.IsVisible = false;

            // 1. Get ALL users
            var allUsers = await _userService.GetUsers();

            // 2. Get CURRENT friends (to filter them out)
            var currentFriends = await _friendService.GetFriends(currentUserId);
            var friendIds = currentFriends?.Select(f => f.FriendId.ToString()).ToList() ?? new List<string>();

            PotentialFriends.Clear();

            if (allUsers != null)
            {
                foreach (var user in allUsers)
                {
                    // Filter Logic:
                    // - Exclude myself
                    // - Exclude existing friends
                    if (user.Id.ToString() != currentUserId && !friendIds.Contains(user.Id.ToString()))
                    {
                        PotentialFriends.Add(user);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load users: {ex.Message}", "OK");
        }
        finally
        {
            LoadingSpinner.IsVisible = false;
            UsersCollectionView.IsVisible = true;
        }
    }

    private async void OnAddUserClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is User userToAdd)
        {
            string currentUserId = Preferences.Get("UserId", "");

            // Disable button to prevent double clicks
            button.IsEnabled = false;
            button.Text = "Adding...";

            try
            {
                bool success = await _friendService.AddFriend(currentUserId, userToAdd.Id.ToString());

                if (success)
                {
                    // Remove from list immediately for better UI feel
                    PotentialFriends.Remove(userToAdd);
                    await DisplayAlert("Success", $"{userToAdd.FullName} added to friends!", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Failed to add friend.", "OK");
                    button.IsEnabled = true;
                    button.Text = "+ Add";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                button.IsEnabled = true;
                button.Text = "+ Add";
            }
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}