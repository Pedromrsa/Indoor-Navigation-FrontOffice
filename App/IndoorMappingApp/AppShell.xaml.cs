namespace IndoorMappingApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("register", typeof(RegisterPage));
        }
    }
}
