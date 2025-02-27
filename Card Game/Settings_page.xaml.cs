namespace Card_Game;

public partial class Settings_page : ContentPage
{
	public Settings_page()
	{
		InitializeComponent();
	}

    private async void home_button(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("MainPage");
    }
}