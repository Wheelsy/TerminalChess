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

        public string name { get; init; }
        public Colour colour { get; }
        private int value { get; set; }
        public List<string> possibleMoves { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="colour"></param>
        public Piece(string name, int value, Colour colour)
        {
            this.name = name;
            this.value = value;
            this.colour = colour;
        }

        /// <summary>
        /// Calculate possible moves of a piece. This is overriden by childeren.
        /// </summary>
        protected virtual void CalculatePossibleMoves()
        {

        }
    }
}