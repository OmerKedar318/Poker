using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    internal class Card
    {
        private int value;
        private string suit;

        public Card(int value, string suit)
        {
            this.value = value;
            this.suit = suit;
        }

        public int GetValue()
        {
            return this.value;
        }

        public string GetSuit()
        {
            return this.suit;
        }
    }
}
