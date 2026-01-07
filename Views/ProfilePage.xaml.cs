using Microsoft.Maui;
using Microsoft.Maui.Storage;
using SocialSyncApp.Models;
using SocialSyncApp.Services;
using System.Linq;
using System.Xml;

namespace SocialSyncApp.Views;

public partial class ProfilePage : ContentPage
{
    private readonly UserService _userService = new();
    private User? _user;

    public ProfilePage()
    {
        InitializeComponent();
    }

    // Add this method
    private async void OnEditProfileClicked(object sender, EventArgs e)
    {
        if (_user != null)
        {
            // Pass the current user object to the edit page
            await Navigation.PushModalAsync(new EditProfilePage(_user));
        }
    }

    // Ensure OnAppearing reloads data so changes show up immediately
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadProfile(); // This will refresh the labels after the modal closes
    }

    private async Task LoadProfile()
    {
        var userIdStr = Preferences.Get("UserId", string.Empty);

        if (string.IsNullOrWhiteSpace(userIdStr)) return;
        if (!Guid.TryParse(userIdStr, out var userId)) return;

        _user = await _userService.GetUserById(userId);

        if (_user == null) return;

        // --- Update UI ---
        NameLabel.Text = _user.FullName ?? "Unknown";
        UsernameLabel.Text = $"@{_user.FullName?.Replace(" ", "").ToLower() ?? "user"}";
        EmailLabel.Text = _user.Email;
        PhoneLabel.Text = _user.Phone ?? "-";
        BioLabel.Text = _user.Bio ?? "No bio yet";

        // ✅ MISSING LINE ADDED HERE:
        DateJoinedLabel.Text = _user.DateJoined.ToString("MMMM dd, yyyy");

        // Check Personas (Optional: Update visibility based on data)
        Persona1Frame.IsVisible = !string.IsNullOrEmpty(_user.Persona1);
        Persona1Label.Text = _user.Persona1;

        Persona2Frame.IsVisible = !string.IsNullOrEmpty(_user.Persona2);
        Persona2Label.Text = _user.Persona2;

        Persona3Frame.IsVisible = !string.IsNullOrEmpty(_user.Persona3);
        Persona3Label.Text = _user.Persona3;
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert(
            "Logout",
            "Are you sure you want to logout?",
            "Yes",
            "No"
        );

        if (!answer) return;

        Preferences.Clear();
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
