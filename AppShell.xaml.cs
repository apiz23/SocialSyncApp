namespace SocialSyncApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        bool isLoggedIn = Preferences.Get("IsLoggedIn", false);

        if (isLoggedIn)
            await GoToAsync("//MainPage");
        else
            await GoToAsync("//LoginPage");
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        Preferences.Clear();

        // Hide sidebar after logout
        FlyoutIsPresented = false;

        await GoToAsync("//LoginPage");
    }
}
