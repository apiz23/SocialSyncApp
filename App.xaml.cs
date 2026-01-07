namespace SocialSyncApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Shell controls navigation
        MainPage = new AppShell();
    }
}
