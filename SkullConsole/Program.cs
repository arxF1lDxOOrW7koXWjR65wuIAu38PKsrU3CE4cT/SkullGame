using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkullConsole
{
    class SkullConsole
    {
        static void Main(string[] args)
        {
            List<Skullbot> bots = new List<Skullbot>() { new Shrekbot(), new Shrekbot(), new Shrekbot(), new Shrekbot()};
            SkullGame game = new SkullGame(bots);
            game.PlayRound();
        }
    }
}
