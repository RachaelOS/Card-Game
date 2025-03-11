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


        private async void image_update(string game_status)
        {
            // Set the initial position of the new background image (off-screen)

            // Animate the old background sliding out
            await game_image.TranslateTo(-this.Width, 0, 1000, Easing.SinInOut);



            switch (game_status)
            {
                case "dealer":
                    //BackgroundImageSource = "dealing.jpg";
                    game_image.Source = "dealer.PNG";
                    // Set the initial position of the new background image (off-screen)
                    game_image.TranslationX = this.Width;
                    // Animate the new background image sliding in
                    await game_image.TranslateTo(0, 0, 1000, Easing.SinInOut);
                    break;
                case "npc1":
                    game_image.Source = "npc1.png";
                    // Set the initial position of the new background image (off-screen)
                    game_image.TranslationX = this.Width;
                    // Animate the new background image sliding in
                    await game_image.TranslateTo(0, 0, 1000, Easing.SinInOut);
                    break;
                case "npc2":
                    game_image.Source = "npc2.png";
                    // Set the initial position of the new background image (off-screen)
                    game_image.TranslationX = -this.Width;
                    // Animate the new background image sliding in
                    await game_image.TranslateTo(0, 0, 1000, Easing.SinInOut);
                    break;
                case "all":
                    game_image.Source = "main_transparent.png";
                    // Set the initial position of the new background image (off-screen)
                    game_image.TranslationX = this.Width;
                    // Animate the new background image sliding in
                    await game_image.TranslateTo(0, 0, 1000, Easing.SinInOut);
                    break;
                case "end":
                    game_image.Source = "game_over.jpg";
                    // Set the initial position of the new background image (off-screen)
                    game_image.TranslationX = this.Width;
                    // Animate the new background image sliding in
                    await game_image.TranslateTo(0, 0, 1000, Easing.SinInOut);
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

            public string Img_Name { get; set; }

            public Card(string suit, string rank, int value, string img_name)
            {
                Suit = suit;
                Rank = rank;
                Value = value;
                Img_Name = img_name;
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

                        string img_name;
                        string number;
                        //cards.Add(new Card(suit, rank));
                        switch (rank)
                        {
                            case "2":
                                number = "two";
                                img_name = number + "_of_" + suit.ToLower() + ".png";
                                cards.Add(new Card(suit, rank, 2, img_name));
                                break;
                            case "3":
                                number = "three";
                                img_name = number + "_of_" + suit.ToLower() + ".png";
                                cards.Add(new Card(suit, rank, 3, img_name));
                                break;
                            case "4":
                                number = "four";
                                img_name = number + "_of_" + suit.ToLower() + ".png";
                                cards.Add(new Card(suit, rank, 4, img_name));
                                break;
                            case "5":
                                number = "five";
                                img_name = number + "_of_" + suit.ToLower() + ".png";
                                cards.Add(new Card(suit, rank, 5, img_name));
                                break;
                            case "6":
                                number = "six";
                                img_name = number + "_of_" + suit.ToLower() + ".png";
                                cards.Add(new Card(suit, rank, 6, img_name));
                                break;
                            case "7":
                                number = "seven";
                                img_name = number + "_of_" + suit.ToLower() + ".png";
                                cards.Add(new Card(suit, rank, 7, img_name));
                                break;
                            case "8":
                                number = "eight";
                                img_name = number + "_of_" + suit.ToLower() + ".png";
                                cards.Add(new Card(suit, rank, 8, img_name));
                                break;
                            case "9":
                                number = "nine";
                                img_name = number + "_of_" + suit.ToLower() + ".png";
                                cards.Add(new Card(suit, rank, 9, img_name));
                                break;
                            case "10":
                                number = "ten";
                                img_name = number + "_of_" + suit.ToLower() + ".png";
                                cards.Add(new Card(suit, rank, 10, img_name));
                                break;
                            case "Jack":
                                img_name = rank.ToLower() + "_of_" + suit.ToLower() + ".png";
                                cards.Add(new Card(suit, rank, 10, img_name));
                                break;
                            case "Queen":
                                img_name = "queen_of_" + suit.ToLower() + ".png";
                                cards.Add(new Card(suit, rank, 10, img_name));
                                break;
                            case "King":
                                img_name = "king_of_" + suit.ToLower() + ".png";
                                cards.Add(new Card(suit, rank, 10, img_name));
                                break;
                            case "Ace":
                                img_name = "ace_of_" + suit.ToLower() + ".png";
                                cards.Add(new Card(suit, rank, 11, img_name));
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

            public int CountCards()
            {
                return Cards.Count;
            }
        }
        private void AddNewImage(Card card, Hand player)
        {
            string img_name = card.Img_Name;
            Debug.WriteLine(img_name);
            int gridcolumn = player.CountCards() - 1;    
            
            // Add a new column to the grid if needed
            if (handGrid.ColumnDefinitions.Count <= gridcolumn)
            {
                var columnDefinition = new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                };

                // Add the ColumnDefinition to the Grid
                handGrid.ColumnDefinitions.Add(columnDefinition);
            }
            // Create a new Image object
            var newImage = new Image
            {
                Source = $"/Resources/Images/deck/{img_name}", // Set the correct path to your image
                Aspect = Aspect.AspectFit,
                HeightRequest = 100,
                WidthRequest = 70
            };

            // Add the new Image to the Grid
            Grid.SetColumn(newImage, gridcolumn); // Set the column position
            this.Content.FindByName<Grid>("handGrid").Children.Add(newImage); // Add the new image to the grid
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

            image_update("dealer");

            UpdateLabel("Dealer is dealing the cards...");
            await Task.Delay(3000);

            // deal cards - 2 each 
            foreach (Hand hand in players)
            {
                Card new_card1 = card.Deal();
                hand.AddCard(new_card1);
                if (hand == player)
                {
                    AddNewImage(new_card1, hand);
                }
                Card new_card2 = card.Deal();
                hand.AddCard(new_card2);
                if (hand == player)
                {
                    AddNewImage(new_card2, hand);
                }

            }




            // print player's cards
            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            if (currentWindow?.Page is not null)
            {
                var blackjackPage = (Blackjack)currentWindow.Page.Navigation.NavigationStack.Last();
                blackjackPage.HandLabel(player.GetHand());
            }


            image_update("all");

            Debug.WriteLine("player hand: " + player.GetHand());
            Debug.WriteLine("player score: " + player.GetScore());

            Debug.WriteLine("dealer hand: " + dealer.GetHand());
            Debug.WriteLine("dealer score: " + dealer.GetScore());

            Debug.WriteLine("npc1 hand: " + npc1.GetHand());
            Debug.WriteLine("npc1 score: " + npc1.GetScore());

            Debug.WriteLine("npc2 hand: " + npc2.GetHand());
            Debug.WriteLine("npc2 score: " + npc2.GetScore());


            await Task.Delay(3000);

            NPC1Turn();
        }

        private async void Stick()
        {
            if (player_turn == true)
            {
                player_turn = false;
                UpdateLabel("You chose to stick! Player2's turn...");
                Debug.WriteLine("player stick");
                await Task.Delay(2000);

                NPC2Turn();
            }
        }
        private async void Twist()
        {
            if (player_turn == true)
            {
                image_update("dealer");

                await Task.Delay(1000);

                Card new_card = card.Deal();
                player.AddCard(new_card);
                HandLabel(player.GetHand());
                AddNewImage(new_card, player);
                int score = player.GetScore();

                Debug.WriteLine("player twist");
                Debug.WriteLine("player hand: " + player.GetHand());
                Debug.WriteLine("player score: " + score);

                if (score > 21)
                {
                    image_update("all");
                    UpdateLabel("Bust! Player2's turn...");
                    await Task.Delay(3000);

                    NPC2Turn();
                }
                else if (score == 21)
                {
                    image_update("all");
                    UpdateLabel("Blackjack! Press stick!");
                    await Task.Delay(1000);
                }

                
            }

        }




        // player 1 takes turn - stick or bust- print actions

        private async void NPC1Turn()
        {
            Debug.WriteLine("player1's turn");
            int npc1_score = npc1.GetScore();

            image_update("npc1");
            UpdateLabel("Player1's turn...");
            await Task.Delay(3000);


            if (npc1_score < 13)
            {
                image_update("dealer");
                Card new_card = card.Deal();
                npc1.AddCard(new_card);
                npc1_score = npc1.GetScore();

                Debug.WriteLine("npc1 new hand: " + npc1.GetHand());
                Debug.WriteLine("npc1 new score: " + npc1_score);

                UpdateLabel("Player1 twists");
                await Task.Delay(3000);
            }


            if (npc1_score > 21)
            {
                image_update("npc1");
                UpdateLabel("Player1 is bust!");
                Debug.WriteLine("npc1 busts");
                await Task.Delay(3000);
            }
            else
            {
                image_update("npc1");
                UpdateLabel("Player1 chose to stick.");
                await Task.Delay(3000);
            }

            image_update("all");

            UpdateLabel("Your turn");

            await Task.Delay(1000);
            player_turn = true;
        }


        // player 2 takes turn - stick or bust - print actions

        private async void NPC2Turn()
        {
            Debug.WriteLine("player2 turn");
            image_update("npc2");
            UpdateLabel("Player2's turn...");
            await Task.Delay(3000);

            int npc2_score = npc2.GetScore();

            if (npc2_score < 17)
            {
                image_update("dealer");
                Card new_card = card.Deal();
                npc2.AddCard(new_card);
                npc2_score = npc2.GetScore();

                UpdateLabel("Player2 twists");
                await Task.Delay(3000);

                Debug.WriteLine("npc2 new hand: " + npc2.GetHand());
                Debug.WriteLine("npc2 new hand: " + npc2_score);
            }


            if (npc2_score > 21)
            {
                image_update("npc2");
                Debug.WriteLine("npc2 busts");
                UpdateLabel("Player2 is bust! Dealer's turn...");
                await Task.Delay(300);
            }
            else
            {
                image_update("npc2");   
                UpdateLabel("Player2 chose to stick. Dealer's turn...");
                await Task.Delay(3000);
            }

            DealersTurn();
        }



        private async void DealersTurn()
        {
            image_update("dealer");
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
            await Task.Delay(1500);

            while ((dealer_score < player_score || dealer_score < npc1_score || dealer_score < npc2_score) && dealer_score < 21)
            {
                
                Card new_card = card.Deal();
                dealer.AddCard(new_card);
                dealer_score = dealer.GetScore();

                Debug.WriteLine("dealer new hand: " + dealer.GetHand());
                Debug.WriteLine("dealer score: " + dealer_score);

                UpdateLabel("Dealer twists \nDealer's hand: \n" + dealer.GetHand());
                await Task.Delay(1500);
            }

            image_update("end");

            if (dealer_score > 21)
            {
                Debug.WriteLine("dealer bust");
                dealer_score = 0;

                Winner();

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
        public async void Winner()
        {
            player.Name = "You";
            Hand[] players = { npc1, player, npc2 };
            List<Hand> candidates = new List<Hand>();
            List<int> scores = new List<int>();
            List<Hand> highest_scores = new List<Hand>();

            //only consider players who are not bust
            foreach (Hand name in players)
            {
                if (name.Status() != "Bust")
                {
                    candidates.Add(name);
                }
            }

            //add candidate scores to list
            foreach (Hand candidate in candidates)
            {
                scores.Add(candidate.GetScore());
            }

            int highest_score = scores.Max();


            //if there are multiple winners, randomly select winner
            if (scores.Count(x => x == highest_score) > 1)
            {
                foreach (Hand candidate in candidates)
                {
                    if (candidate.GetScore() == highest_score)
                    {
                        highest_scores.Add(candidate);
                    }
                }
                Random rng = new Random();
                int index = rng.Next(0, highest_scores.Count);
                Hand winner = highest_scores[index];

                UpdateLabel($"There's a draw! Randomly selecting winner!");
                Debug.WriteLine($"There's a draw! Randomly selecting winner!");
                await Task.Delay(2000);

                UpdateLabel($"Dealer Busts. {winner.Name} wins!");
                Debug.WriteLine($"Dealer Busts. {winner.Name} wins!");
            }
            //if there is only one winner
            else
            {
                int index = scores.IndexOf(highest_score);
                Hand winner = candidates[index];

                if (winner.GetScore() == 21)
                {
                    UpdateLabel($"Blackjack! {winner.Name} wins!");
                    Debug.WriteLine($"Blackjack! {winner.Name} wins!");
                }
                else
                {
                    UpdateLabel($"{winner.Name} wins with score of {highest_score}");
                    Debug.WriteLine($"{winner.Name} wins with score of {highest_score}");
                }

            }




        }
    }
}