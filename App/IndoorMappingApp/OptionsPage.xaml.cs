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
            throw new NotImplementedException("Navigation to Account Settings is not implemented yet.");
        }

        private async void OnDiaryClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException("Navigation to Account Settings is not implemented yet.");
        }

        private async void OnPathProblemsClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException("Navigation to Account Settings is not implemented yet."); ;
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            // You could show a confirmation dialog or navigate to a login page
            await DisplayAlert("Logout", "You have been logged out.", "OK");
        }
    }
}

