using Microsoft.Maui.Controls;

namespace IndoorMappingApp;

public partial class ShowRoomPage : ContentPage
{
	public ShowRoomPage()
	{
		InitializeComponent();

        var htmlSource = new HtmlWebViewSource
        {
            Html = @"
                <html>
                  <head>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <style>
                        html, body {
                            margin: 0;
                            padding: 0;
                            height: 100%;
                            overflow: hidden;
                            background-color: black;
                        }
                        iframe {
                            width: 100%;
                            height: 100%;
                            border: none;
                        }
                    </style>
                  </head>
                  <body>
                   <iframe width=""853"" height=""480"" src=""https://my.matterport.com/show/?m=GzqaahfUmtE"" frameborder=""0"" allowfullscreen allow=""autoplay; fullscreen; web-share; xr-spatial-tracking;""></iframe>
                  </body>
                </html>"
        };

        MatterportWebView.Source = htmlSource;
    }
}