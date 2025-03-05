using System.Security.Cryptography.X509Certificates;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;

using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using System.Xml.Linq;


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

        private void image_update(string game_status)
        {
            switch (game_status)
            {
                case "dealing" :
                    //BackgroundImageSource = "dealing.jpg";
                    game_image.Source = "dealing.jpg";
                    break;
                case "playing":
                    game_image.Source = "playing.png";
                    break;
                case "end":
                    game_image.Source = "game_over.jpg";
                    break;
            }
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
                }

                //checks if score is > 21, if it is goes back over cards to change aces to 1
                if (Score > 21)
                {
                    Score = 0;
                    foreach (Card card in Cards)
                    {
                        //if added card is an ace,change value of Ace depending on current score
                        if (card.Rank == "Ace")
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
            public string Status()
            {
                GetScore();

                if (Score > 21)
                {
                    return "Bust";
                }
                else if (Score == 21)
                {
                    return "Blackjack";
                }
                else
                {
                    return "Playing";
                }
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

        private void HandLabel(string newText)
        {
            hand_label.Text = newText;
        }


        public Hand npc1 = new Hand("Player1");
        public Hand npc2 = new Hand("Player2");
        public Hand player = new Hand("you");
        public Hand dealer = new Hand("dealer");

        bool player_turn = false;

        // reset deck
        Deck card = new Deck();



        public async void StartGame()
        {

            Hand[] players = { npc1, player, npc2, dealer };
            
            // shuffle deck
            card.Shuffle();

            image_update("dealing");

            UpdateLabel("Dealer is dealing the cards...");
            await Task.Delay(2000);

            // deal cards - 2 each 
            foreach (Hand hand in players)
            {
                Card new_card1 = card.Deal();
                Card new_card2 = card.Deal();
                hand.AddCard(new_card1);
                hand.AddCard(new_card2);
            }

            // print player's cards
            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            if (currentWindow?.Page is not null)
            {
                var blackjackPage = (Blackjack)currentWindow.Page.Navigation.NavigationStack.Last();
                blackjackPage.HandLabel(player.GetHand());
            }

            image_update("playing");

            Debug.WriteLine("player hand: " + player.GetHand());
            Debug.WriteLine("player score: " + player.GetScore());
             
            Debug.WriteLine("dealer hand: " + dealer.GetHand());
            Debug.WriteLine("dealer score: " + dealer.GetScore());

            Debug.WriteLine("npc1 hand: " + npc1.GetHand());
            Debug.WriteLine("npc1 score: " + npc1.GetScore());

            Debug.WriteLine("npc2 hand: " + npc2.GetHand());
            Debug.WriteLine("npc2 score: " + npc2.GetScore());

            NPC1Turn();
        }

        private async void Stick()
        {
            if (player_turn == true)
            {
                player_turn = false;
                UpdateLabel("You chose to stick! Player2's turn...");
                await Task.Delay(2000);

                NPC2Turn();
            }
        }
        private async void Twist()
        {
            if (player_turn == true)
            {
                image_update("dealing");

                Card new_card = card.Deal();
                player.AddCard(new_card);
                HandLabel(player.GetHand());
                int score = player.GetScore();

                Debug.WriteLine("player twist");
                Debug.WriteLine("player hand: " + player.GetHand());
                Debug.WriteLine("player score: " + score);

                if (score > 21)
                {
                    UpdateLabel("Bust! Player2's turn...");
                    await Task.Delay(2000);

                    NPC2Turn();
                }
                else if (score == 21)
                {
                    UpdateLabel("Blackjack! Press stick!");
                }

                image_update("playing");
            }

        }




        // player 1 takes turn - stick or bust- print actions

        private async void NPC1Turn()
        {
            Debug.WriteLine("player1's turn");
            int npc1_score = npc1.GetScore();

            UpdateLabel("Player1's turn...");
            await Task.Delay(1000);


            if (npc1_score < 13)
            {
                image_update("dealing");
                Card new_card = card.Deal();
                npc1.AddCard(new_card);
                npc1_score = npc1.GetScore();

                Debug.WriteLine("npc1 new hand: " + npc1.GetHand());
                Debug.WriteLine("npc1 new score: " + npc1_score);

                UpdateLabel("Player1 twists");
                await Task.Delay(1000);
            } 


            if (npc1_score > 21)
            {
                UpdateLabel("Player1 is bust!");
                Debug.WriteLine("npc1 busts");
                await Task.Delay(1000);
            }
            else
            {
                UpdateLabel("Player1 chose to stick.");
                await Task.Delay(1000);
            }

            image_update("playing");

            UpdateLabel("Your turn");
            player_turn = true;
        }


        // player 2 takes turn - stick or bust - print actions

        private async void NPC2Turn()
        {
            Debug.WriteLine("player2 turn");

            UpdateLabel("Player2's turn...");
            await Task.Delay(1000);

            int npc2_score = npc2.GetScore();

            if (npc2_score < 17)
            {
                image_update("dealing");
                Card new_card = card.Deal();
                npc2.AddCard(new_card);
                npc2_score = npc2.GetScore();

                UpdateLabel("Player2 twists");
                await Task.Delay(1000);

                Debug.WriteLine("npc2 new hand: " + npc2.GetHand());
                Debug.WriteLine("npc2 new hand: " + npc2_score);
            }


            if (npc2_score > 21)
            {
                Debug.WriteLine("npc2 busts");
                UpdateLabel("Player2 is bust! Dealer's turn...");
                await Task.Delay(1000);
            }
            else
            {
                UpdateLabel("Player2 chose to stick. Dealer's turn...");
                await Task.Delay(1000);
            }

            image_update("playing");
            DealersTurn();
        }



        private async void DealersTurn()
        {
            int dealer_score = dealer.GetScore();
            int player_score = player.GetScore();
            int npc1_score = npc1.GetScore();
            int npc2_score = npc2.GetScore();
            string player_status = player.Status();
            string npc1_status = npc1.Status();
            string npc2_status = npc2.Status();
            //keep statuses and names in same order
            string[] statuses = { player_status, npc1_status, npc2_status };
            int[] scores = { player_score, npc1_score, npc2_score };

            // sets variable for scores = 0 if that player is bust
            foreach (string status in statuses)
            {
                if (status == "Bust")
                {
                    int index = Array.IndexOf(statuses, status);
                    scores[index] = 0;
                }
        
            }


            // print dealer's cards
            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            if (currentWindow?.Page is not null)
            {
                var blackjackPage = (Blackjack)currentWindow.Page.Navigation.NavigationStack.Last();
                blackjackPage.UpdateLabel("Dealer's turn... \nDealer's hand: \n" + dealer.GetHand());
            }
            await Task.Delay(1000);

            while ((dealer_score < player_score || dealer_score < npc1_score || dealer_score < npc2_score) && dealer_score < 21) 
            {
                image_update("dealing");
                Card new_card = card.Deal();
                dealer.AddCard(new_card);
                dealer_score = dealer.GetScore();

                Debug.WriteLine("dealer new hand: " + dealer.GetHand());
                Debug.WriteLine("dealer score: " + dealer_score);

                UpdateLabel("Dealer twists \nDealer's hand: \n" + dealer.GetHand());
                await Task.Delay(1000);
            } 

            image_update("end");

            if (dealer_score > 21)
            {
                Debug.WriteLine("dealer bust");
                dealer_score = 0;

                Hand winner = Winner();
                UpdateLabel($"Dealer Busts. {winner.Name} wins!");
                Debug.WriteLine($"Dealer Busts. {winner.Name} wins!");

            }
            else if (dealer_score == 21)
            {
                UpdateLabel("Blackjack! Dealer wins!");
                Debug.WriteLine("Dealer Blackjack");
            }
            else
            {
                UpdateLabel("Dealer wins with score of " + dealer_score);
                Debug.WriteLine("Dealer wins with score of " + dealer_score);
            }
        }
        // end of game
        


        //determines winner of game
        public Hand Winner()
        {
            Hand[] players = {npc1, player, npc2, dealer};
            List<Hand> candidates = new List<Hand>();
            List<int> scores = new List<int>();


            foreach (Hand name in players)
            {
                if (name.Status() != "Bust")
                {
                    candidates.Add(name);
                }
            }

            foreach (Hand candidate in candidates)
            {
                scores.Add(candidate.GetScore());
            }

            int highest_score = scores.Max();
            int index = scores.IndexOf(highest_score);
            Hand winner = candidates[index];

            return winner;
        }


    }
}