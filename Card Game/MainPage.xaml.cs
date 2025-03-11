using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Diagnostics;

namespace Card_Game
{
    public class StartGameMessage : ValueChangedMessage<string>
    {
        public StartGameMessage() : base("StartGame")
        {
        }
    }
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void instructions_clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Instructions_page");
        }

        private async void play_click(object sender, EventArgs e)
        {

            await Shell.Current.GoToAsync("Blackjack");
            WeakReferenceMessenger.Default.Send(new StartGameMessage());
        }
        private async void about_click(object sender, EventArgs e)
        {

            await Shell.Current.GoToAsync("//About");
        }

        private void quit_click(object sender, EventArgs e)
        {
            Application.Current?.Quit();
        }


    }
}
