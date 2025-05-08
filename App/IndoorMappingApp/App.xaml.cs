using Plugin.LocalNotification;
using Microsoft.Maui.Storage;
using Plugin.LocalNotification.AndroidOption; // For Preferences

namespace IndoorMappingApp
{
    public partial class App : Application
    {
        public static HttpClient ApiClient { get; private set; }

        public App()
        {
            InitializeComponent();
            ApiClient = new HttpClient
            {
                BaseAddress = new Uri("https://isepindoornavigationapi-vgq7.onrender.com/swagger/index.html")
            };

            MainPage = new AppShell();

            Preferences.Clear(); // for demonstrating purposes

            ScheduleDailyReminder();
            
        }

        public void ScheduleDailyReminder()
        {
            // Only schedule if it hasn't been set before
            if (!Preferences.Get("DailyNotificationSet", false))
            {
                var notification = new NotificationRequest
                {
                    NotificationId = 1000,
                    Title = "Don't forget!",
                    Description = "Check the app today for new updates.",
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(30),
                        RepeatType = NotificationRepeat.Daily
                    }
                };

                // DateTime.Now.Date.AddHours(18)

                LocalNotificationCenter.Current.Show(notification);

                // Mark as scheduled
                Preferences.Set("DailyNotificationSet", true);
            }
        }
    }
}
