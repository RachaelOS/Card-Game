using System.Security.Cryptography.X509Certificates;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;

using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;


namespace Card_Game
{
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

        private void Twist_click(object sender, EventArgs e)
        {
            Twist();
        }

        private void Stick_click(object sender, EventArgs e)
        {
            Stick();
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
                while (n > 1)
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
            }

            public int GetScore()
            {
                Score = 0;

                foreach (Card card in Cards)
                {
                    Score += card.Value;
                    Debug.WriteLine("score:");
                    Debug.WriteLine(Score);
                    Debug.WriteLine("value");
                    Debug.WriteLine(card.Value);
                    Debug.WriteLine("-----");
                }

                //checks if score is > 21, if it is goes back over cards to change aces to 1
                if (Score > 21)
                {
                    Score = 0;
                    foreach (Card card in Cards)
                    {
                        //if added card is an ace,change value of Ace depending on current score
                        if (card.Rank == "Ace" && Score > 10)
                        {
                            card.Value = 1;
                        }
                    }
                    foreach (Card card in Cards)
                    {
                        Score += card.Value;
                    }
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

        private void DealerLabel(string newText)
        {
            dealer_label.Text = newText;
        }


        public Hand npc1 = new Hand("npc1");
        public Hand npc2 = new Hand("npc2");
        public Hand player = new Hand("player");
        public Hand dealer = new Hand("dealer");

        // reset deck
        Deck card = new Deck();



        public void StartGame()
        {

            Hand[] players = { npc1, player, npc2, dealer };
            
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

            Debug.WriteLine("player hand");
            Debug.WriteLine(player.GetHand());
            Debug.WriteLine(player.GetScore());

            Debug.WriteLine("dealer");

            Debug.WriteLine(dealer.GetHand());
            Debug.WriteLine(dealer.GetScore());

            NPC1Turn();
        }

        private void Stick()
        {

            string newText = "Player2's turn";

            update_label.Text += newText;

            NPC2Turn();
        }
        private void Twist()
        {
            Card new_card = card.Deal();
            player.AddCard(new_card);
            UpdateLabel(player.GetHand());
            int score = player.GetScore();

            if (score > 21)
            {
                UpdateLabel("Bust. Player2's turn");

                NPC2Turn();
            }
            else if (score == 21)
            {
                UpdateLabel("Blackjack! Press stick!");
            }

            Debug.WriteLine("player");
            Debug.WriteLine(player.GetHand());
            Debug.WriteLine(score);
        }




        // player 1 takes turn - stick or bust- print actions

        private void NPC1Turn()
        {
            DealerLabel("Player1's turn");
            int npc1_score = npc1.GetScore();

            // print player's cards
            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            if (currentWindow?.Page is not null)
            {
                var blackjackPage = (Blackjack)currentWindow.Page.Navigation.NavigationStack.Last();
                blackjackPage.DealerLabel(npc1.GetHand());
            }

            if (npc1_score < 13)
            {
                Card new_card = card.Deal();
                npc1.AddCard(new_card);
                npc1_score = npc1.GetScore();
                DealerLabel(npc1.GetHand());

                Debug.WriteLine("npc1:");
                Debug.WriteLine(npc1.GetHand());
                Debug.WriteLine(npc1_score);
            } 


            if (npc1_score > 21)
            {
                DealerLabel("Player1 Busts. Your turn");
            }
            else
            {
                DealerLabel("Your turn");
            }
        }


        // player 2 takes turn - stick or bust - print actions

        private void NPC2Turn()
        {
            DealerLabel("Player2's turn");
            Debug.WriteLine("player2");
            int npc2_score = npc2.GetScore();

            // print player's cards
            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            if (currentWindow?.Page is not null)
            {
                var blackjackPage = (Blackjack)currentWindow.Page.Navigation.NavigationStack.Last();
                blackjackPage.DealerLabel(npc2.GetHand());
            }

            if (npc2_score < 17)
            {
                Card new_card = card.Deal();
                npc2.AddCard(new_card);
                npc2_score = npc2.GetScore();
                DealerLabel(npc2.GetHand());

                Debug.WriteLine("npc2:");
                Debug.WriteLine(npc2.GetHand());
                Debug.WriteLine(npc2_score);
            }


            if (npc2_score > 21)
            {
                DealerLabel("Player2 Busts. Dealer's turn");
            }
            else
            {
                DealerLabel("Dealer's turn");
            }

            DealersTurn();
        }



        private void DealersTurn()
        {
            int dealer_score = dealer.GetScore();
            int player_score = player.GetScore();
            string player_status = Status(player);
            string npc1_status = Status(npc1);
            string npc2_status = Status(npc2);
            string[] statuses = { player_status, npc1_status, npc2_status };
            string[] names = { "you", "Player1", "Player2" };

            // print player's cards
            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            if (currentWindow?.Page is not null)
            {
                var blackjackPage = (Blackjack)currentWindow.Page.Navigation.NavigationStack.Last();
                blackjackPage.DealerLabel(dealer.GetHand());
            }

            do
            {
                Card new_card = card.Deal();
                dealer.AddCard(new_card);
                dealer_score = dealer.GetScore();
                DealerLabel(dealer.GetHand());

                Debug.WriteLine("dealer:");
                Debug.WriteLine(dealer.GetHand());
                Debug.WriteLine(dealer_score);
            } while (dealer_score < player_score);

            if (dealer_score > 21)
            {
                foreach (string status in statuses)
                {
                    if (status == "Winner")
                    {
                        int index = Array.IndexOf(statuses, status);
                        string name = names[index];
                        UpdateLabel($"Dealer Busts. {name} wins!");
                    }
                }
                UpdateLabel("Dealer Busts. Player wins!");
            }
            else if (dealer_score == 21)
            {
                UpdateLabel("Dealer Blackjack");
            }
            else
            {
                UpdateLabel("Dealer wins with score of " + dealer_score);
            }
        }
        // end of game
        

        public string Status(Hand hand)
        {
            int[] players_scores = { npc1.GetScore(), player.GetScore(), npc2.GetScore(), dealer.GetScore() };

            if (hand.GetScore() > 21)
            {
                return "Bust";
            }
            else if (hand.GetScore() == 21)
            {
                return "Blackjack";
            }
            else if (hand.GetScore() == players_scores.Max())
            {
                return "Winner";
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