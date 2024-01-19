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
        public enum Color
        {
            White,
            Black
        }

        private string name;
        private int value;
        private List<string> possibleMoves;
        private Color color;

        public Piece(string name, int value, Color colour)
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