using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    internal class Player
    {
        private string name;
        private int chips;
        private Hand hand;
        private bool isFolded;

        public Player(string name, int chips)
        {
            this.name = name;
            this.chips = chips;
            this.hand = new Hand();
            this.isFolded = false;
        }

        public string GetName()
        {
            return this.name;
        }

        public int GetChips()
        {
            return this.chips;
        }

        public Hand GetHand()
        {
            return this.hand;
        }

        public bool IsFolded()
        {
            return this.isFolded;
        }

        public bool Bet(int amount)
        {
            if (this.chips < amount)
            {
                return false;
            }
            chips -= amount;
            return true;
        }

        public void WinPot(int amount)
        {
            this.chips += amount;
        }

        public void Fold()
        {
            this.isFolded = true;
        }

        public void ResetHand()
        {
            this.hand.ClearHand();
            this.isFolded = false;
        }
    }
}
