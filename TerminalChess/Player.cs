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
        public bool Winner { get; set; }
        public int Score { get; set; }
        public List<string> capturedPieces;
        public bool InCheck { get; set; }
        public bool IsAI { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="username"></param>
        public Player(string username, bool isAI) { 
            this.username = username;
            this.Winner = false;
            this.Score = 0;
            capturedPieces = new();
            InCheck = false;
            this.IsAI = isAI;
        }
    }
}
