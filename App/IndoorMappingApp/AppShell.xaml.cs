namespace IndoorMappingApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("register", typeof(RegisterPage));
			Routing.RegisterRoute("mainmenu", typeof(MainMenuPage));
            Routing.RegisterRoute("routes", typeof(AvailableRoutesPage));
            Routing.RegisterRoute("options", typeof(OptionsPage));
        }
    }
}
