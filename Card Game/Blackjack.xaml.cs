using System.Security.Cryptography.X509Certificates;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;

using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Runtime.CompilerServices;
//using Photos;

namespace Card_Game;

public partial class Blackjack : ContentPage
{
	public Blackjack()
	{
		InitializeComponent();
        //receives message from MainPage to start game - calls StartGame method
        WeakReferenceMessenger.Default.Register<StartGameMessage>(this, (r, m) => StartGame());

    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Unregister the message handler when the page is disposed
        WeakReferenceMessenger.Default.Unregister<StartGameMessage>(this);
    }

    private async void home_button(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    // Define classes for deck of cards, player hands, and dealer hands
    //define card class
    public class Card
    {
        public string Suit { get; set; }
        public string Rank { get; set; }
        public int Value { get; set; }

        public Card(string suit, string rank, int value)
        {
            Suit = suit;
            Rank = rank;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Rank} of {Suit} with value of {Value}";
        }
    }

    //define deck of cards and actions that can be made with the deck
    public class Deck
    {
        private List<Card> cards;
        private static readonly string[] Suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        private static readonly string[] Ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

        //constructor to create deck of cards
        public Deck()
        {
            cards = new List<Card>();
            foreach (var suit in Suits)
            {
                foreach (var rank in Ranks)
                {
                    //cards.Add(new Card(suit, rank));
                    switch (rank)
                    {
                        case "2":
                            cards.Add(new Card(suit, rank, 2));
                            break;
                        case "3":
                            cards.Add(new Card(suit, rank, 3));
                            break;
                        case "4":
                            cards.Add(new Card(suit, rank, 4));
                            break;
                        case "5":
                            cards.Add(new Card(suit, rank, 5));
                            break;
                        case "6":
                            cards.Add(new Card(suit, rank, 6));
                            break;
                        case "7":
                            cards.Add(new Card(suit, rank, 7));
                            break;
                        case "8":
                            cards.Add(new Card(suit, rank, 8));
                            break;
                        case "9":
                            cards.Add(new Card(suit, rank, 9));
                            break;
                        case "10":
                            cards.Add(new Card(suit, rank, 10));
                            break;
                        case "Jack":
                            cards.Add(new Card(suit, rank, 10));
                            break;
                        case "Queen":
                            cards.Add(new Card(suit, rank, 10));
                            break;
                        case "King":
                            cards.Add(new Card(suit, rank, 10));
                            break;
                        case "Ace":
                            cards.Add(new Card(suit, rank, 11));
                            break;
                    }
                }
            }
        }
        
        public void Shuffle()
        {
            Random rng = new Random();
            int n = cards.Count;
            while (n >1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }

        public Card Deal()
        {
            if (cards.Count == 0)
            {
                throw new InvalidOperationException("No cards left in the deck.");
            }

            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        public int CardsRemaining()
        {
            return cards.Count;
        }

    }


    public class Hand
    {
        private List<Card> Cards;
        private int Score = 0;
        public string Name { get; set; }


        public Hand(string name)
        {
            Name = name;
            Cards = new List<Card>();
        }
 
        public void AddCard(Card card)
        {
            Cards.Add(card);
            //if added card is an ace,change value of Ace depending on current score
            if (card.Rank == "Ace" && Score > 10)
            {
                card.Value = 1;
            }
        }

        public int GetScore()
        {

            foreach(Card card in Cards)
            {
                Score += card.Value;
            }
            return Score;
        }

        public string GetHand()
        {
            List<string> handDetails = new List<string>();
            foreach (Card card in Cards)
            {
                handDetails.Add($"{card.Rank} of {card.Suit}");
            }
            return string.Join(",", handDetails);
        }

        public string GetName()
        {
            return Name;
        }
    }

    private void UpdateLabel(string newText)
    {
            update_label.Text = newText;
    }


    private void Twist(bool twist_status)
    {
        twist_status = true;
    }

    public static void StartGame()
    {
        // Debug.WriteLine("gameplay method called");
        // initial settings for game - no. players, player hands (empty), array of players hands
        // int no_players = 3;
        Hand npc1 = new Hand("npc1");
        Hand npc2 = new Hand("npc2");
        Hand player = new Hand("player");
        Hand[] players = { npc1, player, npc2 };
        bool twist_status = false;
        bool stick_status = false;

        // reset deck
        Deck card = new Deck();

        // shuffle deck
        card.Shuffle();

        // deal cards - 2 each 
        foreach (Hand hand in players)
        {
            Card new_card1 = card.Deal();
            Card new_card2 = card.Deal();
            hand.AddCard(new_card1);
            hand.AddCard(new_card2);
            // Debug.WriteLine(hand.GetName());
            // hand.GetHand();
            // Debug.WriteLine(hand.GetScore());
        }

        // print player's cards
        var currentWindow = Application.Current?.Windows.FirstOrDefault();
        if (currentWindow?.Page is not null)
        {
            var blackjackPage = (Blackjack)currentWindow.Page.Navigation.NavigationStack.Last();
            blackjackPage.UpdateLabel(player.GetHand());
        }

        // player 1 takes turn - stick or bust- print action

        while (Gameplay.Status(player) == "Playing")
        {
            // player takes turn - stick or bust
            if (twist_status == true)
            {
                Card new_card = card.Deal();
                player.AddCard(new_card);
                var blackjackPage = (Blackjack)currentWindow.Page.Navigation.NavigationStack[currentWindow.Page.Navigation.NavigationStack.Count - 1];
                blackjackPage.UpdateLabel(player.GetHand());
                int score = player.GetScore();

                if (score > 21)
                {
                    blackjackPage.UpdateLabel("Bust");
                }
                else if (score == 21)
                {
                    blackjackPage.UpdateLabel("Blackjack!");
                }
            }
        }
            // player 1 takes turn - stick or bust- print actions
            // player 2 takes turn - stick or bust - print actions
            // dealer wins or dealers turn - print actions
            // end of game
        }


        // player takes turn - stick or bust
        // player 2 takes turn - stick or bust - print actions
        // dealer wins or dealers turn - print actions
        // end of game
    }
    public class Gameplay()
    {

        public static string Status(Hand hand)
        {
            if (hand.GetScore() > 21)
            {
                return "Bust";
            }
            else if (hand.GetScore() == 21)
            {
                return "Blackjack";
            }
            else
            {
                return "Playing";
            }
        }

        public static string Winner(Hand player, Hand dealer)
        {
            if (player.GetScore() > 21)
            {
                return "Dealer";
            }
            else if (dealer.GetScore() > 21)
            {
                return "Player";
            }
            else if (player.GetScore() > dealer.GetScore())
            {
                return "Player";
            }
            else if (dealer.GetScore() > player.GetScore())
            {
                return "Dealer";
            }
            else
            {
                return "Push";
            }

        }
    }


}