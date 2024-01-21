using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class Rook : Piece
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="colour"></param>
        public Rook(Colour colour) : base("", 5, colour)
        {
            // Set append the name string with the appropraite colour code
            name += (colour == Piece.Colour.Black) ? "R".Pastel(Color.Chocolate) : "R".Pastel(Color.SandyBrown);
        }

        /// <summary>
        /// Overriden method to caluculate the legal moves for a Rook
        /// </summary>
        protected override void CalculatePossibleMoves()
        {
            base.CalculatePossibleMoves();
        }
    }
}
