namespace Card_Game;

public partial class About : ContentPage
{
	public About()
	{
		InitializeComponent();
	}

    private async void PortfolioLink(object sender, EventArgs e)
    {
        Uri uri = new Uri("https://rachaelos.github.io/");
        await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
    }

    private async void GithubLink(object sender, EventArgs e)
    {
        Uri uri = new Uri("https://github.com/RachaelOS/Card-Game");
        await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
    }
    private async void home_button(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}