using SocialSyncApp.Models;
using SocialSyncApp.Services;
using System.Collections.ObjectModel;

namespace SocialSyncApp.Views;

public partial class EventsPage : ContentPage
{
    private readonly EventService _eventService;
    public ObservableCollection<Event> Events { get; set; } = new ObservableCollection<Event>();

    public EventsPage()
    {
        InitializeComponent();
        _eventService = new EventService();
        EventsCollectionView.ItemsSource = Events;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadEvents();
    }

    private async Task LoadEvents()
    {
        try
        {
            var fetchedEvents = await _eventService.GetEvents();

            // ✅ 1. Get Logged In User ID
            string currentUserId = Preferences.Get("UserId", "");

            Events.Clear();

            if (fetchedEvents != null)
            {
                foreach (var evt in fetchedEvents)
                {
                    // ✅ 2. Check Ownership
                    // We check if the current user ID matches the event's CreatorId
                    if (!string.IsNullOrEmpty(currentUserId) &&
                        evt.CreatorId.ToString().Equals(currentUserId, StringComparison.OrdinalIgnoreCase))
                    {
                        evt.IsOwner = true;
                    }
                    else
                    {
                        evt.IsOwner = false;
                    }

                    Events.Add(evt);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load events: {ex.Message}", "OK");
        }
        finally
        {
            LoadingSpinner.IsVisible = false;
            EventsRefreshView.IsVisible = true;
            EventsRefreshView.IsRefreshing = false;
        }
    }

    private async void OnRefreshing(object sender, EventArgs e) => await LoadEvents();

    // --- Button Handlers ---

    private async void OnCreateEventClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Coming Soon", "Create Event Page implementation needed.", "OK");
        // await Shell.Current.GoToAsync(nameof(AddEventPage));
    }

    private async void OnEditEventClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Event eventToEdit)
        {
            // Security check: Only owner can edit
            if (!eventToEdit.IsOwner) return;

            // Navigate to the new Edit Page
            await Navigation.PushModalAsync(new EditEventPage(eventToEdit));
        }
    }

    // ✅ NEW: Delete Logic
    private async void OnDeleteEventClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Event eventToDelete)
        {
            // Security Check
            if (!eventToDelete.IsOwner) return;

            bool confirm = await DisplayAlert("Delete Event",
                $"Are you sure you want to delete '{eventToDelete.Title}'?", "Delete", "Cancel");

            if (!confirm) return;

            LoadingSpinner.IsVisible = true;

            try
            {
                bool success = await _eventService.DeleteEvent(eventToDelete.Id);

                if (success)
                {
                    Events.Remove(eventToDelete);
                    await DisplayAlert("Success", "Event deleted successfully.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Failed to delete event.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
            finally
            {
                LoadingSpinner.IsVisible = false;
            }
        }
    }

    private async void OnViewDetailsClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Event selectedEvent)
        {
            await DisplayAlert("Details",
                $"Title: {selectedEvent.Title}\n" +
                $"Description: {selectedEvent.Description}\n" +
                $"Host: {selectedEvent.CreatedBy}",
                "OK");
        }
    }
}