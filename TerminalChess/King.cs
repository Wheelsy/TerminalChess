using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class King : Piece
    {
        public King(Colour colour) : base("K", 0, colour)
        {
        }

        protected override void CalculatePossibleMoves()
        {
            base.CalculatePossibleMoves();
        }
    }
}
