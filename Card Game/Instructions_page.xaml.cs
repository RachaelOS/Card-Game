using System.Diagnostics;

namespace Card_Game;

public partial class Instructions_page : ContentPage
{
	public Instructions_page()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Set the initial value when the page appears
        next_counter = 0;
        instructions_label.Text = instructions[next_counter];
        backButton.IsVisible = false;
        nextButton.IsVisible = true;
    }

    int next_counter = 0;
    string[] instructions = ["To play blackjack you must aim for the combined total of the cards in your hand to equal 21...", 
        "The value of each card is determined by the number on the card\n Jack, Queen and King are equal to 10, an Ace can be 1 or 11...", 
        "If the score in your hand is greater than 21, you're bust!", 
        "Each player will get a turn to either stick or twist\n Twist means add a new card to the hand, stick means you don't want any more cards and will end your turn.", 
        "Each player will take it in turns to twist as many times as they like then stick...", 
        "The player with the highest score (not above 21) at the end of the round is the winner.\n That is unless the dealer has the highest score or if everyone is bust then the dealer wins...", 
        "You can't see the other players' scores during the round except for the dealer who goes last. You can only see if they're bust...", 
        "So be careful when you twist! Have fun and enter the casino!"];
   

    private void next_clicked(object sender, EventArgs e)
    {
        Debug.WriteLine(instructions.Length);

        next_counter += 1;

        if (next_counter > 0)
        {
            Back_visible();
        }
        
        if (next_counter == instructions.Length-1)
        {
            nextButton.IsVisible = false;
        }
        Debug.WriteLine(next_counter);
        instructions_label.Text = instructions[next_counter];



    }

    private void Back_visible()
    {
        backButton.IsVisible = true;
    }

    private void back_button(object sender, EventArgs e)
    {
        next_counter -= 1;

        if (next_counter == 0)
        {
            backButton.IsVisible = false;
        }

        if (next_counter < instructions.Length-1)
        {
            nextButton.IsVisible = true;
        }

        instructions_label.Text = instructions[next_counter];
    }

    private async void home_button(object sender, EventArgs e)
    {
        next_counter = 0;
        await Shell.Current.GoToAsync("MainPage");
    }
}