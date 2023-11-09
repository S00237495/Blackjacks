namespace Blackjack
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Blackjack!");

            while (true)
            {
                PlayBlackjack();
                Console.Write("Do you want to play again? (y/n): ");
                if (Console.ReadLine().ToLower() != "y")
                    break;
            }
        }

        static void PlayBlackjack()
        {
            // Initialize a new deck and shuffle it
            Deck deck = new Deck();
            deck.Shuffle();

            // Create player and dealer hands
            Hand playerHand = new Hand();
            Hand dealerHand = new Hand();

            // Deal the initial cards
            playerHand.AddCard(deck.DrawCard());
            dealerHand.AddCard(deck.DrawCard());
            playerHand.AddCard(deck.DrawCard());
            dealerHand.AddCard(deck.DrawCard());

            // Display the initial hands
            Console.WriteLine("Your hand: ");
            playerHand.Display();
            Console.WriteLine("Dealer's hand: ");
            dealerHand.DisplayDealer();

            // Player's turn
            while (true)
            {
                Console.Write("Do you want to Hit or Stand? (h/s): ");
                char choice = Console.ReadLine().ToLower()[0];
                if (choice == 'h')
                {
                    playerHand.AddCard(deck.DrawCard());
                    Console.WriteLine("Your hand: ");
                    playerHand.Display();
                    if (playerHand.GetTotal() > 21)
                    {
                        Console.WriteLine("Bust! You lose.");
                        return;
                    }
                }
                else if (choice == 's')
                {
                    break;
                }
            }

            // Dealer's turn
            Console.WriteLine("Dealer's turn:");
            dealerHand.Display();
            while (dealerHand.GetTotal() < 17)
            {
                dealerHand.AddCard(deck.DrawCard());
                dealerHand.Display();
                if (dealerHand.GetTotal() > 21)
                {
                    Console.WriteLine("Dealer busts! You win.");
                    return;
                }
            }

            // Determine the winner
            int playerTotal = playerHand.GetTotal();
            int dealerTotal = dealerHand.GetTotal();

            if (playerTotal > dealerTotal)
            {
                Console.WriteLine("You win!");
            }
            else if (dealerTotal > playerTotal)
            {
                Console.WriteLine("Dealer wins.");
            }
            else
            {
                Console.WriteLine("It's a tie.");
            }
        }
    }

    class Deck
    {
        private List<Card> cards;
        private Random random = new Random();
        //add on feature if jokes is pulled instant loss
        private bool hasJoker;

        public Deck(bool includeJoker = false)
        {
            cards = new List<Card>();

            hasJoker = includeJoker;

            foreach (string suit in new[] { "Hearts", "Diamonds", "Clubs", "Spades" })
            {
                for (int rank = 2; rank <= 10; rank++)
                {
                    cards.Add(new Card(rank.ToString(), suit));
                }
                cards.Add(new Card("Jack", suit));
                cards.Add(new Card("Queen", suit));
                cards.Add(new Card("King", suit));
                cards.Add(new Card("Ace", suit));
            }
            if (hasJoker)
            {
                cards.Add(new Card("JOKER", "JOKERSUIT"));
            }

            
        }

        public void Shuffle()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                int j = random.Next(i, cards.Count);
                Card temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
        }

        public Card DrawCard()
        {
            Card card = cards[0];
          
            
            cards.RemoveAt(0);

            if (hasJoker && card.Rank == "Joker")
            {
                Console.WriteLine("MWAHAHAHAAHAHA JOKER HAS BEEN PULLED HAHAHA YOU LLLLLLLOOOOOOOOSEEEEEE");
                Environment.Exit(0); 
                // Exit the game
            }

            return card;
        }
    }

    class Card
    {
      
        public string Rank { get; }
        public string Suit { get; }

        public Card(string rank, string suit)
      
        
        {
            Rank = rank;
            Suit = suit;
        }

        public int GetValue()
        {
            if (Rank == "Ace")
                return 11;
            if (Rank == "King" || Rank == "Queen" || Rank == "Jack")
                return 10;
            return int.Parse(Rank);
        }

        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }
    }



    class Hand
    {
        private List<Card> cards = new List<Card>();

        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        public int GetTotal()
        {
            int total = 0;
            int aces = 0;
            foreach (var card in cards)
            {
                total += card.GetValue();
                if (card.Rank == "Ace")
                {
                    aces++;
                }
            }

            while (total > 21 && aces > 0)
            {
                total -= 10;
                aces--;
            }

            return total;
        }
        //totalling all cards
        public void Display()
        {
            foreach (var card in cards)
            {
                Console.WriteLine(card);
            }
            Console.WriteLine($"Total: {GetTotal()}");
        }

        public void DisplayDealer()
        {
            Console.WriteLine(cards[0]);
            Console.WriteLine("Total: ?");
        }
    }
}