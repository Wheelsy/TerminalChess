using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class Knight : Piece
    {
        public Knight(Colour colour) : base("N", 3, colour)
        {
        }

        protected override void CalculatePossibleMoves()
        {
            base.CalculatePossibleMoves();
        }
    }
}
