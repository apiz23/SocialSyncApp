
namespace SocialSyncApp.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    private async void OnFriendsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FriendsPage());
    }

    private async void OnEventsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EventsPage());
    }

    private async void OnPostsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PostsPage());
    }

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage());
    }
}