using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    internal class Deck
    {
        private List<Card> cards;

        public Deck()
        {
            for (int i = 2; i < 15; i++)
            {
                cards.Add(new Card(i, "Hearts"));
                cards.Add(new Card(i, "Diamonds"));
                cards.Add(new Card(i, "Spades"));
                cards.Add(new Card(i, "Clubs"));
            }
        }

        public void Shuffle()
        {
            for (int i = cards.Count - 1; i > 0; i--)
            {
                Random rand = new Random();
                int j = rand.Next(0, i);
                Card temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
        }

        public Card DealSingleCard()
        {
            Card dealt = cards[cards.Count - 1];
            cards.RemoveAt(cards.Count - 1);
            return dealt;
        }
    }
}
