using SocialSyncApp.Models;
using SocialSyncApp.Services;

namespace SocialSyncApp.Views;

public partial class EditProfilePage : ContentPage
{
    private readonly UserService _userService;
    private readonly User _currentUser;

    // Constructor accepts the current user object to pre-fill fields
    public EditProfilePage(User user)
    {
        InitializeComponent();
        _userService = new UserService();
        _currentUser = user;

        LoadUserData();
    }

    private void LoadUserData()
    {
        NameEntry.Text = _currentUser.FullName;
        PhoneEntry.Text = _currentUser.Phone;
        BioEditor.Text = _currentUser.Bio;
        Persona1Entry.Text = _currentUser.Persona1;
        Persona2Entry.Text = _currentUser.Persona2;
        Persona3Entry.Text = _currentUser.Persona3;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        LoadingOverlay.IsVisible = true;

        try
        {
            // Update the local object with new values
            _currentUser.FullName = NameEntry.Text;
            _currentUser.Phone = PhoneEntry.Text;
            _currentUser.Bio = BioEditor.Text;
            _currentUser.Persona1 = Persona1Entry.Text;
            _currentUser.Persona2 = Persona2Entry.Text;
            _currentUser.Persona3 = Persona3Entry.Text;

            // Send update to Supabase
            bool success = await _userService.UpdateUser(_currentUser.Id, _currentUser);

            if (success)
            {
                await DisplayAlert("Success", "Profile updated successfully!", "OK");
                await Navigation.PopModalAsync(); // Close modal
            }
            else
            {
                await DisplayAlert("Error", "Failed to update profile.", "OK");
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