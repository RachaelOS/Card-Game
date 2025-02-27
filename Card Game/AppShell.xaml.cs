namespace Card_Game
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("Settings", typeof(Settings_page));

            Routing.RegisterRoute("Blackjack", typeof(Blackjack));

            Routing.RegisterRoute("MainPage", typeof(MainPage));
        }
    }
}
