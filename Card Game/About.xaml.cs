namespace Card_Game;

public partial class About : ContentPage
{
	public About()
	{
		InitializeComponent();
	}
    private async void home_button(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}