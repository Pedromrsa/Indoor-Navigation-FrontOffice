using Plugin.LocalNotification.AndroidOption;
using Plugin.LocalNotification;

namespace IndoorMappingApp
{
    public partial class OptionsPage : ContentPage
    {
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


