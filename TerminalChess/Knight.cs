using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class Knight : Piece
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="colour"></param>
        public Knight(Colour colour) : base("", 3, colour)
        {
            // Set append the name string with the appropraite colour code
            Name += (colour == Piece.Colour.Black) ? "N".Pastel(Color.Chocolate) : "N".Pastel(Color.SandyBrown);
        }

        /// <summary>
        /// Overriden method to caluculate the legal moves for a Knight
        /// </summary>
        protected override void CalculatePossibleMoves(int row, int col, Board board)
        {
           base.CalculatePossibleMoves(row, col, board);

            // Check if the square 2 up and 1 left is within the board boundaries (L shape move)
            Square frontLeft = board.GetSquareAtPos(row + (2 * colourModifier), col - (1 * colourModifier));

            if (frontLeft != null)
            {
                if (frontLeft.piece != null)
                {
                    if (frontLeft.piece.colour != this.colour)
                    {
                        possibleMoves.Add((frontLeft.row, frontLeft.col));
                    }
                }
                else
                {
                        possibleMoves.Add((frontLeft.row, frontLeft.col));
                }
            }

            // Check if the square 2 up and 1 right is within the board boundaries (L shape move)
            Square frontRight = board.GetSquareAtPos(row + (2 * colourModifier), col + (1 * colourModifier));

            if (frontRight != null)
            {
                if (frontRight.piece != null)
                {
                    if (frontRight.piece.colour != this.colour)
                    {
                        possibleMoves.Add((frontRight.row, frontRight.col));
                    }
                }
                else
                {
                    possibleMoves.Add((frontRight.row, frontRight.col));
                }
            }

            // Check if the square 1 up and 2 left is within the board boundaries (L shape move)
            Square left1 = board.GetSquareAtPos(row + (1 * colourModifier), col - (2 * colourModifier));

            if (left1 != null)
            {
                if (left1.piece != null)
                {
                    if (left1.piece.colour != this.colour)
                    {
                        possibleMoves.Add((left1.row, left1.col));
                    }
                }
                else
                {
                    possibleMoves.Add((left1.row, left1.col));
                }
            }

            // Check if the square 1 down and 2 left is within the board boundaries (L shape move)
            Square left2 = board.GetSquareAtPos(row - (1 * colourModifier), col - (2 * colourModifier));

            if (left2 != null)
            {
                if (left2.piece != null)
                {
                    if (left2.piece.colour != this.colour)
                    {
                        possibleMoves.Add((left2.row, left2.col));
                    }
                }
                else
                {
                    possibleMoves.Add((left2.row, left2.col));
                }
            }

            // Check if the square 1 up and 2 right is within the board boundaries (L shape move)
            Square right1 = board.GetSquareAtPos(row + (1 * colourModifier), col + 2 * colourModifier);

            if (right1 != null)
            {
                if (right1.piece != null)
                {
                    if (right1.piece.colour != this.colour)
                    {
                        possibleMoves.Add((right1.row, right1.col));
                    }
                }
                else
                {
                    possibleMoves.Add((right1.row, right1.col));
                }
            }

            // Check if the square 1 down and 2 right is within the board boundaries (L shape move)
            Square right2 = board.GetSquareAtPos(row - (1 * colourModifier), col + 2 * colourModifier);

            if (right2 != null)
            {
                if (right2.piece != null)
                {
                    if (right2.piece.colour != this.colour)
                    {
                        possibleMoves.Add((right2.row, right2.col));
                    }
                }
                else
                {
                    possibleMoves.Add((right2.row, right2.col));
                }
            }

            // Check if the square 2 down and 1 left is within the board boundaries (L shape move)
            Square backLeft = board.GetSquareAtPos(row - (2 * colourModifier), col - (1 * colourModifier));

            if (backLeft != null)
            {
                if (backLeft.piece != null)
                {
                    if (backLeft.piece.colour != this.colour)
                    {
                        possibleMoves.Add((backLeft.row, backLeft.col));
                    }
                }
                else
                {
                    possibleMoves.Add((backLeft.row, backLeft.col));
                }
            }

            // Check if the square 2 down and 1 right is within the board boundaries (L shape move)
            Square backRight = board.GetSquareAtPos(row - (2 * colourModifier), col + (1 * colourModifier));

            if (backRight != null)
            {
                if (backRight.piece != null)
                {
                    if (backRight.piece.colour != this.colour)
                    {
                        possibleMoves.Add((backRight.row, backRight.col));
                    }
                }
                else
                {
                    possibleMoves.Add((backRight.row, backRight.col));
                }
            }
        }
    }
}
