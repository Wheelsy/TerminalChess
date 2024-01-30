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
            Name += (colour == Piece.Colour.Black) ? "P".Pastel(Color.Chocolate) : "P".Pastel(Color.SandyBrown);
        }

        /// <summary>
        /// Overriden method to caluculate the legal moves for a Pawn
        /// </summary>
        protected override void CalculatePossibleMoves(int row, int col, GameEngine ge)
        {
            base.CalculatePossibleMoves(row, col, ge);

            if (ge.TurnNo == 1)
            {
                possibleMoves.Add((row + colourModifier, col));
                possibleMoves.Add((row + (2 * colourModifier), col));
                return;
            }       

            // Check if the square in front is occupied
            Square infront = ge.GetSquareAtPos(row + colourModifier, col);

            if(infront == null)
            {
                if (infront.piece != null)
                {
                    if (infront.piece.colour != this.colour)
                    {
                        possibleMoves.Add((infront.row, infront.col));
                    }
                }
            }

            // Check diagonals
            Square diagonal1 = ge.GetSquareAtPos(row + colourModifier, col + colourModifier);

            if (diagonal1 != null)
            {
                if (diagonal1.piece != null)
                {
                    if (diagonal1.piece.colour != this.colour)
                    {
                        possibleMoves.Add((diagonal1.row, diagonal1.col));
                    }
                }
            }

            Square diagonal2 = ge.GetSquareAtPos(row + colourModifier, col - colourModifier);

            if (diagonal2 != null)
            {
                if (diagonal2.piece != null)
                {
                    if (diagonal2.piece.colour != this.colour)
                    {
                        possibleMoves.Add((diagonal2.row, diagonal2.col));
                    }
                }
            }
        }
    }
}
