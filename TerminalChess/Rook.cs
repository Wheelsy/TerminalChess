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
            Name += (colour == Piece.Colour.Black) ? "R".Pastel(Color.Chocolate) : "R".Pastel(Color.SandyBrown);
        }

        /// <summary>
        /// Overriden method to caluculate the legal moves for a Rook
        /// </summary>
        protected override void CalculatePossibleMoves(int row, int col, GameEngine ge)
        {
            base.CalculatePossibleMoves(row, col, ge);

            CheckForwardAndBackMoves(row, col, 1, ge); // Check up moves
            CheckForwardAndBackMoves(row, col, -1, ge); // Check down moves
            CheckLeftAndRightMoves(row, col, 1, ge); // Check right moves
            CheckLeftAndRightMoves(row, col, -1, ge); // Check left moves
        }

        /// <summary>
        /// Loop through the forward and backward column of squares
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="rowModifier"></param>
        /// <param name="ge"></param>
        private void CheckForwardAndBackMoves(int row, int col, int rowModifier, GameEngine ge)
        {
            for (int i = 1; i < 8; i++)
            {
                Square sqr = ge.GetSquareAtPos(row + i * rowModifier, col);

                if (sqr == null)
                {
                    break;
                }

                if (sqr.piece != null)
                {
                    if (sqr.piece.colour != this.colour)
                    {
                        possibleMoves.Add((sqr.row, sqr.col));
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                possibleMoves.Add((sqr.row, sqr.col));
            }
        }

        /// <summary>
        /// Loop through the left and right row of squares
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="colModifier"></param>
        /// <param name="ge"></param>
        private void CheckLeftAndRightMoves(int row, int col, int colModifier, GameEngine ge)
        {
            for (int i = 1; i < 8; i++)
            {
                Square sqr = ge.GetSquareAtPos(row, col + i * colModifier);

                if (sqr == null)
                {
                    break;
                }

                if (sqr.piece != null)
                {
                    if (sqr.piece.colour != this.colour)
                    {
                        possibleMoves.Add((sqr.row, sqr.col));
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                possibleMoves.Add((sqr.row, sqr.col));
            }
        }
    }
}
