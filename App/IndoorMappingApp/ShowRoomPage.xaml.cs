using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;

namespace IndoorMappingApp;

public partial class ShowRoomPage : ContentPage
{
    public ShowRoomPage()
    {
        InitializeComponent();

        // Point directly at your Matterport tour URL
        MatterportWebView.Source = new UrlWebViewSource
        {
            Url = "https://my.matterport.com/show/?m=GzqaahfUmtE"
        };

        
        MatterportWebView.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>()
                         .SetIsJavaScriptAlertEnabled(true);
    }
}
