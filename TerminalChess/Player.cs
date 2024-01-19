using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class Player
    {
        private string username;
        public bool MyTurn { get; set; }

        public Player(string username) { 
            this.username = username;
        }

        public string GetUsername()
        {
            return username;
        }
    }
}
