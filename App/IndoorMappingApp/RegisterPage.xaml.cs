using System;
using System.Collections.Generic;
using System.Globalization;
using IndoorMappingApp.Scripts.Services;
using Microsoft.Maui.Controls;
using IndoorMappingApp.Scripts.DTOs;

namespace IndoorMappingApp
{
    public partial class RegisterPage : ContentPage
    {
        readonly IndoorApiService _api;

        public RegisterPage()
        {
            InitializeComponent();
            _api = new IndoorApiService();

            // Populate dynamic UI and hook change events
            PopulateLimitationOptions();
            SubscribeToCultureChanges();

            NameEntry.TextChanged += (s, e) => ValidateForm();
            EmailEntry.TextChanged += (s, e) => ValidateForm();
            PasswordEntry.TextChanged += (s, e) => ValidateForm();
            LimitationPicker.SelectedIndexChanged += (s, e) => ValidateForm();
        }

        void SubscribeToCultureChanges()
        {
            LocalizationResourceManager.Instance.PropertyChanged += (_, __) =>
            {
                // re-populate the picker items on culture change
                PopulateLimitationOptions();
            };
        }

        void PopulateLimitationOptions()
        {
            // pull the translated strings from your RESX via the manager
            var lrm = LocalizationResourceManager.Instance;
            var options = new[]
            {
                lrm["RegisterPage_Limitation_Normal"],
                lrm["RegisterPage_Limitation_Paraplegic"],
                lrm["RegisterPage_Limitation_Tetraplegic"],
            };

            LimitationPicker.ItemsSource = options;
            LimitationPicker.Title = lrm["RegisterPage_Picker_LimitationTitle"];
        }

        void ValidateForm()
        {
            bool isValid =
                !string.IsNullOrWhiteSpace(NameEntry.Text) &&
                !string.IsNullOrWhiteSpace(EmailEntry.Text) &&
                !string.IsNullOrWhiteSpace(PasswordEntry.Text) &&
                LimitationPicker.SelectedIndex >= 0;

            RegisterButton.IsEnabled = isValid;
        }

        async void OnRegisterClicked(object sender, EventArgs e)
        {
            // validation in code uses translated dialog texts
            var lrm = LocalizationResourceManager.Instance;
            if (string.IsNullOrWhiteSpace(NameEntry.Text))
            {
                await DisplayAlert(
                    lrm["ValidationError_Title"],
                    lrm["ValidationError_EnterName"],
                    lrm["Button_OK"]);
                return;
            }

            var dto = new RegisterRequestDTO
            {
                nome = NameEntry.Text,
                email = EmailEntry.Text,
                password = PasswordEntry.Text,
                tipoId = 4,
                mobilidadeId = LimitationPicker.SelectedIndex + 1
            };

            var result = await _api.RegisterAsync(dto);
            if (result?.IsSuccessStatusCode == true)
            {
                await DisplayAlert(
                    lrm["Registration_Success_Title"],
                    lrm["Registration_Success_Message"],
                    lrm["Button_OK"]);
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert(
                    lrm["Registration_Failure_Title"],
                    lrm["Registration_Failure_Message"],
                    lrm["Button_OK"]);
            }
        }

        async void OnLoginClicked(object sender, EventArgs e)
            => await Navigation.PushAsync(new LoginPage());
    }
}
