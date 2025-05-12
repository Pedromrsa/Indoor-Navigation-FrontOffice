using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;

namespace IndoorMappingApp;

public partial class ShowRoomPage : ContentPage
{
    public ShowRoomPage(string roomName)
    {
        InitializeComponent();

        if (roomName.Equals("Show Room 311"))
        {
            // Room B311
            MatterportWebView.Source = new UrlWebViewSource
            {
                Url = "https://my.matterport.com/show/?m=GzqaahfUmtE"
            };
        }
        else
        {
            //// Room B404
            //MatterportWebView.Source = new UrlWebViewSource
            //{
            //    Url = "https://my.matterport.com/show/?m=GzqaahfUmtE"
            //};
        }
        
        MatterportWebView.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>()
                         .SetIsJavaScriptAlertEnabled(true);
    }
}
