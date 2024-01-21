using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class Bishop : Piece
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="colour"></param>
        public Bishop(Colour colour) : base("", 3, colour)
        {
            // Set append the name string with the appropraite colour code
            name += (colour == Piece.Colour.Black) ? "B".Pastel(Color.Chocolate) : "B".Pastel(Color.SandyBrown);
        }

        /// <summary>
        /// Overriden method to caluculate the legal moves for a Bishop
        /// </summary>
        protected override void CalculatePossibleMoves()
        {
            base.CalculatePossibleMoves();
        }
    }
}
