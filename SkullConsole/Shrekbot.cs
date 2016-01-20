using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkullConsole
{
    public class Shrekbot : Skullbot
    {
        static int shrekID = 0;
        int bet = 0;
        int cardsOut = 0;
        static Random r = new Random();
        int thisID = -1;

        IList<string> names = new List<string>();
        Dictionary<string, int> cardsDown = new Dictionary<string, int>();
        public string Flip() //return the name of the bot you want to flip as a string
        {
            cardsDown[this.GetName()] = 0;
            foreach(string s in cardsDown.Keys)
            {
                if (cardsDown[s] != 0)
                {
                    cardsDown[s]--;
                    return s;
                }
            }
            return null;
        } 
        public string GetName() //this is only called once so don't even think about changing name midway through
        {
            if(thisID == -1) thisID = shrekID++;
            return "SHREKBOT" + thisID;
        }
        public int GetMove() // 0 to play a flower, -1 to play a skull, -2 to fold, any positive number 16 or less to bet
        {
            if(bet != 0)
            {
                if(bet < 4)
                {
                    bet = bet + 1;
                    return bet;
                }
                else
                {
                    return -2;
                }
            }
            else if(cardsOut > 4 && cardsOut > r.Next(16))
            {
                bet = 1;
                return bet;
            }
            else
            {
                if (r.Next(4) == 0) return -1;
                else return 0;
            }
        }

        public void SeePlay(string info) //this is to recieve info about plays other bots have made. Possible inputs are below
        {
            string[] arr = info.Split(' ');
            if (!names.Contains(arr[0]))
            {
                cardsDown.Add(arr[0], 0);
                names.Add(arr[0]);
            }
            if (arr[0] == "RESET")
            {
                bet = 0;
                cardsOut = 0;
                cardsDown = new Dictionary<string, int>();
                foreach(string s in names)
                {
                    cardsDown.Add(s, 0);
                }
            }
            else if (arr[1] == "BET")
            {
                bet = int.Parse(arr[2]);
            }
            else if(arr[1] == "PLAY")
            {
                cardsOut++;
                cardsDown[arr[0]]++;
            }
            else if(arr[1] == "LOSES")
            {
                names.Remove(arr[0]);
            }
        }
        /**

        [bot] PLAY
        [bot] FOLD
        [bot] BET [amount]
        [bot] FLIPS OWN FLOWER
        [bot] FLIPS OWN SKULL
        [bot] FLIPS [bot] SKULL
        [bot] FLIPS [bot] FLOWER
        RESET
        [bot] WINS ROUND
        [bot] WINS GAME

    **/
    }
}
