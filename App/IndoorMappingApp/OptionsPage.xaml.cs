namespace IndoorMappingApp
{
    public partial class OptionsPage : ContentPage
    {
        public OptionsPage()
        {
            InitializeComponent();
        }


        private async void OnAccountSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AccountSettings());
        }

        private async void OnFeedbackClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FeedbackOptionsPage());
        }

        private async void OnDiaryClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException("Navigation to Diary is not implemented yet.");
        }

        private async void OnPathProblemsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PathProblems());
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            // Clear user session data
            Preferences.Remove("AuthToken");
            Preferences.Remove("UserId");

            // Navigate to login page
            await Shell.Current.GoToAsync("//LoginPage");

            // You could show a confirmation dialog or navigate to a login page
            await DisplayAlert("Logout", "You have been logged out.", "OK");
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException("Back Button"); ;
        }
    }
}

