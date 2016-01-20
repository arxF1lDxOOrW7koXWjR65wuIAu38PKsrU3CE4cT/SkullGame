using System;
using System.Collections.Generic;

namespace SkullConsole
{
    public interface Skullbot
    {

        string Flip();
        string GetName();
        int GetMove(); // 0 to play a flower, -1 to play a skull, -2 to fold, any positive number 16 or less to bet that much

        void SeePlay(string play); //Every time any player makes a play, this will be called for each bot
    }
}