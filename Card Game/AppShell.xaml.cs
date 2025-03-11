namespace Card_Game
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("Instructions", typeof(Instructions_page));

            Routing.RegisterRoute("Blackjack", typeof(Blackjack));

            Routing.RegisterRoute("MainPage", typeof(MainPage));
        }
    }
}
