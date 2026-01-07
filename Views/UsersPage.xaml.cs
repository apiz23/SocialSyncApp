namespace SocialSyncApp.Views;

public partial class UsersPage : ContentPage
{
    public UsersPage()
    {
        InitializeComponent();
    }

    private async void OnAddUserClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Add User", "Add User form would open here", "OK");
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Edit User", "Edit User form would open here", "OK");
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Delete User", "Are you sure you want to delete this user?", "Yes", "No");
        if (answer)
        {
            await DisplayAlert("Success", "User deleted successfully!", "OK");
        }
    }
}
