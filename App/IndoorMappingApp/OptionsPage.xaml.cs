using Plugin.LocalNotification.AndroidOption;
using Plugin.LocalNotification;
using IndoorMappingApp.Scripts.DTOs;
using IndoorMappingApp.Scripts.Services;

namespace IndoorMappingApp
{
    public partial class OptionsPage : ContentPage
    {
        readonly IndoorApiService _api = new IndoorApiService();

        public OptionsPage()
        {
            InitializeComponent();

            // Set the toggle based on saved preference
            DailyReminderSwitch.IsToggled = Preferences.Get("DailyNotificationSet", false);
        }


        private async void OnAccountSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AccountSettings());
        }

        private async void OnFeedbackClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FeedbackOptionsPage());
        }

        private async void OnDeleteAccountClicked(object sender, EventArgs e)
        {
            // Retrieve user ID from ActiveUser
            if (!int.TryParse(ActiveUser.UserId, out int userId))
            {
                await DisplayAlert("Error", "User ID not found or invalid. Please log in again.", "OK");
                return;
            }

            var dto = new DeleteAccountDTO
            {
                id = int.Parse(ActiveUser.UserId)
            };

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirm Deletion",
                "Are you sure you want to delete your account? This action cannot be undone.",
                "Yes, delete",
                "Cancel"
            );

            if (confirm)
            {
                var result = await _api.DeleteUserAsync(userId, dto);

                await Application.Current.MainPage.DisplayAlert(
                    result.Success ? "Success" : "Error",
                    result.Message,
                    "OK"
                );

                if (result.Success)
                {
                    await Shell.Current.GoToAsync("//MenuPage");
                }
            }

        }

        /*private async void OnDiaryClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException("Navigation to Diary is not implemented yet.");
        }*/

        private async void OnPathProblemsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PathProblems());
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            var lrm = LocalizationResourceManager.Instance;

            // Clear user session data
            Preferences.Remove("AuthToken");
            Preferences.Remove("UserId");

            // Navigate to login page
            await Shell.Current.GoToAsync("//MenuPage");
            //await Navigation.PushAsync(new LoginPage());

            // You could show a confirmation dialog or navigate to a login page
            //await DisplayAlert("Logout", "You have been logged out.", "OK");
            await DisplayAlert(
                   lrm["Options_Logout"],
                   lrm["Logout_message"],
                   lrm["Button_OK"]);
        }

        private void OnReminderToggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                ((App)Application.Current).ScheduleDailyReminder();
            }
            else
            {
                CancelDailyReminder();
            }
        }



        private void CancelDailyReminder()
        {
            LocalNotificationCenter.Current.Cancel(1000); // Cancel by ID
            Preferences.Set("DailyNotificationSet", false);
        }
    }

}


