using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalChess
{
    internal class Square
    {
        public int col { get; init; }
        public int row { get; init; }
        public Piece piece { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="piece"></param>
        public  Square(int col, int row, Piece piece)
        {
            this.col = col;
            this.row = row;
            this.piece = piece;
        }
    }
}
