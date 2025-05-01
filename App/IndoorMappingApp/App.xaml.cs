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
        }
    }
}
