using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace IndoorMappingApp
{
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        async void OnLoginClicked(object sender, EventArgs e)
            => await Navigation.PushAsync(new LoginPage());

        async void OnRegisterClicked(object sender, EventArgs e)
            => await Navigation.PushAsync(new RegisterPage());

        async void OnGuestClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(
                "mainmenu",
                animate: true,
                new Dictionary<string, object>
                {
            { "IsGuest", true }
                });
        }
        

            async void OnShowRoomClicked(object sender, EventArgs e)
            => await Navigation.PushAsync(new ShowRoomPage());


        // Called by the "Português" button
        void OnToggleLanguageClicked(object sender, EventArgs e)
        {
            // Switch between English (en-US) and Portuguese (pt-PT)
            var current = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var newCulture = current == "pt"
                ? new CultureInfo("en-US")
                : new CultureInfo("pt-PT");

            LocalizationResourceManager.Instance.SetCulture(newCulture);
        }
    }
}
