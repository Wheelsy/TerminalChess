using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class Piece
    {
        // Colours a piece can be
        public enum Colour
        {
            White,
            Black
        }

        public string Name { get; init; }
        public Colour colour { get; }
        public int Value { get; set; }

        protected List<(int, int)> possibleMoves;
        protected int modifier;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="colour"></param>
        public Piece(string name, int value, Colour colour)
        {
            this.Name = name;
            this.Value = value;
            this.colour = colour;
            possibleMoves = new();
        }

        public List<(int, int)> GetPossibleMoves(int row, int col, GameEngine ge)
        {
            if (possibleMoves.Count > 0)
            {
                possibleMoves.Clear();
            }
            CalculatePossibleMoves(row, col, ge);
            return possibleMoves;
        }

        /// <summary>
        /// Calculate possible moves of a piece. This is overriden by childeren.
        /// </summary>
        protected virtual void CalculatePossibleMoves(int row, int col, GameEngine ge)
        {
            this.modifier = (colour == Colour.White) ? 1 : -1;
        }
    }
}