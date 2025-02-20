using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    internal class Game
    {
        private List<Player> players;
        private Deck deck;
        private int pot;
        private int dealerIndex;
        private int smallBlind;
        private int bigBlind;
        private List<Card> communityCards;

        public Game(List<string> names, int startingChips)
        {
            players = new List<Player>();
            deck = new Deck();
            pot = 0;
            communityCards = new List<Card>();
            foreach (string name in names)
            {
                players.Add(new Player(name, startingChips));
            }
            dealerIndex = 0;
            smallBlind = 10;
            bigBlind = 20;
        }

        public void StartRound()
        {
            ResetRound();
            deck.Shuffle();
            DealStartingCards();
            HandleBettingRound();
            DealFlop();
            HandleBettingRound();
            DealTurn();
            HandleBettingRound();
            DealRiver();
            HandleBettingRound();
            DetermineWinner();
            RotateDealer();
        }

        public void ResetRound()
        {
            pot = 0;
            communityCards.Clear();
            foreach (Player player in players)
            {
                player.ResetHand();
            }
        }

        public void DealStartingCards()
        {
            foreach (Player player in players)
            {
                player.GetHand().AddCard(deck.DealSingleCard());
                player.GetHand().AddCard(deck.DealSingleCard());
            }
        }

        public void DealFlop()
        {
            communityCards.Add(deck.DealSingleCard());
            communityCards.Add(deck.DealSingleCard());
            communityCards.Add(deck.DealSingleCard());
        }

        public void DealTurn()
        {
            communityCards.Add(deck.DealSingleCard());
        }

        public void DealRiver()
        {
            communityCards.Add(deck.DealSingleCard());
        }

        public void HandleBettingRound()
        {
            foreach (Player player in players)
            {
                if (!player.IsFolded())
                {
                    // Insert Betting Logic
                }
            }
        }

        public void DetermineWinner()
        {
            Player bestPlayer = players[0];
            int bestHandRank = bestPlayer.GetHand().EvaluateHand(communityCards);
            foreach (Player player in players)
            {
                if (!player.IsFolded())
                {
                    int handRank = player.GetHand().EvaluateHand(communityCards);
                    if (handRank > bestHandRank)
                    {
                        bestPlayer = player;
                        bestHandRank = handRank;
                    }
                }
            }
            // Handle ties
            bestPlayer.WinPot(pot);
            Console.WriteLine(bestPlayer.GetName() + " wins the pot of " + pot + " chips!");
        }

        public void RotateDealer()
        {
            dealerIndex = (dealerIndex + 1) % players.Count;
        }
    }
}
