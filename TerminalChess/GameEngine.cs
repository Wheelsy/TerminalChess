using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class GameEngine
    {
        private Player p1;
        private Player p2;
        private int cols = 8;
        private int rows = 8;
        private List<Square> board;
        private int numPieces;

        /// <summary>
        /// Constructor for a new game
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public GameEngine(Player p1, Player p2) { 
            this.p1 = p1;
            this.p2 = p2;
            numPieces = 32;

            for(int i = 0; i < numPieces; i++)
            {

            }
        }

        public string View()
        {
            string view = "";
            Utils utils = new();
            string currentPlayer = (p1.MyTurn) ? p1.GetUsername() : p2.GetUsername();
            
            for(int i = 0; i < cols; i++)
            {

            }

            utils.Print($"{currentPlayer} your turn:");

            return view;
        }
    }
}
