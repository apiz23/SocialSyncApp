using SocialSyncApp.Services;

namespace SocialSyncApp.Views;

public partial class LoginPage : ContentPage
{
    // Assuming you have this service created
    private readonly AuthService _authService = new();

    public LoginPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Check if user is already logged in
        bool isLoggedIn = Preferences.Get("IsLoggedIn", false);
        if (isLoggedIn)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // 1. Basic Validation
        if (string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlert("Error", "Please enter your email and password.", "OK");
            return;
        }

        // 2. Set Loading State
        LoginButton.Text = ""; // Hide text to show spinner nicely
        LoginButton.IsEnabled = false;
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;

        try
        {
            // 3. Attempt Login
            var user = await _authService.Login(EmailEntry.Text.Trim(), PasswordEntry.Text);

            if (user == null)
            {
                await DisplayAlert("Login Failed", "Invalid email or password.", "OK");
                return;
            }

            // 4. Save Session Data
            Preferences.Set("IsLoggedIn", true);
            Preferences.Set("UserId", user.Id.ToString());
            Preferences.Set("UserEmail", user.Email);
            Preferences.Set("UserFullName", user.FullName ?? "User");

            // 5. Navigate to Main Page
            await Shell.Current.GoToAsync("//MainPage");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Something went wrong: {ex.Message}", "OK");
        }
        finally
        {
            // 6. Reset UI State
            LoginButton.IsEnabled = true;
            LoginButton.Text = "Sign In";
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private void OnTogglePasswordTapped(object sender, EventArgs e)
    {
        // Toggle the entry visibility
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;

        // Toggle the Icons (Swap Visibility)
        // If IsPassword is TRUE (Hidden), Show Closed Eye.
        // If IsPassword is FALSE (Visible), Show Open Eye.
        EyeClosedIcon.IsVisible = PasswordEntry.IsPassword;
        EyeOpenIcon.IsVisible = !PasswordEntry.IsPassword;
    }

    private async void OnSignUpTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Coming Soon", "Registration is under construction.", "OK");
    }
}