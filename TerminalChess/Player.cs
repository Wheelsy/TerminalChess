using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class Player
    {
        public string username { get; set; }
        public bool MyTurn { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="username"></param>
        public Player(string username) { 
            this.username = username;
        }
    }
}
