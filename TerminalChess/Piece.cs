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
        public bool HasMoved { get; set; }

        public int col { get; set; }
        public int row { get; set; }    

        protected List<(int, int)> possibleMoves;
        protected int colourModifier;

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
            HasMoved = false;
        }

        public List<(int, int)> GetPossibleMoves(int row, int col, Board board)
        {
            if (possibleMoves.Count > 0)
            {
                possibleMoves.Clear();
            }

            Task.Run(() => CalculatePossibleMoves(row, col, board)).Wait(); // Start the calculation on a separate thread
            return possibleMoves.ToList(); // Return a copy of possibleMoves to ensure thread safety
        }

        /// <summary>
        /// Calculate possible moves of a piece. This is overriden by childeren.
        /// </summary>
        protected virtual void CalculatePossibleMoves(int row, int col, Board board)
        {
            this.colourModifier = (colour == Colour.White) ? 1 : -1;
        }
    }
}