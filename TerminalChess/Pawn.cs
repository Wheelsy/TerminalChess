using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class Pawn : Piece
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="colour"></param>
        public Pawn(Colour colour) : base("", 1, colour)
        {
            // Set append the name string with the appropraite colour code
            name += (colour == Piece.Colour.Black) ? "P".Pastel(Color.Chocolate) : "P".Pastel(Color.SandyBrown);
        }

        /// <summary>
        /// Overriden method to caluculate the legal moves for a Pawn
        /// </summary>
        protected override void CalculatePossibleMoves()
        {
            base.CalculatePossibleMoves();
        }
    }
}
