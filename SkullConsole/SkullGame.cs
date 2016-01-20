using System;
using System.Collections.Generic;

namespace SkullConsole
{
    public class SkullGame
    {

        List<SkullBotInfo> bots = new List<SkullBotInfo>();

        int currentPlayer = 0;
        public SkullGame(List<Skullbot> players)
        {
            foreach(Skullbot player in players)
            {
                this.bots.Add(new SkullBotInfo(player));
            }
        }

        public void PlayRound()
        {

            foreach (SkullBotInfo info in bots)
            {
                info.cards = new Stack<bool>();
            }
           //True for a skull, false for flower
            foreach(SkullBotInfo bot in bots)
            {
                int play = bot.b.GetMove();
                if (play == 0) bot.cards.Push(false);
                else if (play == -1) bot.cards.Push(true); //this need more validation
                else
                {

                }
                Notify(bot.name + " PLAY");
            }
            currentPlayer = currentPlayer % bots.Count;
            bool bet = false;
            while(!bet)
            {
                int action = bots[currentPlayer].b.GetMove();
                if(action == 0)
                {
                    bots[currentPlayer].cards.Push(false);
                    Notify(bots[currentPlayer].name + " PLAY");
                    currentPlayer = (currentPlayer + 1) % bots.Count;
                }
                else if(action == -1)
                {
                    bots[currentPlayer].cards.Push(true);
                    Notify(bots[currentPlayer].name + " PLAY");
                    currentPlayer = (currentPlayer + 1) % bots.Count;
                }
                else if(action > 0)
                {
                    bet = true;
                    Notify(bots[currentPlayer].name + " BET " + action);
                    BettingRound(action);
                }
            }
        }

        private void BettingRound(int bet)
        {
            Queue<int> bettingPlayers = new Queue<int>();
            for(int i = (currentPlayer + 1) % bots.Count; i != currentPlayer; i = (i + 1) % bots.Count)
            {
                bettingPlayers.Enqueue(i);
            }
            bettingPlayers.Enqueue(currentPlayer);

            while(bettingPlayers.Count > 1)
            {
                int cur = bettingPlayers.Dequeue();
                int action = bots[cur].b.GetMove();
                if(action == -2)
                {
                    Notify(bots[cur].name + " FOLD");
                }
                else if(action > bet)
                {
                    bettingPlayers.Enqueue(cur);
                    bet = action;
                    Notify(bots[cur].name + " BET " + bet);
                }
                else
                {
                    //invalid bet, handle this later
                }
            }
            currentPlayer = bettingPlayers.Dequeue();
            Flip(currentPlayer, bet);
        }

        private void Flip(int flipper, int flips)
        {
            bool skull = false;
            while (bots[flipper].cards.Count != 0 && flips > 0)
            {
                bool card = bots[flipper].cards.Pop();
                if (card)
                {
                    Notify(bots[flipper].name + " FLIPS OWN SKULL");
                    skull = true;
                    Punish(bots[flipper]);
                    Notify("RESET");
                    this.PlayRound();
                }
                else
                {
                    Notify(bots[flipper].name + " FLIPS OWN FLOWER");
                }
                flips--;
            }//done with own flips
            if (!skull)
            {
                bool won = true;
                for (; flips > 0; flips--)
                {
                    string player = bots[flipper].b.Flip();
                    SkullBotInfo bot = null;
                    bool card = true;
                    if (player != null)
                    {
                        foreach (SkullBotInfo b in bots)
                        {
                            if (b.name == player)
                            {
                                bot = b;
                                card = b.cards.Pop();
                                break;
                            }
                        }
                    }
                    if (card)
                    {
                        Notify(bots[flipper].name + " FLIPS " + bot.name + " SKULL");
                        Punish(bots[flipper]);
                        won = false;
                        Notify("RESET");
                        this.PlayRound();
                        break;
                    }
                    else
                    {
                        Notify(bots[flipper].name + " FLIPS " + bot.name + " FLOWER");
                    }
                }
                if (won)
                {
                    Console.WriteLine(bots[flipper].name + " WINS ROUND");
                    if (bots[flipper].won)
                    {
                        Console.WriteLine(bots[flipper].name + " WINS GAME");
                    }
                    else
                    {
                        bots[flipper].won = true;
                        Notify("RESET");
                        this.PlayRound();
                    }
                }
            }
        }

        private void Punish(SkullBotInfo skullbot)
        {
            if(skullbot.cardsRemaining == 1)
            {
                bots.Remove(skullbot);
                Notify(skullbot.name + " LOSES GAME");
                if (bots.Count == 1)
                {
                    Console.WriteLine(bots[0].name + " WINS GAME");
                    Environment.Exit(0);
                }
            }
            else
            {
                Random r = new Random();
                if(r.Next(skullbot.cardsRemaining) == 0)//lose the skull
                {
                    skullbot.skull = false;
                }
                else
                {

                }
                skullbot.cardsRemaining--;
            }

        }

        public void Notify(string info)
        {
            foreach(SkullBotInfo bot in bots)
            {
                bot.b.SeePlay(info);
            }
            Console.WriteLine(info);
        }
        public class SkullBotInfo
        {
            public static int id = 0;
            public Stack<bool> cards { get; set; }
            public bool won { get; set; } = false;
            public string name { get; }
            public Skullbot b { get; }
            public int cardsRemaining { get; set; } = 4;
            public bool skull { get; set; }
            public SkullBotInfo(Skullbot b)
            {
                this.b = b;
                this.name = b.GetName();
            }
        }
    }
}