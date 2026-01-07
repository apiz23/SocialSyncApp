using SocialSyncApp.Models;
using SocialSyncApp.Services;

namespace SocialSyncApp.Views;

public partial class EditEventPage : ContentPage
{
    private readonly EventService _eventService;
    private readonly Event _eventToEdit;

    public EditEventPage(Event eventToEdit)
    {
        InitializeComponent();
        _eventService = new EventService();
        _eventToEdit = eventToEdit;

        LoadEventData();
    }

    private void LoadEventData()
    {
        TitleEntry.Text = _eventToEdit.Title;
        DescriptionEditor.Text = _eventToEdit.Description;
        LocationEntry.Text = _eventToEdit.Location;
        ImageUrlEntry.Text = _eventToEdit.ImageUrl;

        if (_eventToEdit.MaxParticipants.HasValue)
        {
            MaxParticipantsEntry.Text = _eventToEdit.MaxParticipants.Value.ToString();
        }

        // Separate DateTime into Date and Time components for the pickers
        EventDatePicker.Date = _eventToEdit.EventDate.Date;
        EventTimePicker.Time = _eventToEdit.EventDate.TimeOfDay;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // 1. Validation
        if (string.IsNullOrWhiteSpace(TitleEntry.Text) || string.IsNullOrWhiteSpace(LocationEntry.Text))
        {
            await DisplayAlert("Error", "Title and Location are required.", "OK");
            return;
        }

        LoadingOverlay.IsVisible = true;

        try
        {
            // 2. Combine Date + Time
            DateTime combinedDateTime = EventDatePicker.Date + EventTimePicker.Time;
            // Ensure UTC to avoid timezone issues when saving to DB
            combinedDateTime = DateTime.SpecifyKind(combinedDateTime, DateTimeKind.Utc);

            // 3. Update Object
            _eventToEdit.Title = TitleEntry.Text.Trim();
            _eventToEdit.Description = DescriptionEditor.Text?.Trim() ?? "";
            _eventToEdit.Location = LocationEntry.Text.Trim();
            _eventToEdit.EventDate = combinedDateTime;
            _eventToEdit.ImageUrl = ImageUrlEntry.Text?.Trim();

            // Parse Max Participants
            if (int.TryParse(MaxParticipantsEntry.Text, out int max))
                _eventToEdit.MaxParticipants = max;
            else
                _eventToEdit.MaxParticipants = null;

            // 4. Call Service
            bool success = await _eventService.UpdateEvent(_eventToEdit.Id, _eventToEdit);

            if (success)
            {
                await DisplayAlert("Success", "Event updated successfully!", "OK");
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error", "Failed to update event.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
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