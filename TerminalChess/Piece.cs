using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class Piece
    {
        public enum Colour
        {
            White,
            Black
        }

        public string name { get; }
        public Colour color { get; }

        private int value;
        private List<string> possibleMoves;

        public Piece(string name, int value, Colour colour)
        {
            this.name = name;
            this.value = value;
            this.color = colour;
        }

        protected virtual void CalculatePossibleMoves()
        {

        }
    }
}