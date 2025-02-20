using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    internal class Hand
    {
        private List<Card> cards;

        public Hand()
        {
            this.cards = new List<Card>();
        }

        public void AddCard(Card card)
        {
            if (this.cards.Count < 2)
            {
                this.cards.Add(card);
            }
            else
            {
                throw new InvalidOperationException("You cannot add another card.");
            }
        }

        public List<Card> GetCards() 
        {
            return this.cards; 
        }

        public void ClearHand()
        {
            this.cards.Clear();
        }

        public int EvaluateHand(List<Card> communityCards)
        {
            List<Card> total = new List<Card>();
            total.AddRange(this.cards);
            total.AddRange(communityCards);
            if (IsRoyalFlush(total)) return 10;
            if (IsStraightFlush(total)) return 9;
            if (IsFourOfAKind(total)) return 8;
            if (IsFullHouse(total)) return 7;
            if (IsFlush(total)) return 6;
            if (IsStraight(total)) return 5;
            if (IsThreeOfAKind(total)) return 4;
            if (IsTwoPair(total)) return 3;
            if (IsOnePair(total)) return 2;
            return 1;
        }

        public bool IsOnePair(List<Card> total)
        {
            int[] counter = new int[13];
            foreach (Card card in total)
            {
                counter[card.GetValue() - 2]++;
            }
            for (int i = 0; i < counter.Length; i++)
            {
                if (counter[i] > 1)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsTwoPair(List<Card> total)
        {
            int[] counter = new int[13];
            bool onePair = false;
            foreach (Card card in total)
            {
                counter[card.GetValue() - 2]++;
            }
            for (int i = 0; i < counter.Length; i++)
            {
                if (counter[i] > 1)
                {
                    onePair = true;
                    if (onePair)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsThreeOfAKind(List<Card> total)
        {
            int[] counter = new int[13];
            foreach (Card card in total)
            {
                counter[card.GetValue() - 2]++;
            }
            for (int i = 0; i < counter.Length; i++)
            {
                if (counter[i] > 2)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsStraight(List<Card> total)
        {
            int[] counter = new int[26];
            int cnt = 0;
            foreach (Card card in total)
            {
                counter[card.GetValue() - 2]++;
                counter[card.GetValue() + 11]++;
            }
            for (int i = 0; i < counter.Length; i++)
            {
                if (counter[i] > 0)
                {
                    cnt++;
                }
                else
                {
                    cnt = 0;
                }
            }
            if (cnt > 4)
            {
                return true;
            }
            return false;
        }
        
        public bool IsFlush(List<Card> total)
        {
            int[] counter = new int[4];
            foreach (Card card in total)
            {
                if (card.GetSuit() == "Hearts")
                {
                    counter[0]++;
                }
                else if (card.GetSuit() == "Diamonds")
                {
                    counter[1]++;
                }
                else if (card.GetSuit() == "Spades")
                {
                    counter[2]++;
                }
                else
                {
                    counter[3]++;
                }
            }
            for (int i = 0; i < counter.Length; i++)
            {
                if (counter[i] > 4)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsFullHouse(List<Card> total)
        {
            return IsThreeOfAKind(total) && IsTwoPair(total);
        }

        public bool IsFourOfAKind(List<Card> total)
        {
            int[] counter = new int[13];
            foreach (Card card in total)
            {
                counter[card.GetValue() - 2]++;
            }
            for (int i = 0; i < counter.Length; i++)
            {
                if (counter[i] > 3)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsStraightFlush(List<Card> total)
        {
            return IsStraight(total) && IsFlush(total); // Check for edge cases!!!
        }

        public bool IsRoyalFlush(List<Card> total)
        {
            if (IsStraightFlush(total))
            {
                foreach (Card card in total)
                {
                    if (card.GetValue() == 14)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
