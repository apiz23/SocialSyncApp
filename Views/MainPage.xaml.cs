namespace SocialSyncApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCreatePostClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//PostsPage");
        }

        private async void OnAddEventClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//EventsPage");
        }

        private async void OnFindFriendsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//FriendsPage");
        }
    }
}